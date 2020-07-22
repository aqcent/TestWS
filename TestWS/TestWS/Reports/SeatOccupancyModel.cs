using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Reports
{
    public class SeatOccupancyModel
    {
        public IEnumerable<SeatOccupancyRow> Rows { get; set; }
    }
}