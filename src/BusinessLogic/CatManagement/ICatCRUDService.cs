using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLogic.CatManagement
{
    public interface ICatCRUDService
    {
        public Task<ServiceResult<CatServiceModel>> CreateAsync(CatCreateServiceModel info, IEnumerable<Claim> userClaims);
        public Task<ServiceResult<CatGetServiceModel>> GetAsync(int id);
    }
}
