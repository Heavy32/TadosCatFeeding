using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatFeedingManagement
{
    public interface ICatFeedingService
    {
        public ServiceResult<CatFeedingModel> Feed(CatFeedingCreateModel info);
    }
}
