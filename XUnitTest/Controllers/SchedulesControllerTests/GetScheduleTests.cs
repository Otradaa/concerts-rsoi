using Gateway.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenuesService.Controllers;
using VenuesService.Data;
using Xunit;

namespace XUnitTest.Controllers.SchedulesControllerTests
{
    public class GetScheduleTests
    {
        /*[Fact]
        public async Task GetScheduleReturnsScheduleByIdTest()
        {
            // Arrange
            int testId = 1;
            var mockLogger = new Mock<ILogger<SchedulesController>>();
            var mockRepo = new Mock<IVenuesRepository>();
            mockRepo.Setup(c => c.GetSchedule(testId))
               .ReturnsAsync(GetTestSchedules().FirstOrDefault(p => p.Id == testId));
            var controller = new SchedulesController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.GetSchedule(testId);

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Schedule>(requestResult.Value);
            Assert.Equal(testId, model.Id);
            Assert.Equal(testId, model.VenueId);
            Assert.Equal(testId, model.ConcertId);
            Assert.Equal(DateTime.Parse("01.01.2020"), model.Date);
        }*/



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
