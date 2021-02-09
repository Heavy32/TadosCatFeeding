using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatSharingManagement
{
    public interface ICatSharingRepository
    {
        public int Create(CatUserLink info);
        public bool IsPetSharedWithUser(int userId, int petId);
    }
}
