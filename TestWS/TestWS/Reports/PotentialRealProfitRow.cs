using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Reports
{
    public class PotentialRealProfitRow
    {
        public string Name { get; set; }
        public int RealProfit { get; set; }
        public int PurchasedTickets { get; set; }
        public int PotentialProfit { get; set; }
        public int ReservedTickets { get; set; }
    }
}