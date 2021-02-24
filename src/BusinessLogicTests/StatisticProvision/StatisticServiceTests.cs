using NUnit.Framework;
using Moq;
using DataBaseRepositories.StatisticProvision;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.StatisticProvision.Tests
{
    public class StatisticServiceTests
    {
        private static readonly StatisticInDbModel statisticInDbModel = new StatisticInDbModel(1, "", "", "");
        private readonly StatisticResult statisticResult = new StatisticResult(new List<Dictionary<string, object>>());
        private readonly StatisticModel statisticModel = new StatisticModel(1, "", "", "");
        private readonly List<StatisticInDbModel> statisticModels = new List<StatisticInDbModel> { statisticInDbModel };
        
        private readonly Mock<IStatisticRepository> mockStatisticRepository = new Mock<IStatisticRepository>();
        private readonly Mock<IStatisticCalculation> mockStatisticCalculation = new Mock<IStatisticCalculation>();
        private readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        
        [Test]
        public async Task Execute_Success_Test()
        {
            //Arrange
            mockStatisticCalculation.Setup(calculation => calculation.ExecuteAsync("expression")).Returns(Task.FromResult(statisticResult));

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            StatisticResult actualResult = await service.ExecuteAsync("expression");
            var expectedResult = statisticResult;

            //Assert
            Assert.AreEqual(actualResult.Results.Count, expectedResult.Results.Count);
        }

        [Test]
        public async Task Get_Success()
        {
            //Arrange
            mockStatisticRepository.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult(statisticInDbModel));
            mockStatisticCalculation.Setup(calculation => calculation.ExecuteAsync(statisticInDbModel.SqlExpression)).Returns(Task.FromResult(statisticResult));

            var service = new StatisticService(
                mockStatisticRepository.Object,
                mockStatisticCalculation.Object,
                mockMapper.Object);

            //Action
            ServiceResult<StatisticResult> actualResult = await service.GetStatisticResultAsync(1);
            var expectedResult = new ServiceResult<StatisticResult>(ServiceResultStatus.ItemRecieved, statisticResult);

            //Assert
            Assert.AreEqual(expectedResult.Status, actualResult.Status);
            Assert.AreEqual(expectedResult.ReturnedObject.Results.Count, expectedResult.ReturnedObject.Results.Count);
        }

        [Test]
        public async Task Get_NotFound_Test()
        {
            //Arrange
            mockStatisticRepository.Setup(repository => repository.GetAsync(1)).Returns(Task.FromResult((StatisticInDbModel)null));

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
            mockStatisticRepository.Setup(repository => repository.GetAllAsync()).Returns(Task.FromResult(statisticModels));
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
            mockStatisticRepository.Setup(repository => repository.GetAllAsync()).Returns(Task.FromResult(new List<StatisticInDbModel> { }));
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