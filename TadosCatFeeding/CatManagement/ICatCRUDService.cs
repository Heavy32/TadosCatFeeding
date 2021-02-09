using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatManagement
{
    public interface ICatCRUDService
    {
        public ServiceResult<CatModel> Create(CatCreateModel info);
        public ServiceResult<CatModel> Get(int id);
    }
}
