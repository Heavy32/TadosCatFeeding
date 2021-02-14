using DataBaseManagement.CatManagement;
using DataBaseManagement.CatSharingManagement;
using DataBaseManagement.UserManagement;
using Moq;
using NUnit.Framework;
using Services.CatSharingManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CatSharingManagement.Tests
{
    public class CatSharingServiceTests
    {
        [Test]
        public void Share_Success_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.Create(catSharingCreateInDb)).Returns(1);
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(cat);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(user);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = service.Share(catSharingCreate, 1);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.PetIsShared);

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public void Share_UserNotFound_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.Create(catSharingCreateInDb)).Returns(1);
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(false);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(cat);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns((UserInDbModel)null);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = service.Share(catSharingCreate, 1);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "User to share cannot be found");

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void Share_CatNotFound_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.Create(catSharingCreateInDb)).Returns(1);
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns((CatInDbModel)null);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(user);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = service.Share(catSharingCreate, 1);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.ItemNotFound, "Cat is not found");

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void Share_UserCannotShare_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.Create(catSharingCreateInDb)).Returns(1);
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(cat);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(user);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            ServiceResult<CatSharingModel> actualResult = service.Share(catSharingCreate, 99);
            var expectedResult = new ServiceResult<CatSharingModel>(ServiceResultStatus.CantShareWithUser, "This user cannot share the pet");

            //Assert 
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        [Test]
        public void IsPetShared_Success_Test()
        {
            //Arrange
            var user = new UserInDbModel(1, "", "", 1, "", "");
            var cat = new CatInDbModel(1, "", 1);
            var catSharingCreateInDb = new CatSharingCreateInDbModel(1, 1);
            var catSharingCreate = new CatSharingCreateModel(1, 1);

            var mockCatSharingDatabase = new Mock<ICatSharingRepository>();
            mockCatSharingDatabase.Setup(repository => repository.Create(catSharingCreateInDb)).Returns(1);
            mockCatSharingDatabase.Setup(repository => repository.IsPetSharedWithUser(1, 1)).Returns(true);

            var mockCatDatabase = new Mock<ICatRepository>();
            mockCatDatabase.Setup(repository => repository.Get(1)).Returns(cat);

            var mockUserDatabase = new Mock<IUserRepository>();
            mockUserDatabase.Setup(repository => repository.Get(1)).Returns(user);

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<CatSharingCreateInDbModel, CatSharingCreateModel>(catSharingCreate)).Returns(catSharingCreateInDb);

            var service = new CatSharingService(
                mockCatSharingDatabase.Object,
                mockCatDatabase.Object,
                mockUserDatabase.Object,
                mockMapper.Object);

            //Action
            bool actualResult = service.IsPetSharedWithUser(1,1);
            var expectedResult = true;

            //Assert 
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}