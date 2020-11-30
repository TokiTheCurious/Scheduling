using Microsoft.EntityFrameworkCore;
using Scheduling.Data.Sql;
using Scheduling.Data.Sql.Repositories;
using Scheduling.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Scheduling.Tests
{
    public class TimeSlotRepositoryTests
    {
        private readonly SchedulingDbContext _context;
        private readonly TimeSlotRepository _repository;
        public TimeSlotRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<SchedulingDbContext>()
                .UseInMemoryDatabase("TimeSlotsTest")
                .Options;
            _context = new SchedulingDbContext(options);
            _repository = new TimeSlotRepository(_context);
        }


        [Fact]
        public void CreateTest()
        {
            Assert.Equal(1,_repository.Create(StaticTimeSlots.slot_10to1030_user1).TimeSlotId);
            Assert.Equal(2,_repository.Create(StaticTimeSlots.slot_10to1030_user2).TimeSlotId);
        }

        [Fact]
        public void CreateTest_DoesNotMakeCollidingSchedule()
        {
            _repository.Create(StaticTimeSlots.slot_1030to11_user1);
            Assert.Equal(0, _repository.Create(StaticTimeSlots.slot_1030to11_user1).TimeSlotId);

        }

        [Fact]
        public void CreateTest_DoesNotCreatePartialModel()
        {
            Assert.Equal(0, _repository.Create(new Infrastructure.Models.TimeSlot()).TimeSlotId);
        }

        [Fact]
        public void SearchTest_FindsOneTimeSlot()
        {
            _repository.Create(StaticTimeSlots.slot_10to1030_user1);
            _repository.Create(StaticTimeSlots.slot_10to1030_user2);
            Assert.Single(_repository.Search(StaticTimeSlots.slot_10to1030_user1));
        }

        [Fact]
        public void SearchTest_FindsTwoTimeSlots()
        {
            _repository.Create(StaticTimeSlots.slot_10to1030_user1);
            _repository.Create(StaticTimeSlots.slot_1030to11_user1);
            _repository.Create(StaticTimeSlots.slot_10to1030_user2);
            Assert.Equal(2, _repository.Search(StaticTimeSlots.slot_1015to1045_user1).Count());
        }

        [Fact]
        public void SearchTest_FindsNoTimeSlots()
        {
            Assert.Empty(_repository.Search(StaticTimeSlots.slot_unscheduled_user1));
        }

        [Fact]
        public void SearchTest_FindsTimeSlotId()
        {
            _repository.Create(StaticTimeSlots.slot_10to1030_user1);
            Assert.NotNull(_repository.FindId(1));
        }

        [Fact]
        public async void SearchTest_FindsNoTimeSlotId()
        {
            Assert.Null(await _repository.FindId(99999999));
        }

        [Fact]
        public async void DeleteTest_DeletesOneEntry()
        {
            _repository.Create(StaticTimeSlots.slot_10to1030_user1);
            _repository.Create(StaticTimeSlots.slot_1030to11_user1);
            _repository.Create(StaticTimeSlots.slot_10to1030_user2);
            IEnumerable<TimeSlot> toDelete = _repository.Search(StaticTimeSlots.slot_10to1030_user1);
            Assert.Equal(1, await _repository.Delete(toDelete));
            TimeSlot toDelete2 = _repository.Search(StaticTimeSlots.slot_10to1030_user2).FirstOrDefault();
            Assert.Equal(1, await _repository.Delete(toDelete2));
        }

        [Fact]
        public async void DeleteTest_DeletesTwoEntries()
        {
            _repository.Create(StaticTimeSlots.slot_10to1030_user1);
            _repository.Create(StaticTimeSlots.slot_1030to11_user1);
            _repository.Create(StaticTimeSlots.slot_10to1030_user2);
            IEnumerable<TimeSlot> toDelete = _repository.Search(StaticTimeSlots.slot_1015to1045_user1);
            Assert.Equal(2, await _repository.Delete(toDelete));
        }

        [Fact]
        public async void IsAvailable_NoScheduledTime()
        {
            Assert.True(await _repository.IsAvailable(StaticTimeSlots.slot_unscheduled_user1));
        }

        [Fact]
        public async void IsAvailable_ScheduledTime()
        {
            _repository.Create(StaticTimeSlots.slot_10to1030_user1);
            Assert.False(await _repository.IsAvailable(StaticTimeSlots.slot_10to1030_user1));
        }
    }
}
