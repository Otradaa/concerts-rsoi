using VenuesService.Controllers;
using VenuesService.Data;
using VenuesService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Controllers.SchedulesControllerTests
{
    public class PostScheduleTests
    {
        [Fact]
        public async Task PostScheduleReturnsCreaterAtActionResultTest()
        {
            // Arrange
            int testId = 1;
            Schedule schedule = GetTestSchedules().FirstOrDefault(p => p.Id == testId);
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var mockRepo = new Mock<IVenuesRepository>();
            mockRepo.Setup(c => c.AddSchedule(schedule)).Verifiable();
            mockRepo.Setup(c => c.SaveChanges())
                .Returns(Task.CompletedTask);
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.PostSchedule(schedule);

            // Assert
            var requestResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<Schedule>(requestResult.Value);
            Assert.Equal(schedule.Id, model.Id);
            Assert.Equal(schedule.VenueId, model.VenueId);
            Assert.Equal(schedule.ConcertId, model.ConcertId);
            Assert.Equal(schedule.Date, model.Date);
        }

        [Fact]
        public async Task PostScheduleReturnsBadRequestResultTest()
        {
            // Arrange & Act
            var mockRepo = new Mock<IVenuesRepository>();
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.PostSchedule(schedule: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private List<Schedule> GetTestSchedules()
        {
            var schedules = new List<Schedule>();
            schedules.Add(new Schedule()
            {
                Id = 1,
                VenueId = 1,
                ConcertId = 1,
                Date = DateTime.Parse("01.01.2020")
            });
            schedules.Add(new Schedule()
            {
                Id = 2,
                VenueId = 2,
                ConcertId = 2,
                Date = DateTime.Parse("02.02.2020")
            });
            schedules.Add(new Schedule()
            {
                Id = 3,
                VenueId = 3,
                ConcertId = 3,
                Date = DateTime.Parse("03.03.2020")
            });
            return schedules;
        }
    }
}
