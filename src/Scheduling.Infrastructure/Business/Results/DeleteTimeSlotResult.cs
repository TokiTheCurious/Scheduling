using Scheduling.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.Infrastructure.Business.Results
{
    public class DeleteTimeSlotResult
    {
        public string Message { get; private set; }
        public IEnumerable<TimeSlot> DeletedTimeSlots { get; private set; }

        public DeleteTimeSlotResult(IEnumerable<TimeSlot> deletedTimeSlots, string customMessage = null)
        {
            int numberOfDeleted = deletedTimeSlots.Count();
            Message = numberOfDeleted == 0 ?
                "No time slots deleted" :
                string.Format("{0} time slot{1} deleted", numberOfDeleted, numberOfDeleted == 1 ? "": "s");
            DeletedTimeSlots = deletedTimeSlots;

            if (!string.IsNullOrWhiteSpace(customMessage))
                Message = customMessage;

        }
    }
}
