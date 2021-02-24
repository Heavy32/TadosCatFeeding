using Microsoft.AspNetCore.Mvc;
using BusinessLogic.StatisticProvision;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
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
        public async Task<IActionResult> ExecuteAsync(int statisticId)
        {
            return responseConverter.GetResponse(await statisticService.GetStatisticResultAsync(statisticId));
        }

        [HttpGet("~/cats/feedings/statistics")]
        [ProducesResponseType(typeof(List<StatisticModel>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAllAsync()
        {
            return responseConverter.GetResponse(await statisticService.GetAllAsync());   
        }
    }
}
