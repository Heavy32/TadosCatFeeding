using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using Services.CatSharingManagement;
using System.Collections.Generic;
using System.Security.Claims;

namespace Services.CatManagement.Tests
{
    public class CatCRUDServiceTests
    {
        [Test]
        public void Create_Success_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var catCreateServiceModel = new CatCreateServiceModel("", 1);
            var catServiceModel = new CatServiceModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Create(It.IsAny<CatCreateInDbModel>())).Returns(1);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(user);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatCreateInDbModel, CatCreateServiceModel>(catCreateServiceModel)).Returns(It.IsAny<CatCreateInDbModel>());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
            };

            var service = new CatCRUDService(
                mockCatDatabase.Object,
                mockCatSharingDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatServiceModel> actualResult = service.Create(catCreateServiceModel, claims);
            var expectedResult = new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemCreated, catServiceModel);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Id, actualResult.ReturnedObject.Id);
        }

        [Test]
        public void Create_Forbidden_Test()
        {
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var catCreateServiceModel = new CatCreateServiceModel("", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Create(It.IsAny<CatCreateInDbModel>())).Returns(1);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(user);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatCreateInDbModel, CatCreateServiceModel>(catCreateServiceModel)).Returns(It.IsAny<CatCreateInDbModel>());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "111"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
            };

            var service = new CatCRUDService(
                mockCatDatabase.Object,
                mockCatSharingDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            ServiceResult<CatServiceModel> actualResult = service.Create(catCreateServiceModel, claims);
            var expectedResult = new ServiceResult<CatServiceModel>(ServiceResultStatus.ActionNotAllowed);

            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public void Create_UserNotFound_Test()
        {
            //Arrange
            var catCreateServiceModel = new CatCreateServiceModel("", 1);
            var catServiceModel = new CatServiceModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Create(It.IsAny<CatCreateInDbModel>())).Returns(1);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns((UserInDbModel)null);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatCreateInDbModel, CatCreateServiceModel>(catCreateServiceModel)).Returns(It.IsAny<CatCreateInDbModel>());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
            };

            var service = new CatCRUDService(
                mockCatDatabase.Object,
                mockCatSharingDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatServiceModel> actualResult = service.Create(catCreateServiceModel, claims);
            var expectedResult = new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemNotFound, "User is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void Get_Success_Test()
        {
            //Arrange
            var catGetServiceModel = new CatGetServiceModel(1, "", 1);
            var catInDbModel = new CatInDbModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(catInDbModel);

            var mockUserDatabase = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatGetServiceModel, CatInDbModel>(catInDbModel)).Returns(catGetServiceModel);

            var service = new CatCRUDService(
                mockCatDatabase.Object,
                mockCatSharingDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatGetServiceModel> actualResult = service.Get(1);
            var expectedResult = new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemRecieved, catGetServiceModel);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Id, actualResult.ReturnedObject.Id);
        }

        [Test]
        public void Get_CatNotFound_Test()
        {
            //Arrange
            var catGetServiceModel = new CatGetServiceModel(1, "", 1);
            var catInDbModel = new CatInDbModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns((CatInDbModel)null);

            var mockUserDatabase = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatGetServiceModel, CatInDbModel>(catInDbModel)).Returns(catGetServiceModel);

            var service = new CatCRUDService(
                mockCatDatabase.Object,
                mockCatSharingDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatGetServiceModel> actualResult = service.Get(1);
            var expectedResult = new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemNotFound);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }
    }
}