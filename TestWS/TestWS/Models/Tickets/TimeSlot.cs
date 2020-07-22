using System;

namespace TestWS.Models.Tickets
{
    public class TimeSlot
    {
        public TimeSlot()
        {
            RequestedSeats = new TimeslotSeatRequest[0];
        }
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int MovieID { get; set; }
        public int HallID { get; set; }
        public int TariffID { get; set; }
        public TimeslotSeatRequest[] RequestedSeats { get; set; }
    }
}