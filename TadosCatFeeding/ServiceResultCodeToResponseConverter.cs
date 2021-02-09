﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding
{
    public class ServiceResultCodeToResponseConverter : IServiceResultStatusToResponseConverter
    {
        public IActionResult GetResponse<T>(ServiceResult<T> result, string modelLocation = null)
            => result.Status switch
            {
                ServiceResultStatus.PetIsNotShared => new ObjectResult(result.Message) { StatusCode = 403 },
                ServiceResultStatus.PetIsShared => new NoContentResult(),
                ServiceResultStatus.IncorrectLoginPassword => new UnauthorizedResult(),
                ServiceResultStatus.ItemCreated => new CreatedResult(modelLocation + (result.ReturnedObject as IUniqueModel)?.Id, result.ReturnedObject),
                ServiceResultStatus.ItemChanged => new NoContentResult(),
                ServiceResultStatus.ItemRecieved => new OkObjectResult(result.ReturnedObject),
                ServiceResultStatus.ItemNotFound => new NotFoundObjectResult(result.ReturnedObject),
                ServiceResultStatus.ItemDeleted => new NoContentResult(),
                ServiceResultStatus.ItemIsNotCreated => new ObjectResult("Internal error, sorry... Our programmers have just been waken up and forced to fix it") { StatusCode = 500 },
                ServiceResultStatus.NoContent => new NoContentResult(),
                ServiceResultStatus.CantShareWithUser => new ObjectResult(result.ReturnedObject) { StatusCode = 403 },
                _ => new BadRequestResult(),
            };
    }
}
