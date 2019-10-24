using System.Collections.Generic;

namespace GeekBurger.Dashboard.Contract
{
    public class UsersWithLessOfferResponse
    {
        public int Users { get; set; }
        public IEnumerable<string> Restrictions { get; set; }
    }
}
