using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using Services.UserManagement.PasswordProtection;
using System.Collections.Generic;
using System.Security.Claims;

namespace Services.UserManagement.Tests
{
    public class UserCRUDServiceTests
    {
        [Test]
        public void Get_NotFound_Test()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(rep => rep.Get(1)).Returns((UserInDbModel)null);
            
            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            var mockMapper = new Mock<IMapper>();

            UserCRUDService service = new UserCRUDService(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<UserGetModel> actualResult = service.Get(1);
            var expectedResult = new ServiceResult<UserGetModel>(ServiceResultStatus.ItemNotFound, "User cannot be found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void Get_Success_Test()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var userInDb = new UserInDbModel(1, "max@mail.com", "max", 1, "salt", "password");
            mockUserRepository.Setup(rep => rep.Get(1)).Returns(userInDb);

            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            mockPasswordProtection.Setup(protection => protection.ProtectPassword("123")).Returns(new HashedPasswordWithSalt());
            
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<UserGetModel, UserInDbModel>(userInDb)).Returns(new UserGetModel { Id = 1, Login = "max@mail.com", NickName = "max" });

            UserCRUDService service = new UserCRUDService(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<UserGetModel> actualResult = service.Get(1);
            var expectedResult = new ServiceResult<UserGetModel>(ServiceResultStatus.ItemRecieved, new UserGetModel { Id = 1, Login = "max@mail.com", NickName = "max" });

            //Assert
            Assert.AreEqual(expectedResult.ReturnedObject.Id, actualResult.ReturnedObject.Id);
            Assert.AreEqual(expectedResult.ReturnedObject.Login, actualResult.ReturnedObject.Login);
            Assert.AreEqual(expectedResult.ReturnedObject.NickName, actualResult.ReturnedObject.NickName);
        }

        [Test]
        public void Update_Forbidden_Test()
        {
            //Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            var mockMapper = new Mock<IMapper>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "10"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
            };

            UserCRUDService service = new UserCRUDService(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<UserServiceModel> actualResult = service.Update(5, new UserUpdateModel { Role = Roles.User }, claims);
            var expectedResult = new ServiceResult<UserServiceModel>(ServiceResultStatus.ActionNotAllowed, "You cannot update this user");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void Delete_Success_Test()
        {
            //Arrange
            var userInDb = new UserInDbModel(1, "max@mail.com", "max", 1, "salt", "password");

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repository => repository.Delete(10)).Verifiable();
            mockUserRepository.Setup(repository => repository.Get(10)).Returns(userInDb);
            
            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            var mockMapper = new Mock<IMapper>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "10"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
            };

            UserCRUDService service = new UserCRUDService(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<UserServiceModel> actualResult = service.Delete(10, claims);
            var expectedResult = new ServiceResult<UserServiceModel>(ServiceResultStatus.ItemDeleted);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public void Delete_Forbidden_Test()
        {
            //Arrange
            var userInDb = new UserInDbModel(1, "max@mail.com", "max", 1, "salt", "password");

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repository => repository.Delete(10)).Verifiable();
            mockUserRepository.Setup(repository => repository.Get(10)).Returns(userInDb);

            var mockPasswordProtection = new Mock<IPasswordProtector<HashedPasswordWithSalt>>();
            var mockMapper = new Mock<IMapper>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "5"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
            };

            UserCRUDService service = new UserCRUDService(
                mockUserRepository.Object,
                mockPasswordProtection.Object,
                mockMapper.Object);

            //Action
            ServiceResult<UserServiceModel> actualResult = service.Delete(10, claims);
            var expectedResult = new ServiceResult<UserServiceModel>(ServiceResultStatus.ActionNotAllowed, "You cannot delete this user");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }
    }
}