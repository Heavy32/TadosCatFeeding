using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadosCatFeeding.Abstractions;

namespace TadosCatFeeding.CatSharingManagement
{
    public interface ICatSharingRepository : IRepository<CatSharingModel>
    {
        bool IsPetSharedWithUser(int userId, int petId);
    }
}
