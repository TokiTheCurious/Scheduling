using Scheduling.Infrastructure.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Scheduling.WebApi.ApiModels
{
    public class TimeSlotRequest
    {
        [Required]
        [Range(1, 99999999999)]
        public Int64 StartTimestamp { get; set; }

        [Required]
        [Range(1, 1440)]
        public int Duration { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int UserId { get; set; }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                if (_startTime == new DateTime())
                {
                    var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    _startTime = dt.AddSeconds(StartTimestamp);
                }

                return _startTime;

            }
        }

        public DateTime EndTime
        {
            get
            {
                return StartTime.AddMinutes(Duration);
            }
        }

        public TimeSlot ToTimeSlot()
        {
            return new TimeSlot { UserId = UserId, StartTime = StartTime, EndTime = EndTime };
        }
    }
}
