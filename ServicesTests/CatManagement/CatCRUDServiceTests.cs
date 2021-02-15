using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using Services.CatSharingManagement;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.CatManagement.Tests
{
    public class CatCRUDServiceTests
    {
        [Test]
        public async Task Create_Success_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var catCreateServiceModel = new CatCreateServiceModel("", 1);
            var catServiceModel = new CatServiceModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.CreateAsync(It.IsAny<CatCreateInDbModel>())).Returns(Task.FromResult(1));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(user));

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
            ServiceResult<CatServiceModel> actualResult = await service.CreateAsync(catCreateServiceModel, claims);
            var expectedResult = new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemCreated, catServiceModel);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Id, actualResult.ReturnedObject.Id);
        }

        [Test]
        public async Task Create_Forbidden_Test()
        {
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var catCreateServiceModel = new CatCreateServiceModel("", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.CreateAsync(It.IsAny<CatCreateInDbModel>())).Returns(Task.FromResult(1));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(user));

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

            ServiceResult<CatServiceModel> actualResult = await service.CreateAsync(catCreateServiceModel, claims);
            var expectedResult = new ServiceResult<CatServiceModel>(ServiceResultStatus.ActionNotAllowed);

            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public async Task Create_UserNotFound_Test()
        {
            //Arrange
            var catCreateServiceModel = new CatCreateServiceModel("", 1);
            var catServiceModel = new CatServiceModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.CreateAsync(It.IsAny<CatCreateInDbModel>())).Returns(Task.FromResult(1));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((UserInDbModel)null));

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
            ServiceResult<CatServiceModel> actualResult = await service.CreateAsync(catCreateServiceModel, claims);
            var expectedResult = new ServiceResult<CatServiceModel>(ServiceResultStatus.ItemNotFound, "User is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task Get_Success_Test()
        {
            //Arrange
            var catGetServiceModel = new CatGetServiceModel(1, "", 1);
            var catInDbModel = new CatInDbModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catInDbModel));

            var mockUserDatabase = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatGetServiceModel, CatInDbModel>(catInDbModel)).Returns(catGetServiceModel);

            var service = new CatCRUDService(
                mockCatDatabase.Object,
                mockCatSharingDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatGetServiceModel> actualResult = await service.GetAsync(1);
            var expectedResult = new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemRecieved, catGetServiceModel);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Id, actualResult.ReturnedObject.Id);
        }

        [Test]
        public async Task Get_CatNotFound_Test()
        {
            //Arrange
            var catGetServiceModel = new CatGetServiceModel(1, "", 1);
            var catInDbModel = new CatInDbModel(1, "", 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((CatInDbModel)null));

            var mockUserDatabase = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatGetServiceModel, CatInDbModel>(catInDbModel)).Returns(catGetServiceModel);

            var service = new CatCRUDService(
                mockCatDatabase.Object,
                mockCatSharingDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatGetServiceModel> actualResult = await service.GetAsync(1);
            var expectedResult = new ServiceResult<CatGetServiceModel>(ServiceResultStatus.ItemNotFound);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }
    }
}