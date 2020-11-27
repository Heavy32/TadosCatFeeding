using NUnit.Framework;
using TadosCatFeeding.PetFeedingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using TadosCatFeeding.UserManagement;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.CatFeedingManagement;
using Microsoft.AspNetCore.Mvc;

namespace TadosCatFeeding.PetFeedingManagement.Tests
{
    public class CatFeedingControllerTests
    {
        [Test]
        public void FeedCat_Return_404_UserNotFound()
        {
            //Arrange
            int userId = 1;
            int catId = 1;
            DateTime time = DateTime.UtcNow;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns((UserModel)null);
            mockContext.Setup(context => context.CatRepository.Get(catId)).Returns(new CatModel());

            CatFeedingController controller = new CatFeedingController(mockContext.Object);

            //Act
            var response = controller.Create(userId, catId, time) as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("User cannot be found", response.Value);
            Assert.AreEqual(404, response.Value);
        }

        [Test]
        public void FeedCat_Return_404_CatNotFound()
        {
            //Arrange
            int userId = 1;
            int catId = 1;
            DateTime time = DateTime.UtcNow;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.CatRepository.Get(catId)).Returns((CatModel)null);
            
            CatFeedingController controller = new CatFeedingController(mockContext.Object);

            //Act
            var response = controller.Create(userId, catId, time) as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("Cat cannot be found", response.Value);
            Assert.AreEqual(404, response.StatusCode);
        }
    }
}