using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CatSharingManagement
{
    public class CatSharingCreateModel
    {
        public int CatId { get; }
        public int UserId { get; }

        public CatSharingCreateModel(int catId, int userId)
        {
            CatId = catId;
            UserId = userId;
        }
    }
}
