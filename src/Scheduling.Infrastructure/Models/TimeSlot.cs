using System;

namespace Scheduling.Infrastructure.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int UserId { get; set; }
        
        public bool IsValid
        {
            get
            {
                return StartTime != new DateTime() && EndTime != new DateTime() && UserId > 0;
            }
        }
    }

}
