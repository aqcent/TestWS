using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Models.Tickets
{
    public class HallInfo
    {
        public Tariff CurrentTariff { get; set; }

        public int CurrentTimeslotID {get; set;}
        public TimeslotSeatRequest[] RequestedSeats { get; set; }
    }
}