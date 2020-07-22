using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Reports
{
    public class SeatOccupancyRow
    {
        public DateTime Timeslot { get; set; }
        public string MovieName { get; set; }
        public int SeatCount { get; set; }
        public int SoldTickets { get; set; }
    }
}