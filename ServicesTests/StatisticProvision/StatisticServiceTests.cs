using NUnit.Framework;
using Moq;
using DataBaseManagement.StatisticProvision;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.StatisticProvision.Tests
{
    public class StatisticServiceTests
    {
        [Test]
        public async Task Execute_Success_Test()
        {
            //Arrange
            var mockStatisticRepository = new Mock<IStatisticRepository>();
            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            mockStatisticCalculation.Setup(calculation => calculation.ExecuteAsync("expression")).Returns(Task.FromResult(new StatisticResult(new List<Dictionary<string, object>>())));
            var mockMapper = new Mock<IMapper>();

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            StatisticResult actualResult = await service.ExecuteAsync("expression");
            var expectedResult = new StatisticResult(new List<Dictionary<string, object>>());

            //Assert
            Assert.AreEqual(actualResult.Results.Count, expectedResult.Results.Count);
        }

        [Test]
        public async Task Get_Success()
        {
            //Arrange
            var statisticInDbModel = new StatisticInDbModel(1, "", "", "");
            var statisticModel = new StatisticModel(1, "", "", "");

            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(statisticInDbModel));

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            mockStatisticCalculation.Setup(calculation => calculation.ExecuteAsync(statisticInDbModel.SqlExpression)).Returns(Task.FromResult(new StatisticResult(new List<Dictionary<string, object>>())));
            var mockMapper = new Mock<IMapper>();

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<StatisticResult> actualResult = await service.GetStatisticResultAsync(1);
            var expectedResult = new ServiceResult<StatisticResult>(ServiceResultStatus.ItemRecieved, new StatisticResult(new List<Dictionary<string, object>>()));

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Results.Count, expectedResult.ReturnedObject.Results.Count);
        }

        [Test]
        public async Task Get_NotFound_Test()
        {
            //Arrange
            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((StatisticInDbModel)null));

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            var mockMapper = new Mock<IMapper>();

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<StatisticResult> actualResult = await service.GetStatisticResultAsync(1);
            var expectedResult = new ServiceResult<StatisticResult>(ServiceResultStatus.ItemNotFound);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
        }

        [Test]
        public async Task GetAll_Success_Test()
        {
            //Arrange
            var statisticInDbModel = new StatisticInDbModel(1, "", "", "");
            var statisticModel = new StatisticModel(1, "", "", "");

            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.GetAllAsync()).Returns(Task.FromResult(new List<StatisticInDbModel> { statisticInDbModel }));

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<StatisticModel, StatisticInDbModel>(statisticInDbModel)).Returns(statisticModel);

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<StatisticModel>> actualResults = await service.GetAllAsync();
            var expectedResults = new ServiceResult<List<StatisticModel>>(ServiceResultStatus.ItemRecieved, new List<StatisticModel> { statisticModel });

            //Assert
            Assert.AreEqual(expectedResults.Status, actualResults.Status);
            Assert.AreEqual(expectedResults.ReturnedObject.Count, actualResults.ReturnedObject.Count);
            Assert.AreEqual(expectedResults.ReturnedObject[0].Id, actualResults.ReturnedObject[0].Id);
        }

        [Test]
        public async Task GetAll_GetNothing_Test()
        {
            //Arrange
            var mockStatisticRepository = new Mock<IStatisticRepository>();
            mockStatisticRepository.Setup(repository => repository.GetAllAsync()).Returns(Task.FromResult(new List<StatisticInDbModel> { }));

            var mockStatisticCalculation = new Mock<IStatisticCalculation>();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<StatisticModel, StatisticInDbModel>(It.IsAny<StatisticInDbModel>())).Returns(It.IsAny<StatisticModel>());

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<List<StatisticModel>> actualResults = await service.GetAllAsync();
            var expectedResults = new ServiceResult<List<StatisticModel>>(ServiceResultStatus.NoContent);

            //Assert
            Assert.AreEqual(expectedResults.Status, actualResults.Status);
        }
    }
}