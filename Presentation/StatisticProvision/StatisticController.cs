using Microsoft.AspNetCore.Mvc;
using Services;
using Services.StatisticProvision;
using System.Collections.Generic;

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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Execute(int statisticId)
        {
            return responseConverter.GetResponse(statisticService.GetStatisticResult(statisticId));
        }

        [HttpGet("~/cats/feedings/statistics")]
        [ProducesResponseType(typeof(List<StatisticModel>), 200)]
        [ProducesResponseType(204)]
        public IActionResult GetAll()
        {
            return responseConverter.GetResponse(statisticService.GetAll());   
        }
    }
}
