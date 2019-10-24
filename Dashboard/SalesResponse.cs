using System;

namespace Dashboard
{
    public class SalesResponse
    {
        public Guid StoreId { get; set; } 
        public Decimal Total { get; set; }
        public Decimal Value { get; set; }   
    }
}