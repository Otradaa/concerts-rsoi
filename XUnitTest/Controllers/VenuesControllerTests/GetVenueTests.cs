using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
//using Gateway.Models;
using VenuesService.Controllers;
using Moq;
using VenuesService.Models;
using Microsoft.EntityFrameworkCore;
using VenuesService.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XUnitTest.Controllers.VenuesControllerTests
{
    public class GetVenueTests
    {
        [Fact]
        public async Task GetVenueReturnsVenueByIdTest()
        {
            // Arrange
            int testId = 1;
            var mockLogger = new Mock<ILogger<VenuesController>>();
            var mockRepo = new Mock<IVenuesRepository>();
            mockRepo.Setup(c => c.GetVenue(testId))
                .ReturnsAsync(GetTestVenues().FirstOrDefault(p => p.Id == testId));
            var controller = new VenuesController(mockRepo.Object, mockLogger.Object);

            // Act
            var result = await controller.GetVenue(testId);

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Venue>(requestResult.Value);
            Assert.Equal("TestOne", model.Name);
            Assert.Equal("TestOne", model.Address);
            Assert.Equal("TestOne", model.Phone);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async Task GetVenueReturnsBadRequestByIdTest()
        {
            // Arrange
            int testId = 1;
            var mockLogger = new Mock<ILogger<VenuesController>>();
            var mockRepo = new Mock<IVenuesRepository>();
            mockRepo.Setup(c => c.GetVenue(testId))
                .ReturnsAsync(GetTestVenues().FirstOrDefault(p => p.Id == testId));
            var controller = new VenuesController(mockRepo.Object, mockLogger.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.GetVenue(testId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            //var model = Assert.IsType<Venue>(viewResult.Value);
            //Assert.Equal("Group One", model.Name);
            //Assert.Equal(testId, model.Id);
        }

        private List<Venue> GetTestVenues()
        {
            var venues = new List<Venue>();
            venues.Add(new Venue()
            {
                Id = 1,
                Name = "TestOne",
                Address = "TestOne",
                Phone = "TestOne"
            });
            venues.Add(new Venue()
            {
                Id = 2,
                Name = "TestTwo",
                Address = "TestTwo",
                Phone = "TestTwo"
            });
            venues.Add(new Venue()
            {
                Id = 3,
                Name = "TestThree",
                Address = "TestThree",
                Phone = "TestThree"
            });
            return venues;
        }
    }
}
