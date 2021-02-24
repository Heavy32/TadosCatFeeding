using DataBaseRepositories.CatRepository;
using DataBaseRepositories.CatSharingRepository;
using DataBaseRepositories.UserManagement;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace BusinessLogic.CatSharingManagement.Tests
{
    public class CatSharingServiceTests
    {
        private readonly UserInDbModel user = new UserInDbModel(1, "", "", 1, "", "");
        private readonly CatInDbModel cat = new CatInDbModel(1, "", 1);
        private readonly CatSharingCreateInDbModel catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
        private readonly CatSharingCreateModel catSharingCreate = new CatSharingCreateModel(1, 1);

        private readonly Mock<ICatSharingRepository> mockCatSharingDatabase = new Mock<ICatSharingRepository>();
        private readonly Mock<ICatRepository> mockCatDatabase = new Mock<ICatRepository>();
        private readonly Mock<IUserRepository> mockUserDatabase = new Mock<IUserRepository>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();

        private void SetUpMockRepositories(
            int catSharingCreateResult,
            bool isCatShared,
            CatInDbModel catGetResult,
            UserInDbModel userGetResult,
            CatSharingCreateInDbModel mapperResult)
        {
            mockCatSharingDatabase.Setup(repository => repository.CreateAsync(catSharingCreateInDb)).Returns(Task.FromResult(catSharingCreateResult));
            mockCatSharingDatabase.Setup(repository => repository.IsCatSharedWithUserAsync(1, 1)).Returns(Task.FromResult(isCatShared));
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catGetResult));
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userGetResult));
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(mapperResult);

        }

        [Test]
        public async Task Share_Success_Test()
        {
            //Arrange
            SetUpMockRepositories(1, true, cat, user, catSharingCreateInDb);
            
            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = await service.ShareAsync(catSharingCreate, 1);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.CatIsShared);

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public async Task Share_UserNotFound_Test()
        {
            //Arrange
            SetUpMockRepositories(1, false, cat, null, catSharingCreateInDb);
            
            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = await service.ShareAsync(catSharingCreate, 1);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "User to share cannot be found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task Share_CatNotFound_Test()
        {
            //Arrange
            SetUpMockRepositories(1, true, null, user, catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = await service.ShareAsync(catSharingCreate, 1);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task Share_UserCannotShare_Test()
        {
            //Arrange
            SetUpMockRepositories(1, true, cat, user, catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = await service.ShareAsync(catSharingCreate, 99);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.CantShareWithUser, "This user cannot share the cat");

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task IsCatShared_Success_Test()
        {
            //Arrange
            SetUpMockRepositories(1, true, cat, user, catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            bool actualResult = await service.IsCatSharedWithUser(1,1);
            var expectedResult = true;

            //Assert 
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}