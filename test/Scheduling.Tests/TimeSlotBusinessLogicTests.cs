using Moq;
using Newtonsoft.Json;
using Scheduling.Business;
using Scheduling.Infrastructure.Business.Results;
using Scheduling.Infrastructure.Data;
using Scheduling.Infrastructure.Models;
using System.Collections.Generic;
using Xunit;

namespace Scheduling.Tests
{
    public class TimeSlotBusinessLogicTests
    {
        private readonly Mock<ITimeSlotRepository> _mockRepo;
        private TimeSlotBusinessLogic BusinessLogic
        {
            get
            {
                return new TimeSlotBusinessLogic(_mockRepo.Object);
            }
        }
        public TimeSlotBusinessLogicTests()
        {
            _mockRepo = new Mock<ITimeSlotRepository>();
        }


        private async void IsAvailableWrapper(bool expectedValue)
        {
            _mockRepo.Setup(r => r.IsAvailable(It.IsAny<TimeSlot>())).ReturnsAsync(expectedValue);
            var res = await BusinessLogic.IsAvailable(StaticTimeSlots.partialTimeSlot);

            _mockRepo.Verify(r => r.IsAvailable(It.IsAny<TimeSlot>()), Times.Once);
            Assert.Equal(expectedValue,res.IsAvailable);
        } 

        [Fact]
        public void IsAvailableTest_False()
        {
            IsAvailableWrapper(false);
        }
        [Fact]
        public void IsAvailableTest_True()
        {
            IsAvailableWrapper(true);
        }


        [Fact]
        public async void CreatTest_Success()
        {
            var expectedResult = new CreateTimeSlotResult
            {
                CreatedTimeSlot = StaticTimeSlots.slot_10to1030_user1,
                Message = "Time slot created"
            };
            expectedResult.CreatedTimeSlot.TimeSlotId = 1;
            _mockRepo.Setup(r => r.Create(It.IsAny<TimeSlot>())).Returns<TimeSlot>(t =>
            {
                t.TimeSlotId = 1;
                return t;
            });

            _mockRepo.Setup(r => r.IsAvailable(It.IsAny<TimeSlot>())).ReturnsAsync(true);

            var result = await BusinessLogic.Create(StaticTimeSlots.slot_10to1030_user1);

            _mockRepo.Verify(r => r.IsAvailable(It.IsAny<TimeSlot>()), Times.Once);
            _mockRepo.Verify(r => r.Create(It.IsAny<TimeSlot>()), Times.Once);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async void CreateTest_NotAvailableResponse()
        {
            var expectedResult = new CreateTimeSlotResult
            {
                Message = "Time slot was unavailable"
            };
            _mockRepo.Setup(r => r.Create(It.IsAny<TimeSlot>())).Returns<TimeSlot>(t =>
            {
                t.TimeSlotId = 1;
                return t;
            });

            _mockRepo.Setup(r => r.IsAvailable(It.IsAny<TimeSlot>())).ReturnsAsync(false);

            var result = await BusinessLogic.Create(StaticTimeSlots.slot_10to1030_user1);

            _mockRepo.Verify(r => r.IsAvailable(It.IsAny<TimeSlot>()), Times.Once);
            _mockRepo.Verify(r => r.Create(It.IsAny<TimeSlot>()), Times.Never);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async void CreateTest_HandlesNullResponse()
        {
            var expectedResult = new CreateTimeSlotResult
            {
                Message = "Time slot was unavailable"
            };

            _mockRepo.Setup(r => r.Create(It.IsAny<TimeSlot>())).Returns<TimeSlot>(t => null);
            _mockRepo.Setup(r => r.IsAvailable(It.IsAny<TimeSlot>())).ReturnsAsync(true);

            var result = await BusinessLogic.Create(StaticTimeSlots.slot_10to1030_user1);

            _mockRepo.Verify(r => r.IsAvailable(It.IsAny<TimeSlot>()), Times.Once);
            _mockRepo.Verify(r => r.Create(It.IsAny<TimeSlot>()), Times.Once);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async void CreateTest_FailsWithUnavailableMessage()
        {
            var expectedResult = new CreateTimeSlotResult
            {
                Message = "Time slot was unavailable"
            };

            _mockRepo.Setup(r => r.Create(It.IsAny<TimeSlot>())).Returns<TimeSlot>(t => StaticTimeSlots.slot_10to1030_user1);
            _mockRepo.Setup(r => r.IsAvailable(It.IsAny<TimeSlot>())).ReturnsAsync(true);

            var result = await BusinessLogic.Create(StaticTimeSlots.slot_10to1030_user1);

            _mockRepo.Verify(r => r.IsAvailable(It.IsAny<TimeSlot>()), Times.Once);
            _mockRepo.Verify(r => r.Create(It.IsAny<TimeSlot>()), Times.Once);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));

        }

        [Fact]

        public async void DeleteTest_Single_Sucess()
        {
            var expectedResult = new DeleteTimeSlotResult(new[] { StaticTimeSlots.slot_10to1030_user1 });

            _mockRepo.Setup(r => r.FindId(It.IsAny<int>())).ReturnsAsync(StaticTimeSlots.slot_10to1030_user1);
            _mockRepo.Setup(r => r.Delete(It.IsAny<TimeSlot>())).ReturnsAsync(1);

            var result = await BusinessLogic.DeleteId(1);

            _mockRepo.Verify(r => r.FindId(It.IsAny<int>()), Times.Once);
            _mockRepo.Verify(r => r.Delete(It.IsAny<TimeSlot>()), Times.Once);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult),JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async void DeleteTest_DoesNotDeleteIfNotFound()
        {
            var expectedResult = new DeleteTimeSlotResult(new List<TimeSlot>());

            _mockRepo.Setup(r => r.Create(It.IsAny<TimeSlot>())).Returns<TimeSlot>(t => null);
            _mockRepo.Setup(r => r.FindId(It.IsAny<int>())).ReturnsAsync(new TimeSlot());
            _mockRepo.Setup(r => r.Delete(It.IsAny<TimeSlot>())).ReturnsAsync(1);

            var result = await BusinessLogic.DeleteId(1);

            _mockRepo.Verify(r => r.FindId(It.IsAny<int>()), Times.Once);
            _mockRepo.Verify(r => r.Delete(It.IsAny<TimeSlot>()), Times.Never);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async void DeleteTest_Batch_Sucess()
        {
            var expectedResult = new DeleteTimeSlotResult(new[] { StaticTimeSlots.slot_10to1030_user1, StaticTimeSlots.slot_1030to11_user1 });

            _mockRepo.Setup(r => r.Search(It.IsAny<TimeSlot>())).Returns<TimeSlot>(t => new[] { StaticTimeSlots.slot_10to1030_user1, StaticTimeSlots.slot_1030to11_user1});
            _mockRepo.Setup(r => r.Delete(It.IsAny<IEnumerable<TimeSlot>>())).ReturnsAsync(1);

            var result = await BusinessLogic.Delete(StaticTimeSlots.slot_10to11_user1);

            _mockRepo.Verify(r => r.Search(It.IsAny<TimeSlot>()), Times.Once);
            _mockRepo.Verify(r => r.Delete(It.IsAny<IEnumerable<TimeSlot>>()), Times.Once);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

        [Fact]
        public async void DeleteTest_Batch_DoesNotDeleteIfNotFound()
        {
            var expectedResult = new DeleteTimeSlotResult(new List<TimeSlot> { });

            _mockRepo.Setup(r => r.Search(It.IsAny<TimeSlot>())).Returns<TimeSlot>(t => new List<TimeSlot> { });
            _mockRepo.Setup(r => r.Delete(It.IsAny<IEnumerable<TimeSlot>>())).ReturnsAsync(1);

            var result = await BusinessLogic.Delete(StaticTimeSlots.slot_10to11_user1);

            _mockRepo.Verify(r => r.Search(It.IsAny<TimeSlot>()), Times.Once);
            _mockRepo.Verify(r => r.Delete(It.IsAny<IEnumerable<TimeSlot>>()), Times.Never);

            Assert.Equal(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(result));
        }

    }
}
