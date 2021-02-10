using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManagement.CatSharingManagement
{
    public interface ICatSharingRepository
    {
        public int Create(CatSharingCreateInDbModel info);
        public bool IsPetSharedWithUser(int userId, int petId);
    }
}
