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
        private readonly UserInDbModel userInDb = new UserInDbModel(1, "max@mail.com", "max", 1, "salt", "password");
        private readonly Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<IPasswordProtector<HashedPasswordWithSalt>> mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();

        [Test]
        public async Task LogIn_Success()
        {
            //Arrange
            mockUserRepository.Setup(repository => repository.GetUserByLoginAsync("max@mail.com")).Returns(Task.FromResult(userInDb));
            mockPasswordProtection.Setup(protection => protection.VerifyPassword(It.IsAny<HashedPasswordWithSalt>(), "password")).Returns(true);
            mockMapper.Setup(mapper => mapper.Map<UserClaimsModel, UserInDbModel>(userInDb)).Returns(new UserClaimsModel(1, "max@mail.ru", "User"));
        
            var userEntranceProvider = new UserEntranceProvider(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<TokenJwt> actualResult = await userEntranceProvider.LogInAsync("max@mail.com", "password");
            var expectedResult = new ServiceResult<TokenJwt>(ServiceResultStatus.ItemRecieved, "some token");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.IsNotNull(actualResult.ReturnedObject);
        }

        [Test]
        public async Task LogIn_Failed_IncorrectLoginPassword()
        {
            //Arrange
            mockUserRepository.Setup(repository => repository.GetUserByLoginAsync("max@mail.com")).Returns(Task.FromResult((UserInDbModel)null));
            mockPasswordProtection.Setup(protection => protection.VerifyPassword(It.IsAny<HashedPasswordWithSalt>(), "password")).Returns(true);
            mockMapper.Setup(mapper => mapper.Map<UserClaimsModel, UserInDbModel>(userInDb)).Returns(new UserClaimsModel(1, "max@mail.ru", "User"));

            var userEntranceProvider = new UserEntranceProvider(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<TokenJwt> actualResult = await userEntranceProvider.LogInAsync("max@mail.com", "password");
            var expectedResult = new ServiceResult<TokenJwt>(ServiceResultStatus.IncorrectLoginPassword);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }
    }
}
