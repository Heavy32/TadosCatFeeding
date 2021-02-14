using NUnit.Framework;
using Moq;
using DataBaseManagement.StatisticProvision;
using System.Collections.Generic;

namespace Services.StatisticProvision.Tests
{
    public class StatisticServiceTests
    {
        [Test]
        public void Execute_Success_Test()
        {
            //Arrange
            var mockStatisticRepository = new Mock<IStatisticRepository>();
            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            mockStatisticCalculation.Setup(calculation => calculation.Execute("expression")).Returns(new StatisticResult(new List<Dictionary<string, object>>()));
            var mockMapper = new Mock<IMapper>();

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            StatisticResult actualResult = service.Execute("expression");
            var expectedResult = new StatisticResult(new List<Dictionary<string, object>>());

            //Assert
            Assert.AreEqual(actualResult.Results.Count, expectedResult.Results.Count);
        }

        [Test]
        public void Get_Success()
        {
            //Arrange
            var statisticInDbModel = new StatisticInDbModel(1, "", "", "");
            var statisticModel = new StatisticModel(1, "", "", "");

            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.Get(1)).Returns(statisticInDbModel);

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            mockStatisticCalculation.Setup(calculation => calculation.Execute(statisticInDbModel.SqlExpression)).Returns(new StatisticResult(new List<Dictionary<string, object>>()));
            var mockMapper = new Mock<IMapper>();

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<StatisticResult> actualResult = service.GetStatisticResult(1);
            var expectedResult = new ServiceResult<StatisticResult>(ServiceResultStatus.ItemRecieved, new StatisticResult(new List<Dictionary<string, object>>()));

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Results.Count, expectedResult.ReturnedObject.Results.Count);
        }

        [Test]
        public void Get_NotFound_Test()
        {
            //Arrange
            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.Get(1)).Returns((StatisticInDbModel)null);

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            var mockMapper = new Mock<IMapper>();

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<StatisticResult> actualResult = service.GetStatisticResult(1);
            var expectedResult = new ServiceResult<StatisticResult>(ServiceResultStatus.ItemNotFound);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public void GetAll_Success_Test()
        {
            //Arrange

            var statisticInDbModel = new StatisticInDbModel(1, "", "", "");
            var statisticModel = new StatisticModel(1, "", "", "");

            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.GetAll()).Returns(new List<StatisticInDbModel> { statisticInDbModel });

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<StatisticModel, StatisticInDbModel>(statisticInDbModel)).Returns(statisticModel);

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<StatisticModel>> actualResults = service.GetAll();
            var expectedResults = new ServiceResult<List<StatisticModel>>(ServiceResultStatus.ItemRecieved, new List<StatisticModel> { statisticModel });

            //Assert
            Assert.AreEqual(expectedResults.Status, actualResults.Status);
            Assert.AreEqual(expectedResults.ReturnedObject.Count, actualResults.ReturnedObject.Count);
            Assert.AreEqual(expectedResults.ReturnedObject[0].Id, actualResults.ReturnedObject[0].Id);
        }

        [Test]
        public void GetAll_GetNothing_Test()
        {
            //Arrange
            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.GetAll()).Returns(new List<StatisticInDbModel> { });

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<StatisticModel, StatisticInDbModel>(It.IsAny<StatisticInDbModel>())).Returns(It.IsAny<StatisticModel>());

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<StatisticModel>> actualResults = service.GetAll();
            var expectedResults = new ServiceResult<List<StatisticModel>>(ServiceResultStatus.NoContent);

            //Assert
            Assert.AreEqual(expectedResults.Status, actualResults.Status);
        }
    }
}