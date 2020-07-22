using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Models.Tickets
{
    public class TicketsJSONModel
    {
        public Movie[] Movies { get; set; }
        public Hall[] Halls { get; set; }
        public TimeSlot[] TimeSlots { get; set; }
        public Tariff[] Tariffs { get; set; }
    }
}