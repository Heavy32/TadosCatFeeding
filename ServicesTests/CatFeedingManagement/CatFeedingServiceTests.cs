using DataBaseManagement.CatFeedingManagement;
using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using Services.CatFeedingManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CatFeedingManagement.Tests
{
    public class CatFeedingServiceTests
    {
        [Test]
        public void Feed_Success_Test()
        {
            //Arrange
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(catInDbModel);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(userInDbModel);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);
            
            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.Create(catFeedingCreateInDbModel)).Returns(1);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = service.Feed(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.NoContent);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public void Feed_PetIsNotShared_FailedTest()
        {
            //Arrange
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(false);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(catInDbModel);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(userInDbModel);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.Create(catFeedingCreateInDbModel)).Returns(1);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = service.Feed(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.ActionNotAllowed, "You cannot feed this cat"); ;

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void Feed_UserNotFound_Test()
        {
            //Arrange
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(catInDbModel);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns((UserInDbModel)null);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.Create(catFeedingCreateInDbModel)).Returns(1);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = service.Feed(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "User is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void Feed_CatNotFound_Test()
        {
            //Arrange
            DateTime now = DateTime.UtcNow;
            var catFeedingCreateModel = new CatFeedingCreateModel(1, 1, now);
            var catInDbModel = new CatInDbModel(1, "", 1);
            var userInDbModel = new UserInDbModel(1, "", "", 1, "", "");
            var catFeedingCreateInDbModel = new CatFeedingCreateInDbModel(1, 1, now);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns((CatInDbModel)null);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(userInDbModel);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.Create(catFeedingCreateInDbModel)).Returns(1);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatFeedingModel> actualResult = service.Feed(catFeedingCreateModel);
            var expectedResult = new ServiceResult<CatFeedingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void GetFeedingForPeriod_Success_Test()
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
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(catInDbModel);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(userInDbModel);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.GetFeedingForPeriod(1, 1, now.AddDays(-1), now)).Returns(results);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<DateTime>> actualResult = service.GetFeedingForPeriod(1, 1, now.AddDays(-1), now);
            var expectedResult = new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemRecieved, results);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Count, actualResult.ReturnedObject.Count);
        }

        [Test]
        public void GetFeedingForPeriod_UserNotFound_Test()
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
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(catInDbModel);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns((UserInDbModel)null);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.GetFeedingForPeriod(1, 1, now.AddDays(-1), now)).Returns(results);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<DateTime>> actualResult = service.GetFeedingForPeriod(1, 1, now.AddDays(-1), now);
            var expectedResult = new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "User is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void GetFeedingForPeriod_CatNotFound_Test()
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
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns((CatInDbModel)null);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(userInDbModel);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatFeedingCreateInDbModel, CatFeedingCreateModel>(catFeedingCreateModel)).Returns(catFeedingCreateInDbModel);

            var mockCatFeedingRepository = new Mock<ICatFeedingRepository>();
            mockCatFeedingRepository.Setup(repository => repository.GetFeedingForPeriod(1, 1, now.AddDays(-1), now)).Returns(results);

            var service = new CatFeedingService(
                mockCatFeedingRepository.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockCatSharingDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<DateTime>> actualResult = service.GetFeedingForPeriod(1, 1, now.AddDays(-1), now);
            var expectedResult = new ServiceResult<List<DateTime>>(ServiceResultStatus.ItemNotFound, "Cat is not found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }
    }
}