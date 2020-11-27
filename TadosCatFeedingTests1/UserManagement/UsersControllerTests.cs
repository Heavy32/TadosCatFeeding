using NUnit.Framework;
using TadosCatFeeding.Controllers;
using Moq;
using TadosCatFeeding.UserManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TadosCatFeeding;

namespace TadosCatFeedingTests.UserManagement
{
    class UsersControllerTests
    {
        [Test]
        public void Get_Return_200_With_User()
        {
            //Arrange
            int userId = 1;
            UserModel user = new UserModel { Login = "a@mail.com", Password = "123456Aa", Nickname = "A", Role = "User" };

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns(user);

            var controller = new UserController(mockContext.Object);
            //Act
            var response = controller.Get(userId) as OkObjectResult;
            var userReturned = response.Value as UserModel;

            //Assert
            Assert.IsNotNull(userReturned);
            Assert.AreEqual(JsonConvert.SerializeObject(user), JsonConvert.SerializeObject(userReturned));
        }

        [Test]
        public void Get_Return_404_User_Not_Found()
        {
            //Arrange
            int userId = 999;
            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns((UserModel)null);

            var controller = new UserController(mockContext.Object);
            //Act
            var response = controller.Get(userId) as NotFoundResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 404);
        }

        [Test]
        public void Delete_Return_204()
        {
            //Arrange
            int userId = 1;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns(new UserModel());
            mockContext.Setup(context => context.UserRepository.Delete(userId));

            UserController controller = new UserController(mockContext.Object);

            //Act
            var response = controller.Delete(userId) as NoContentResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 204);
        }

        [Test]
        public void Delete_Return_404()
        {
            //Arrange
            int userId = 1;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns((UserModel)null);

            UserController controller = new UserController(mockContext.Object);
            //Act
            var response = controller.Delete(userId) as NotFoundResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 404);
        }

        [Test]
        public void Patch_Return_204()
        {
            //Arrange
            int userId = 1;
            UserModel userInfo = new UserModel { Login = "a@mail.com", Password = "123456Aa", Nickname = "A", Role = "User" };

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Update(userId, userInfo));
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns(new UserModel());

            var mockConfiguration = new Mock<IConfiguration>();
            UserController controller = new UserController(mockContext.Object);

            //Act
            var response = controller.Update(userId, userInfo) as NoContentResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 204);
        }

        [Test]
        public void Patch_Return_404()
        {
            //Arrange
            int userId = 1;
            UserModel userInfo = new UserModel { Login = "a@mail.com", Password = "123456Aa", Nickname = "A", Role = "User" };

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns((UserModel)null);

            UserController controller = new UserController(mockContext.Object);

            //Act
            var response = controller.Update(userId, userInfo) as NotFoundResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 404);
        }

        [Test]
        public void Post()
        {

        }
    }
}
