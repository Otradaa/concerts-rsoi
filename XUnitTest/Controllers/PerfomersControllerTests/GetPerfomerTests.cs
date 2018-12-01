using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
//using Gateway.Models;
using PerfomersServer.Controllers;
using Moq;
using PerfomersServer.Models;
using Microsoft.EntityFrameworkCore;
using PerfomersServer.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTest
{
    public class GetPerfomerTests
    {
        [Fact]
        public async Task GetPerfomerReturnsPerfomerByIdTest()
        {
            // Arrange
            int testId = 1;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(c => c.GetPerfomer(testId))
                .ReturnsAsync(GetTestPerfomers().FirstOrDefault(p => p.Id == testId));
            var controller = new PerfomersController(mockRepo.Object);

            // Act
            var result = await controller.GetPerfomer(testId);

            // Assert
            var requestResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Perfomer>(requestResult.Value);
            Assert.Equal("Group One", model.Name);
            Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async Task GetPerfomerReturnsBadRequestByIdTest()
        {
            // Arrange
            int testId = 1;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(c => c.GetPerfomer(testId))
                .ReturnsAsync(GetTestPerfomers().FirstOrDefault(p => p.Id == testId));
            var controller = new PerfomersController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.GetPerfomer(testId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            //var model = Assert.IsType<Perfomer>(viewResult.Value);
            //Assert.Equal("Group One", model.Name);
            //Assert.Equal(testId, model.Id);
        }

        [Fact]
        public async Task GetPerfomerReturnsNotFoundByIdTest()
        {
            // Arrange
            int testId = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(c => c.GetPerfomer(testId))
                .ReturnsAsync(GetTestPerfomers().FirstOrDefault(p => p.Id == testId));
            var controller = new PerfomersController(mockRepo.Object);

            // Act
            var result = await controller.GetPerfomer(testId);

            // Assert
           Assert.IsType<NotFoundResult>(result);
            
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
    }
}
