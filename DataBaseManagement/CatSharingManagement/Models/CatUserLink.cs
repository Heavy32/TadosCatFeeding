using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatSharingManagement
{
    public class CatUserLink
    {
        public int CatId { get; }
        public int UserId { get; }

        public CatUserLink(int catId, int userId)
        {
            CatId = catId;
            UserId = userId;
        }
    }
}
