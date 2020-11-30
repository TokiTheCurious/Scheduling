using Scheduling.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduling.Infrastructure.Business.Results
{
    public class CreateTimeSlotResult
    {
        public string Message { get; set; }
        public TimeSlot CreatedTimeSlot { get; set; }
    }
}
