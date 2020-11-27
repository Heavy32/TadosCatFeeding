using TadosCatFeeding.PetSharingManagement;
using NUnit.Framework;
using Moq;
using TadosCatFeeding.UserManagement;
using TadosCatFeeding.CatManagement;
using Microsoft.AspNetCore.Mvc;

namespace TadosCatFeeding.PetSharingManagement.Tests
{
    [TestFixture()]
    public class CatSharingControllerTests
    {
        [Test]
        public void Share_Return_204()
        {
            //Arrange
            int userId = 1;
            int catId = 1;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(userId)).Returns(new UserModel());
            mockContext.Setup(context => context.CatRepository.Get(catId)).Returns(new CatModel());
            mockContext.Setup(context => context.CatSharingRepository.IsPetSharedWithUser(userId, catId)).Returns(true);

            CatSharingController controller = new CatSharingController(mockContext.Object);

            //Act
            var response = controller.Share(userId, catId) as NoContentResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(204, response.StatusCode);
        }

        [Test]
        public void Share_Return_404_UserNotFound()
        {
            //Arrange
            int userId = 1;
            int catId = 1;

            var mockContext = new Mock<IContext>();
            mockContext.Setup(context => context.UserRepository.Get(1)).Returns((UserModel)null);
            mockContext.Setup(context => context.CatRepository.Get(1)).Returns(new CatModel());

            CatSharingController controller = new CatSharingController(mockContext.Object);

            //Act
            var response = controller.Share(userId, catId) as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(404, response.StatusCode);
            Assert.AreEqual("User cannot be found", response.Value);
        }

        [Test]
        public void Share_Return_404_CatNotFound()
        {
            //Arrange
            int userId = 1;
            int catId = 1;

            var mockContext = new Mock<IContext>();

            mockContext.Setup(context => context.CatRepository.Get(1)).Returns((CatModel)null);
            CatSharingController controller = new CatSharingController(mockContext.Object);

            //Act
            var response = controller.Share(userId, catId) as NotFoundObjectResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(404, response.StatusCode);
            Assert.AreEqual("Cat cannot be found", response.Value);
        }

        [Test]
        public void Share_Return_204_IfPetIsShared()
        {
            //Arrange
            int userId = 1;
            int catId = 1;

            var mockContext = new Mock<IContext>();

            mockContext.Setup(context => context.UserRepository.Get(1)).Returns(new UserModel());
            mockContext.Setup(context => context.CatRepository.Get(1)).Returns(new CatModel());
            mockContext.Setup(context => context.CatSharingRepository.IsPetSharedWithUser(userId, catId)).Returns(false);

            CatSharingController controller = new CatSharingController(mockContext.Object);

            //Act
            var response = controller.Share(userId, catId) as NoContentResult;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(204, response.StatusCode);
        }
    }
}