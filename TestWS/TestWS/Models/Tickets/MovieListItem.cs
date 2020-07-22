using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWS.Models.Tickets
{
    public class MovieListItem
    {
        public MovieListItem()
        {
            AvailableTimeSlots = new TimeSlotTag[0];
        }
        public Movie Movie { get; set; }
        public TimeSlotTag[] AvailableTimeSlots { get; set; }
    }
}