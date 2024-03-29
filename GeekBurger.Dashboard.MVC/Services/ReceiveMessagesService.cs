﻿using GeekBurger.Dashboard.MVC.Configuration;
using GeekBurger.Dashboard.MVC.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.Dashboard.MVC.Services
{
    public class ReceiveMessagesService
    {
        private readonly string _topicName;
        private static ServiceBusConfiguration _serviceBusConfiguration;
        private readonly string _subscriptionName;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly ILogger<ReceiveMessagesService> _logger;

        public ReceiveMessagesService(IHubContext<MessageHub> hubContext, ILogger<ReceiveMessagesService> logger,
            string topic, string subscription, string filterName = null, string filter = null)
        {
            _logger = logger;
            _hubContext = hubContext;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _serviceBusConfiguration = configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            _topicName = topic;
            _subscriptionName = subscription;

            ReceiveMessages(filterName, filter);
        }

        private void ReceiveMessages(string filterName = null, string filter = null)
        {
            var subscriptionClient = new SubscriptionClient(_serviceBusConfiguration.ConnectionString, _topicName, _subscriptionName);

            var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };

            if (filterName != null && filter != null)
            {
                const string defaultRule = "$default";

                if (subscriptionClient.GetRulesAsync().Result.Any(x => x.Name == defaultRule))
                    subscriptionClient.RemoveRuleAsync(defaultRule).Wait();

                if (subscriptionClient.GetRulesAsync().Result.All(x => x.Name != filterName))
                    subscriptionClient.AddRuleAsync(new RuleDescription
                    {
                        Filter = new CorrelationFilter { Label = filter },
                        Name = filterName
                    }).Wait();
            }

            subscriptionClient.RegisterMessageHandler(Handle, mo);
        }

        private Task Handle(Message message, CancellationToken arg2)
        {
            var messageString = "";
            if (message.Body != null)
                messageString = Encoding.UTF8.GetString(message.Body);

            List<ProductToGetFormat> products = null;
            if (message.Label == "showproductslist")
                products = JsonConvert.DeserializeObject<List<ProductToGetFormat>>(messageString);

            _hubContext.Clients.All.SendAsync(_topicName, message.Label, products ?? (object)messageString);

            return Task.CompletedTask;
        }

        private Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            _logger.LogError($"Message handler encountered an exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            _logger.LogError($"- Endpoint: {context.Endpoint}, Path: {context.EntityPath}, Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
