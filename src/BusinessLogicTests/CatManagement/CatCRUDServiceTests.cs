using DataBaseRepositories.CatRepository;
using DataBaseRepositories.CatSharingRepository;
using DataBaseRepositories.UserManagement;
using Moq;
using NUnit.Framework;
using BusinessLogic.CatSharingManagement;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLogic.CatManagement.Tests
{
    public class CatCRUDServiceTests
    {
        private readonly UserInDbModel user = new UserInDbModel(1, "", "", 1, "", "");
        private readonly CatCreateServiceModel catCreateServiceModel = new CatCreateServiceModel("", 1);
        private readonly CatServiceModel catServiceModel = new CatServiceModel(1, "", 1);
        private readonly CatInDbModel catInDbModel = new CatInDbModel(1, "", 1);
        private readonly CatGetServiceModel catGetServiceModel = new CatGetServiceModel(1, "", 1);
        private readonly List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, "1")
        };

        private readonly Mock<ICatRepository> mockCatDatabase = new Mock<ICatRepository>();
        private readonly Mock<IUserRepository> mockUserDatabase = new Mock<IUserRepository>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ICatSharingRepository> mockCatSharingDatabase = new Mock<ICatSharingRepository>();
        
        private void SetUpMockRepositories(
            int CatDatabaseCreateResult,
            UserInDbModel userRepositoryGetResult,
            CatCreateInDbModel mapperResult)
        {
            mockCatDatabase.Setup(repository => repository.CreateAsync(It.IsAny<CatCreateInDbModel>())).Returns(Task.FromResult(CatDatabaseCreateResult));
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userRepositoryGetResult));
            mockMapper.Setup(mapper => mapper.Map<CatCreateInDbModel, CatCreateServiceModel>(catCreateServiceModel)).Returns(mapperResult);
        }

        [Test]
        public async Task Create_Success_Test()
        {
            //Arrange
            SetUpMockRepositories(1, user, It.IsAny<CatCreateInDbModel>());

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
            //Arrange
            SetUpMockRepositories(1, user, It.IsAny<CatCreateInDbModel>());

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
            SetUpMockRepositories(1, null, It.IsAny<CatCreateInDbModel>());

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
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catInDbModel));
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
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((CatInDbModel)null));
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