using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
//using Gateway.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using Gateway.Controllers;
using Gateway.Services;
using Gateway.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace XUnitTest
{
    public class GatewayControllerTests
    {
        [Fact]
        public async Task GetAllConcertReturnsConcerts()
        {
            int testId = 1;
            var mockLogger = new Mock<ILogger<ConcertsController>>();
            var mockService = new Mock<IGatewayService>();
            mockService.Setup(c => c.GetConcerts(testId, testId))
                .ReturnsAsync(GetTestConcerts());
            mockService.Setup(c => c.GetPerfomerById(testId))
                .ReturnsAsync(GetPerfomer(testId));
            mockService.Setup(c => c.GetVenueById(testId))
                .ReturnsAsync(GetVenue(testId));
            var controller = new ConcertsController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Get(testId, testId);

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<ConcertRequest>>(requestResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task GetAllConcertReturnsNoContent()
        {
            int testId = 1;
            List<Concert> list = null;
            var mockLogger = new Mock<ILogger<ConcertsController>>();
            var mockService = new Mock<IGatewayService>();
            mockService.Setup(c => c.GetConcerts(testId, testId))
                .ReturnsAsync(list);
           
            var controller = new ConcertsController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Get(testId, testId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostGatewayReturnsBadRequestResult()
        {
            int testId = 1;
            bool success = true;
            var mockLogger = new Mock<ILogger<ConcertsController>>();
            var mockService = new Mock<IGatewayService>();
            
            mockService.Setup(c => c.PostConcert(It.IsAny<Concert>()))
                .ReturnsAsync((true,GetTestConcerts()[0]));
            mockService.Setup(c => c.PutSchedule(It.IsAny<Schedule>()))
                .ReturnsAsync(false);
            var controller = new ConcertsController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Post(concert:null);

            // Assert
            var requestResult = Assert.IsType<BadRequestResult>(result);
            
        }

        [Fact]
        public async Task PostGatewayReturnsCreatedAtActionResult()
        {
            int testId = 1;
            bool success = true;
            var mockLogger = new Mock<ILogger<ConcertsController>>();
            var mockService = new Mock<IGatewayService>();
            Schedule schedule = new Schedule();
            mockService.Setup(c => c.PostSchedule(It.IsAny<Schedule>()))
                .ReturnsAsync(true);
            Concert concert = GetTestConcerts()[0];
            mockService.Setup(c => c.PostConcert(It.IsAny<Concert>()))
                .ReturnsAsync((true, concert));
            var controller = new ConcertsController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Post(concert);

            // Assert
            var requestResult = Assert.IsType<CreatedAtActionResult>(result);

        }

        [Fact]
        public async Task PutGatewayReturnsBadRequestResult()
        {
            int testId = 1;
            bool success = true;
            var mockLogger = new Mock<ILogger<ConcertsController>>();
            var mockService = new Mock<IGatewayService>();
            mockService.Setup(c => c.PutConcert(testId, It.IsAny<Concert>()))
                .ReturnsAsync(true);
            mockService.Setup(c => c.PutSchedule(It.IsAny<Schedule>()))
                .ReturnsAsync(false);
            var controller = new ConcertsController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Put(testId,GetTestConcerts()[0]);

            // Assert
            var requestResult = Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task PutGatewayReturnsNoContentResult()
        {
            int testId = 1;
            bool success = true;
            var mockLogger = new Mock<ILogger<ConcertsController>>();
            var mockService = new Mock<IGatewayService>();
            Schedule schedule = new Schedule();
            mockService.Setup(c => c.PutSchedule(It.IsAny<Schedule>()))
                .ReturnsAsync(true);
            Concert concert = GetTestConcerts()[0];
            mockService.Setup(c => c.PutConcert(testId, It.IsAny<Concert>()))
                .ReturnsAsync(true);
            var controller = new ConcertsController(mockService.Object, mockLogger.Object);

            // Act
            var result = await controller.Put(testId,concert);

            // Assert
            var requestResult = Assert.IsType<NoContentResult>(result);

        }

        private Venue GetVenue(int id)
        {
            return GetTestVenues()[id];
        }
        private Perfomer GetPerfomer(int id)
        {
            return GetTestPerfomers()[id];
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
        private List<Perfomer> GetTestPerfomers()
        {
            var perfomers = new List<Perfomer>();
            perfomers.Add(new Perfomer()
            {
                Id = 1,
                Name = "Group One"
            });
            perfomers.Add(new Perfomer()
            {
                Id = 2,
                Name = "Group Two"
            });
            perfomers.Add(new Perfomer()
            {
                Id = 3,
                Name = "Group Three"
            });
            return perfomers;
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
