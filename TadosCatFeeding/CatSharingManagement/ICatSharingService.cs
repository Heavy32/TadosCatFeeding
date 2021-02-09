using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatSharingManagement
{
    public interface ICatSharingService
    {
        public ServiceResult<CatSharingModel> Share(CatUserLink info, int ownerId);
    }
}
