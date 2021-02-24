using DataBaseRepositories.CatFeedingRepository;
using DataBaseRepositories.CatRepository;
using DataBaseRepositories.CatSharingRepository;
using DataBaseRepositories.UserManagement;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.CatFeedingManagement.Tests
{
    public class CatFeedingServiceTests
    {
        private static readonly DateTime now = DateTime.UtcNow;
        private readonly CatFeedingCreateModel catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
        private readonly CatInDbModel catInDbModel = new CatInDbModel(1, "", 1);
        private readonly UserInDbModel userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
        private readonly CatFeedingCreateInDbModel catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);
        private readonly List<CatFeedingInDbModel> results = new List<CatFeedingInDbModel>
        {
            new CatFeedingInDbModel(1, 1 ,1 ,now),
            new CatFeedingInDbModel(1, 1 ,1 ,now),
        };

        private readonly Mock<ICatSharingRepository> mockCatSharingDatabase = new Mock<ICatSharingRepository>();
        private readonly Mock<ICatRepository> mockCatDatabase = new Mock<ICatRepository>();
        private readonly Mock<IUserRepository> mockUserDatabase = new Mock<IUserRepository>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        private readonly Mock<ICatFeedingRepository> mockCatFeedingRepository = new Mock<ICatFeedingRepository>();

        private void SetUpMockRepositories(
            bool catSharingRepositoryResult,
            CatInDbModel catRepositoryResult,
            UserInDbModel userRepositoryResult,
            int catFeedingRepositoryResult,
            CatFeedingCreateInDbModel mapperResult
            )
        {
            mockCatSharingDatabase.Setup(repository => repository.IsCatSharedWithUserAsync(1, 1)).Returns(Task.FromResult(catSharingRepositoryResult));
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catRepositoryResult));
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userRepositoryResult));
            mockCatFeedingRepository.Setup(repository => repository.CreateAsync(catFeedingCreateInDbModel)).Returns(Task.FromResult(catFeedingRepositoryResult));
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(mapperResult);
        }

        [Test]
        public async Task Feed_Success_Test()
        {
            //Arrange
            SetUpMockRepositories(true, catInDbModel, userInDbModel, 1, catFeedingCreateInDbModel);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = await service.FeedAsync(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.NoContent);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public async Task Feed_CatIsNotShared_FailedTest()
        {
            //Arrange
            SetUpMockRepositories(false, catInDbModel, userInDbModel, 1, catFeedingCreateInDbModel);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = await service.FeedAsync(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.ActionNotAllowed, "You cannot feed this cat"); ;

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task Feed_UserNotFound_Test()
        {
            //Arrange
            SetUpMockRepositories(true, catInDbModel, null, 1, catFeedingCreateInDbModel);
            
            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = await service.FeedAsync(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "User is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task Feed_CatNotFound_Test()
        {
            //Arrange
            SetUpMockRepositories(true, null, userInDbModel, 1, catFeedingCreateInDbModel);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = await service.FeedAsync(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task GetFeedingForPeriod_Success_Test()
        {
            //Arrange
            SetUpMockRepositories(true, catInDbModel, userInDbModel, 1, catFeedingCreateInDbModel);
            mockCatFeedingRepository.Setup(repository => repository.GetFeedingsForPeriodAsync(1, 1, now.AddDays(-1), now)).Returns(Task.FromResult(results));

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<DateTime>> actualResult = await service.GetFeedingForPeriodAsync(1, 1, now.AddDays(-1), now);
            var expectedResult = new ServiceResult<List<CatFeedingInDbModel>>(ServiceResultStatus.ItemRecieved, results);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Count, actualResult.ReturnedObject.Count);
        }

        [Test]
        public async Task GetFeedingForPeriod_UserNotFound_Test()
        {
            //Arrange
            SetUpMockRepositories(true, catInDbModel, null, 1, catFeedingCreateInDbModel);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<DateTime>> actualResult = await service.GetFeedingForPeriodAsync(1, 1, now.AddDays(-1), now);
            var expectedResult = new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "User is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public async Task GetFeedingForPeriod_CatNotFound_Test()
        {
            //Arrange
            SetUpMockRepositories(true, null, userInDbModel, 1, catFeedingCreateInDbModel);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<DateTime>> actualResult = await service.GetFeedingForPeriodAsync(1, 1, now.AddDays(-1), now);
            var expectedResult = new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "Cat is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }
    }
}