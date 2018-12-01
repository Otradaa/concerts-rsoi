using ConcertsService.Controllers;
using ConcertsService.Data;
using ConcertsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Controllers.ConcertsControllerTests
{
    public class PostConcertTests
    {
        [Fact]
        public async Task PostConcertReturnsCreaterAtActionResultTest()
        {
            // Arrange
            int testId = 1;
            Concert concert = GetTestConcerts().FirstOrDefault(p => p.Id == testId);
            var mockRepo = new Mock<IConcertRepository>();
            mockRepo.Setup(c => c.AddConcert(concert))
                .Returns(EntityState.Added);
            mockRepo.Setup(c => c.SaveChanges())
                .Returns(Task.CompletedTask);
            var controller = new ConcertsController(mockRepo.Object);

            // Act
            var result = await controller.PostConcert(concert);

            // Assert
            var requestResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsType<Concert>(requestResult.Value);
            Assert.Equal(concert.Id, model.Id);
            Assert.Equal(concert.VenueId, model.VenueId);
            Assert.Equal(concert.PerfomerId, model.PerfomerId);
            Assert.Equal(concert.Date, model.Date);
        }

        [Fact]
        public async Task PostConcertReturnsBadRequestResultTest()
        {
            // Arrange & Act
            var mockRepo = new Mock<IConcertRepository>();
            var controller = new ConcertsController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.PostConcert(concert: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
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
