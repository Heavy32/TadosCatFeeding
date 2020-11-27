using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.Abstractions;
using TadosCatFeeding.Models;

namespace TadosCatFeeding.CatManagement
{
    public interface ICatRepository : IRepository<CatModel>
    {
        
    }
}
