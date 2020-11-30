using Scheduling.Infrastructure.Models;
using System;

namespace Scheduling.Tests
{
    public class StaticTimeSlots
    {
        public int MyProperty { get; set; }

        public static TimeSlot partialTimeSlot
        {
            get { return new TimeSlot { StartTime = DateTime.Parse("2020-10-10"), EndTime = DateTime.Parse("2020-10-10 00:30") }; }
        }

        public static TimeSlot slot_10to1030_user1
        {
            get { return new TimeSlot { StartTime = DateTime.Parse("2020-10-10 10:00"), EndTime = DateTime.Parse("2020-10-10 10:30"), UserId = 1 }; }
        }
        public static TimeSlot slot_1015to1045_user1
        {
            get { return new TimeSlot { StartTime = DateTime.Parse("2020-10-10 10:15"), EndTime = DateTime.Parse("2020-10-10 10:45"), UserId = 1 }; }
        }

        public static TimeSlot slot_10to1030_user2
        {
            get { return new TimeSlot { StartTime = DateTime.Parse("2020-10-10 10:00"), EndTime = DateTime.Parse("2020-10-10 10:30"), UserId = 2 }; }
        }

        public static TimeSlot slot_1030to11_user1
        {
            get { return new TimeSlot { StartTime = DateTime.Parse("2020-10-10 10:30"), EndTime = DateTime.Parse("2020-10-10 11:00"), UserId = 1 }; }
        }
        public static TimeSlot slot_10to11_user1
        {
            get { return new TimeSlot { StartTime = DateTime.Parse("2020-10-10 10:00"), EndTime = DateTime.Parse("2020-10-10 11:00"), UserId = 1 }; }
        }
        public static TimeSlot slot_unscheduled_user1
        {
            get { return new TimeSlot { StartTime = DateTime.Parse("5000-10-10 10:00"), EndTime = DateTime.Parse("5000-10-10 11:00"), UserId = 1 }; }
        }
    }
}