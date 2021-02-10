using Microsoft.AspNetCore.Mvc;
using Services;
using Services.StatisticProvision;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService statisticService;
        private readonly IServiceResultStatusToResponseConverter responseConverter;

        public StatisticController(IStatisticService statisticService, IServiceResultStatusToResponseConverter responseConverter)
        {
            this.statisticService = statisticService;
            this.responseConverter = responseConverter;
        }

        [HttpGet("~/cats/feedings/statistics/{statisticId}")]
        public IActionResult Execute(int statisticId)
        {
            return responseConverter.GetResponse(statisticService.GetStatisticResult(statisticId));
        }

        [HttpGet("~/cats/feedings/statistics")]     
        public IActionResult GetAll()
        {
            return responseConverter.GetResponse(statisticService.GetAll());   
        }
    }
}
