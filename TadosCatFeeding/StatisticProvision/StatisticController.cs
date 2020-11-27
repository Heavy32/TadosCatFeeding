using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TadosCatFeeding.CatManagement;
using TadosCatFeeding.StatisticProvision;
using TadosCatFeeding.UserManagement;

namespace TadosCatFeeding.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IContext context;

        public StatisticController(IContext context)
        {
            this.context = context;
        }

        [HttpGet("~/cats/feedings/statistics/{statisticId}")]
        public IActionResult Execute(int statisticId)
        {
            StatisticCalculation calculation = new StatisticCalculation(context.StatisticRepository.ConnectionString);

            string sqlExpression = context.StatisticRepository.Get(statisticId).SqlExpression;

            return Ok(calculation.Execute(sqlExpression));
        }

        [HttpGet("~/cats/feedings/statistics")]     
        public IActionResult GetAll()
        {
            List<StatisticModel> list = context.StatisticRepository.GetAll();
            if(list.Count == 0)
            {
                return NoContent();
            }
            return Ok(list);           
        }

        [HttpGet("~/users/{userId}/cats/{catId}/feedings")]
        public IActionResult GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish)
        {
            UserModel user = context.UserRepository.Get(userId);
            if(user == null)
            {
                return NotFound("User cannot be found");
            }

            CatModel cat = context.CatRepository.Get(catId);
            if(cat == null)
            {
                return NotFound("Cat cannot be found");
            }

            if (user.Login != User.Identity.Name)
            {
                return Forbid();
            }

            return Ok(context.StatisticRepository.GetFeedingForPeriod(userId, catId, start, finish));
        }
    }
}
