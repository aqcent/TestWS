using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Models.Tickets
{
    public class SeatsProcessRequest
    {
        public int TimeslotID { get; set; }
        public SeatsRequest SeatsRequest { get; set; }

        public RequestStatus SelectedStatus { get; set; }
    }
}