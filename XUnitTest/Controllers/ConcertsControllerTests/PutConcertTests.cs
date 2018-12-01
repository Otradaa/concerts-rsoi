using ConcertsService.Controllers;
using ConcertsService.Data;
using ConcertsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest.Controllers.ConcertsControllerTests
{
    public class PutConcertTests
    {
        [Fact]
        public async Task PutConcertReturnsBadRequestResultModelStateTest()
        {
            // Arrange & Act
            var mockRepo = new Mock<IConcertRepository>();
            var controller = new ConcertsController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.PutConcert(id: 0, concert: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutConcertReturnsBadRequestResultIdTest()
        {
            // Arrange
            int testId = 2;
            Concert concert = GetTestConcerts()[0];
            var mockRepo = new Mock<IConcertRepository>();
            var controller = new ConcertsController(mockRepo.Object);
           
            // Act
            var result = await controller.PutConcert(testId, concert);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutConcertReturnsNoContentResultTest()
        {
            // Arrange
            int testId = 1;
            Concert concert = GetTestConcerts().FirstOrDefault(p => p.Id == testId);
            var mockRepo = new Mock<IConcertRepository>();
            mockRepo.Setup(c => c.ChangeState(concert, EntityState.Modified)).Verifiable();
            mockRepo.Setup(c => c.SaveChanges()).Returns(Task.CompletedTask);
            var controller = new ConcertsController(mockRepo.Object);

            // Act
            var result = await controller.PutConcert(testId, concert);

            // Assert
            Assert.IsType<NoContentResult>(result);
            
        }

        [Fact]
        public async Task PutConcertThrowsDbUpdateExceptionTest()
        {
            // Arrange
            var mock = new Mock<IUpdateEntry>();
            var entries = new List<IUpdateEntry>();
            entries.Add(mock.Object);

            int testId = 1;
            Concert concert = GetTestConcerts().FirstOrDefault(p => p.Id == testId);
            var mockRepo = new Mock<IConcertRepository>();
            mockRepo.Setup(c => c.ChangeState(concert, EntityState.Modified)).Verifiable();
            mockRepo.Setup(c => c.SaveChanges()).Throws(
                new DbUpdateConcurrencyException(It.IsNotNull<string>(), entries));
            mockRepo.Setup(c => c.ConcertExists(testId)).Returns(true);
            var controller = new ConcertsController(mockRepo.Object);

            // Act Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
                controller.PutConcert(testId, concert));

        }

        [Fact]
        public async Task PutConcertReturnsNotFoundTest()
        {
            // Arrange
            var mock = new Mock<IUpdateEntry>();
            var entries = new List<IUpdateEntry>();
            entries.Add(mock.Object);

            int testId = 1;
            Concert concert = GetTestConcerts().FirstOrDefault(p => p.Id == testId);
            var mockRepo = new Mock<IConcertRepository>();
            mockRepo.Setup(c => c.ChangeState(concert, EntityState.Modified)).Verifiable();
            mockRepo.Setup(c => c.SaveChanges()).Throws(
                new DbUpdateConcurrencyException(It.IsNotNull<string>(), entries));
            mockRepo.Setup(c => c.ConcertExists(testId)).Returns(false);
            var controller = new ConcertsController(mockRepo.Object);

            // Act
            var result = await controller.PutConcert(testId, concert);

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
