using Scheduling.Infrastructure.Data;
using Scheduling.Infrastructure.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Scheduling.Data.Sql.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        static object insertLock = new object();
        SchedulingDbContext _context;
        public TimeSlotRepository(SchedulingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeSlot>> Get()
        {
            return await _context.TimeSlot.ToListAsync();
        }
        public TimeSlot Create(TimeSlot timeSlot)
        {
            //This is a race condition if scaled horizontally, best solution would probably be a stored procedure that inserts based on condition of a timeslot being available
            if (timeSlot.IsValid)
            {
                lock (insertLock)
                {
                    if (IsAvailable(timeSlot).Result)
                    {
                        _context.TimeSlot.Add(timeSlot);
                        _context.SaveChanges();
                    }
                }
            }
            return timeSlot;
        }

        public IEnumerable<TimeSlot> Search(TimeSlot timeSlot)
        {
            if (!timeSlot.IsValid)
                return new List<TimeSlot>();

            return _context
                .TimeSlot
                .Where(t => t.UserId == timeSlot.UserId)
                .Where(t => 
                    (t.StartTime >= timeSlot.StartTime && t.StartTime < timeSlot.EndTime) ||
                    (t.EndTime > timeSlot.StartTime && t.EndTime <= timeSlot.EndTime) ||
                    (timeSlot.StartTime >= t.StartTime && timeSlot.StartTime < t.EndTime))
                .ToList();
        }

        public async Task<TimeSlot> FindId(int timeSlotId)
        {
            if (timeSlotId <= 0)
                return null;

            return await _context.TimeSlot.FindAsync(timeSlotId);
        }

        public async Task<int> Delete(IEnumerable<TimeSlot> deletableTimeSlots)
        {
            if (deletableTimeSlots.Any(t => !t.IsValid))
                return 0;

            _context.TimeSlot.RemoveRange(deletableTimeSlots);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Delete(TimeSlot deletableTimeSlot)
        {
            if (!deletableTimeSlot.IsValid)
                return 0;

            _context.TimeSlot.Remove(deletableTimeSlot);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> IsAvailable(TimeSlot timeSlot)
        {
            if (!timeSlot.IsValid)
                return false;

            return await _context
                .TimeSlot
                .Where(t => t.UserId == timeSlot.UserId)
                .FirstOrDefaultAsync(t => 
                                    (t.StartTime >= timeSlot.StartTime && t.StartTime < timeSlot.EndTime) ||
                                    (t.EndTime > timeSlot.StartTime && t.EndTime <= timeSlot.EndTime) ||
                                    (timeSlot.StartTime >= t.StartTime && timeSlot.StartTime < t.EndTime)) == null;
        }
    }
}
