using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using TadosCatFeeding.UserManagement;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.StatisticProvision;
using Microsoft.AspNetCore.Mvc;

namespace TadosCatFeeding.Controllers.Tests
{
    public class StatisticControllerTests
    {
        [Test]
        public void GetAll_Return_200()
        {
            //Arrange
            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.StatisticRepository.GetAll()).Returns(new List<StatisticModel> { new StatisticModel() });

            StatisticController controller = new StatisticController(mockContext.Object);

            //Act
            var response = controller.GetAll() as OkObjectResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public void GetAll_Return_204()
        {
            //Arrange
            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.StatisticRepository.GetAll()).Returns(new List<StatisticModel>());

            StatisticController controller = new StatisticController(mockContext.Object);

            //Act
            var response = controller.GetAll() as NoContentResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(204, response.StatusCode);
        }

        [Test]
        public void GetFeedingForPeriod_Return_404_UserNotFound()
        {
            //Arrange
            int userId = 1;
            int catId = 1;
            DateTime start = DateTime.UtcNow.AddDays(-1);
            DateTime finish = DateTime.UtcNow;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns((UserModel)null);
            
            StatisticController controller = new StatisticController(mockContext.Object);

            //Act 
            var response = controller.GetFeedingForPeriod(userId, catId, start, finish) as NotFoundObjectResult;
            
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(404, response.StatusCode);
            Assert.AreEqual("User cannot be found", response.Value);
        }

        [Test]
        public void GetFeedingForPeriod_Return_404_СatNotFound()
        {
            //Arrange
            int userId = 1;
            int catId = 1;
            DateTime start = DateTime.UtcNow.AddDays(-1);
            DateTime finish = DateTime.UtcNow;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns(new UserModel());
            mockContext.Setup(context => context.CatRepository.Get(catId)).Returns((CatModel)null);

            StatisticController controller = new StatisticController(mockContext.Object);

            //Act 
            var response = controller.GetFeedingForPeriod(userId, catId, start, finish) as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(404, response.StatusCode);
            Assert.AreEqual("Cat cannot be found", response.Value);
        }
    }
}