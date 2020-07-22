using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Models.Tickets
{
    public class TimeslotSeatRequest
    {
        public int Row { get; set; }
        public int Seat { get; set; }
        public RequestStatus Status { get; set; }        
    }
}