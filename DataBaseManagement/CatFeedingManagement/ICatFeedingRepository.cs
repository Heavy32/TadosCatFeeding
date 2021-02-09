using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatFeedingManagement
{
    public interface ICatFeedingRepository
    {
        public int Create(CatFeedingCreateModel info);
        public List<DateTime> GetFeedingForPeriod(int userId, int catId, DateTime start, DateTime finish);

    }
}
