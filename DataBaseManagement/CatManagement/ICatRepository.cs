using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatManagement
{
    public interface ICatRepository
    {
        public int Create(CatCreateModel info);
        public CatModel Get(int id);

    }
}
