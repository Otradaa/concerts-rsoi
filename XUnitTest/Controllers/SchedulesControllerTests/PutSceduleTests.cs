using VenuesService.Controllers;
using VenuesService.Data;
using VenuesService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Controllers.SchedulesControllerTests
{
    public class PutSceduleTests
    {
        [Fact]
        public async Task PutScheduleReturnsBadRequestResultModelStateTest()
        {
            // Arrange & Act
            var mockRepo = new Mock<IVenuesRepository>();
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.PutSchedule(schedule: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

       /* [Fact]
        public async Task PutScheduleReturnsBadRequestResultIdTest()
        {
            // Arrange
            int testId = 2;
            Schedule schedule = GetTestSchedules()[0];
            var mockRepo = new Mock<IVenuesRepository>();
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.PutSchedule(schedule);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }*/

        [Fact]
        public async Task PutScheduleReturnsNoContentResultTest()
        {
            // Arrange
            int testId = 1;
            Schedule schedule = GetTestSchedules().FirstOrDefault(p => p.Id == testId);
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var mockRepo = new Mock<IVenuesRepository>();
            mockRepo.Setup(c => c.ChangeState(schedule, schedule, EntityState.Modified)).Verifiable();
            mockRepo.Setup(c => c.SaveChanges()).Returns(Task.CompletedTask);
            mockRepo.Setup(c => c.FirstSchedule(testId)).Returns(schedule);
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.PutSchedule(schedule);

            // Assert
            Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public async Task PutScheduleThrowsDbUpdateExceptionTest()
        {
            // Arrange
            var mock = new Mock<IUpdateEntry>();
            var entries = new List<IUpdateEntry>();
            entries.Add(mock.Object);

            int testId = 1;
            Schedule schedule = GetTestSchedules().FirstOrDefault(p => p.Id == testId);
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var mockRepo = new Mock<IVenuesRepository>();
            mockRepo.Setup(c => c.FirstSchedule(testId)).Returns(schedule);
            mockRepo.Setup(c => c.ChangeState(schedule, schedule, EntityState.Modified))
                .Verifiable();
            mockRepo.Setup(c => c.SaveChanges()).Throws(
                new DbUpdateConcurrencyException(It.IsNotNull<string>(), entries));
            mockRepo.Setup(c => c.ScheduleExists(testId)).Returns(true);
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);

            // Act Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
                controller.PutSchedule(schedule));

        }

        [Fact]
        public async Task PutScheduleReturnsNotFoundTest()
        {
            // Arrange
            var mock = new Mock<IUpdateEntry>();
            var entries = new List<IUpdateEntry>();
            entries.Add(mock.Object);

            int testId = 1;
            Schedule schedule = GetTestSchedules().FirstOrDefault(p => p.Id == testId);
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var mockRepo = new Mock<IVenuesRepository>();
            mockRepo.Setup(c => c.ChangeState(schedule, schedule, EntityState.Modified)).Verifiable();
            mockRepo.Setup(c => c.SaveChanges()).Throws(
                new DbUpdateConcurrencyException(It.IsNotNull<string>(), entries));
            mockRepo.Setup(c => c.ScheduleExists(testId)).Returns(false);
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.PutSchedule(schedule);

            // Assert
            Assert.IsType<NotFoundResult>(result);

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
