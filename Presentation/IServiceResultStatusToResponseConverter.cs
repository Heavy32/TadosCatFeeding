using Microsoft.AspNetCore.Mvc;
using Services;

namespace Presentation
{
    public interface IServiceResultStatusToResponseConverter
    {
        public IActionResult GetResponse<T>(ServiceResult<T> serviceResult, string modelPath = null);
    }
}
