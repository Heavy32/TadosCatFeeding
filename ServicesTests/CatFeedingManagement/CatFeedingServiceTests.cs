using DataBaseManagement.CatFeedingManagement;
using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.CatFeedingManagement.Tests
{
    public class CatFeedingServiceTests
    {
        [Test]
        public async Task Feed_Success_Test()
        {
            //Arrange
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catInDbModel));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userInDbModel));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);
            
            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.CreateAsync(catFeedingCreateInDbModel)).Returns(Task.FromResult(1));

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
        public async Task Feed_PetIsNotShared_FailedTest()
        {
            //Arrange
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(false));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catInDbModel));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userInDbModel));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.CreateAsync(catFeedingCreateInDbModel)).Returns(Task.FromResult(1));

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
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catInDbModel));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((UserInDbModel)null));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.CreateAsync(catFeedingCreateInDbModel)).Returns(Task.FromResult(1));

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
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((CatInDbModel)null));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userInDbModel));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.CreateAsync(catFeedingCreateInDbModel)).Returns(Task.FromResult(1));

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
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);
            var results = new List<DateTime>
            {
                now,
                now
            };

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catInDbModel));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userInDbModel));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.GetFeedingForPeriodAsync(1, 1, now.AddDays(-1), now)).Returns(Task.FromResult(results));

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<DateTime>> actualResult = await service.GetFeedingForPeriodAsync(1, 1, now.AddDays(-1), now);
            var expectedResult = new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemRecieved, results);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Count, actualResult.ReturnedObject.Count);
        }

        [Test]
        public async Task GetFeedingForPeriod_UserNotFound_Test()
        {
            //Arrange
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);
            var results = new List<DateTime>
            {
                now,
                now
            };

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(catInDbModel));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((UserInDbModel)null));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.GetFeedingForPeriodAsync(1, 1, now.AddDays(-1), now)).Returns(Task.FromResult(results));

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
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);
            var results = new List<DateTime>
            {
                now,
                now
            };

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUserAsync(1, 1)).Returns(Task.FromResult(true));

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((CatInDbModel)null));

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(userInDbModel));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.GetFeedingForPeriodAsync(1, 1, now.AddDays(-1), now)).Returns(Task.FromResult(results));

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