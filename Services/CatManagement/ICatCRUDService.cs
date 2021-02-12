using System.Collections.Generic;
using System.Security.Claims;

namespace Services.CatManagement
{
    public interface ICatCRUDService
    {
        public ServiceResult<CatServiceModel> Create(CatCreateServiceModel info, IEnumerable<Claim> userClaims);
        public ServiceResult<CatGetServiceModel> Get(int id);
    }
}
