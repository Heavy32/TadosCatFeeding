using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.StatisticProvision;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticCRUDService statisticService;
        private readonly IServiceResultStatusToResponseConverter responseConverter;

        public StatisticController(IStatisticCRUDService statisticService, IServiceResultStatusToResponseConverter responseConverter)
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

        //shouldn't be here
    //    [HttpGet("~/users/{userId}/cats/{catId}/feedings")]
    //    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //    public IActionResult GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish)
    //    {
    //        UserModel user = context.UserRepository.Get(userId);
    //        if (user == null)
    //        {
    //            return NotFound("User cannot be found");
    //        }

    //        CatModel cat = context.CatRepository.Get(catId);
    //        if (cat == null)
    //        {
    //            return NotFound("Cat cannot be found");
    //        }

    //        if (user.Login != User.Identity.Name)
    //        {
    //            return Forbid();
    //        }

    //        return Ok(context.StatisticRepository.GetFeedingForPeriod(userId, catId, start, finish));
    //    }
    }
}
