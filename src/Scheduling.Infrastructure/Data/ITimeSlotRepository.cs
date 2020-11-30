using Scheduling.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduling.Infrastructure.Data
{
    public interface ITimeSlotRepository
    {
        Task<IEnumerable<TimeSlot>> Get();
        Task<bool> IsAvailable(TimeSlot timeSlot);
        IEnumerable<TimeSlot> Search(TimeSlot timeSlot);
        Task<TimeSlot> FindId(int timeSlotId);
        TimeSlot Create(TimeSlot timeSlot);
        Task<int> Delete(IEnumerable<TimeSlot> deletableTimeSlots);
        Task<int> Delete(TimeSlot timeSlot);
    }
}
