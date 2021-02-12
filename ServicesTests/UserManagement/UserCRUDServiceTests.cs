using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using Services.UserManagement.PasswordProtection;

namespace Services.UserManagement.Tests
{
    [TestFixture()]
    public class UserCRUDServiceTests
    {
        [Test()]
        public void UserCRUDServiceTest()
        {
            Assert.Fail();
        }

        [TestCase(1)]
        public void Get_NotFound_Test(int id)
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(rep => rep.Get(id)).Returns((UserInDbModel)null);
            
            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            var mockMapper = new Mock<IMapper>();

            UserCRUDService service = new UserCRUDService(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<UserGetModel> actualResult = service.Get(id);
            ServiceResult<UserGetModel> expectedResult = new ServiceResult<UserGetModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test()]
        public void UpdateTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteTest()
        {
            Assert.Fail();
        }
    }
}