using Scheduling.Infrastructure.Business;
using Scheduling.Infrastructure.Models;
using Scheduling.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Scheduling.Infrastructure.Business.Results;

namespace Scheduling.Business
{
    public class TimeSlotBusinessLogic : ITimeSlotBusinessLogic
    {
        ITimeSlotRepository _repository;
        public TimeSlotBusinessLogic(ITimeSlotRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TimeSlot>> Get()
        {
            return await _repository.Get();
        }

        public async Task<IsAvailableTimeSlotResult> IsAvailable(TimeSlot timeSlot)
        {
            return new IsAvailableTimeSlotResult { IsAvailable = await _repository.IsAvailable(timeSlot) };
        }

        public async Task<CreateTimeSlotResult> Create(TimeSlot timeSlot)
        {
            if(await _repository.IsAvailable(timeSlot))
            {
                var createResult = _repository.Create(timeSlot);
                if (createResult?.TimeSlotId > 0)
                    return new CreateTimeSlotResult { Message = "Time slot created", CreatedTimeSlot = createResult };
            }
            return new CreateTimeSlotResult { Message = "Time slot was unavailable" };
        }

        public async Task<DeleteTimeSlotResult> Delete(TimeSlot timeSlot)
        {
            IEnumerable<TimeSlot> toDelete = _repository.Search(timeSlot);

            if(toDelete.Count() > 0)
                await _repository.Delete(toDelete);

            return new DeleteTimeSlotResult(toDelete);
        }

        public async Task<DeleteTimeSlotResult> DeleteId(int timeSlotId)
        {
            TimeSlot toDelete = await _repository.FindId(timeSlotId);

            if(toDelete?.TimeSlotId > 0)
                await _repository.Delete(toDelete);

            return new DeleteTimeSlotResult(new List<TimeSlot> { toDelete }.Where(d => d?.TimeSlotId > 0));
        }

    }
}
