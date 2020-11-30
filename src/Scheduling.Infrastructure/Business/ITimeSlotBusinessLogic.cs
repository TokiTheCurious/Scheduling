using Scheduling.Infrastructure.Business.Results;
using Scheduling.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduling.Infrastructure.Business
{
    public interface ITimeSlotBusinessLogic
    {
        Task<IEnumerable<TimeSlot>> Get();
        Task<IsAvailableTimeSlotResult> IsAvailable(TimeSlot timeSlot);
        Task<CreateTimeSlotResult> Create(TimeSlot timeSlot);
        Task<DeleteTimeSlotResult> Delete(TimeSlot timeSlot);
        Task<DeleteTimeSlotResult> DeleteId(int timeSlotId);
    }
}
