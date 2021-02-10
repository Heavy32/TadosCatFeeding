using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManagement.CatSharingManagement
{
    public class CatSharingCreateInDbModel
    {
        public int CatId { get; }
        public int UserId { get; }

        public CatSharingCreateInDbModel(int catId, int userId)
        {
            CatId = catId;
            UserId = userId;
        }
    }
}
