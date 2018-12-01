using ConcertsService.Controllers;
using ConcertsService.Data;
using ConcertsService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest
{
    public class GetConcertTests
    {
        [Fact]
        public async Task GetConcertReturnsConcertByIdTest()
        {
            // Arrange
            int testId = 1;
            var mockRepo = new Mock<IConcertRepository>();
            mockRepo.Setup(c => c.GetConcert(testId))
                .ReturnsAsync(GetTestConcerts().FirstOrDefault(p => p.Id == testId));
            var controller = new ConcertsController(mockRepo.Object);

            // Act
            var result = await controller.GetConcert(testId);

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Concert>(requestResult.Value);
            Assert.Equal(testId, model.Id);
            Assert.Equal(testId, model.VenueId);
            Assert.Equal(testId, model.PerfomerId);
            Assert.Equal(DateTime.Parse("01.01.2020"), model.Date);
        }

        [Fact]
        public async Task GetConcertReturnsBadRequestByIdTest()
        {
            // Arrange
            int testId = 1;
            var mockRepo = new Mock<IConcertRepository>();
            mockRepo.Setup(c => c.GetConcert(testId))
                .ReturnsAsync(GetTestConcerts().FirstOrDefault(p => p.Id == testId));
            var controller = new ConcertsController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.GetConcert(testId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            //var model = Assert.IsType<Perfomer>(viewResult.Value);
            //Assert.Equal("Group One", model.Name);
            //Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async Task GetConcertReturnsNotFoundByIdTest()
        {
            // Arrange
            int testId = 0;
            var mockRepo = new Mock<IConcertRepository>();
            mockRepo.Setup(c => c.GetConcert(testId))
                .ReturnsAsync(GetTestConcerts().FirstOrDefault(p => p.Id == testId));
            var controller = new ConcertsController(mockRepo.Object);

            // Act
            var result = await controller.GetConcert(testId);

            // Assert
            Assert.IsType<NotFoundResult>(result);

        }


        private List<Concert> GetTestConcerts()
        {
            var concerts = new List<Concert>();
            concerts.Add(new Concert()
            {
                Id = 1,
                VenueId = 1,
                PerfomerId = 1,
                Date = DateTime.Parse("01.01.2020")
            });
            concerts.Add(new Concert()
            {
                Id = 2,
                VenueId = 2,
                PerfomerId = 2,
                Date = DateTime.Parse("02.02.2020")
            });
            concerts.Add(new Concert()
            {
                Id = 3,
                VenueId = 3,
                PerfomerId = 3,
                Date = DateTime.Parse("03.03.2020")
            });
            return concerts;
        }
    }
}
