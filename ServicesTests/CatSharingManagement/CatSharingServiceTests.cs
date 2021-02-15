using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Services.CatSharingManagement.Tests
{
    public class CatSharingServiceTests
    {
        [Test]
        public async Task Share_Success_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.CreateAsync(catSharingCreateInDb)).Returns(Task.FromResult(1));
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(cat));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(user));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = await service.ShareAsync(catSharingCreate, 1);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.PetIsShared);

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public async Task Share_UserNotFound_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.CreateAsync(catSharingCreateInDb)).Returns(Task.FromResult(1));
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(false));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(cat));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((UserInDbModel)null));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

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
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.CreateAsync(catSharingCreateInDb)).Returns(Task.FromResult(1));
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((CatInDbModel)null));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(user));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

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
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.CreateAsync(catSharingCreateInDb)).Returns(Task.FromResult(1));
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(cat));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(user));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = await service.ShareAsync(catSharingCreate, 99);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.CantShareWithUser, "This user cannot share the pet");

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task IsPetShared_Success_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.CreateAsync(catSharingCreateInDb)).Returns(Task.FromResult(1));
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(cat));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(user));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            bool actualResult = await service.IsPetSharedWithUser(1,1);
            var expectedResult = true;

            //Assert 
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}