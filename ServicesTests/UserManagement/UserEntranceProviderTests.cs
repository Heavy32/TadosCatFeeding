using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using Services;
using Services.UserManagement;
using Services.UserManagement.PasswordProtection;
using System.Threading.Tasks;

namespace ServicesTests.UserManagement
{
    public class UserEntranceProviderTests
    {
        [Test]
        public async Task LogIn_Success()
        {
            //Arrange
            var userInDb = new UserInDbModel(1, "max@mail.com", "max", 1, "salt", "password");

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repository => repository.GetUserByLoginAsync("max@mail.com")).Returns(Task.FromResult(userInDb));

            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            mockPasswordProtection.Setup(protection => protection.VerifyPassword(It.IsAny<HashedPasswordWithSalt>(), "password")).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<UserClaimsModel, UserInDbModel>(userInDb)).Returns(new UserClaimsModel(1, "max@mail.ru", "User"));
        
            var userEntranceProvider = new UserEntranceProvider(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<TokenJwt> actualResult = await userEntranceProvider.LogIn("max@mail.com", "password");
            var expectedResult = new ServiceResult<TokenJwt>(ServiceResultStatus.ItemRecieved, "some token");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.IsNotNull(actualResult.ReturnedObject);
        }

        [Test]
        public async Task LogIn_Failed_IncorrectLoginPassword()
        {
            //Arrange
            var userInDb = new UserInDbModel(1, "max@mail.com", "max", 1, "salt", "password");

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repository => repository.GetUserByLoginAsync("max@mail.com")).Returns(Task.FromResult((UserInDbModel)null));

            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            mockPasswordProtection.Setup(protection => protection.VerifyPassword(It.IsAny<HashedPasswordWithSalt>(), "password")).Returns(true);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<UserClaimsModel, UserInDbModel>(userInDb)).Returns(new UserClaimsModel(1, "max@mail.ru", "User"));

            var userEntranceProvider = new UserEntranceProvider(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<TokenJwt> actualResult = await userEntranceProvider.LogIn("max@mail.com", "password");
            var expectedResult = new ServiceResult<TokenJwt>(ServiceResultStatus.IncorrectLoginPassword);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }
    }
}
