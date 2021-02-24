using Microsoft.AspNetCore.Mvc;
using BusinessLogic;

namespace WebApi
{
    public interface IServiceResultStatusToResponseConverter
    {
        public IActionResult GetResponse<T>(ServiceResult<T> serviceResult, string modelPath = null);
    }
}
