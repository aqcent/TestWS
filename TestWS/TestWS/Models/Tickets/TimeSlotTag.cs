using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Models.Tickets
{
    public class TimeSlotTag
    {
        public int TimeSlotID { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Cost { get; set; }
    }
}