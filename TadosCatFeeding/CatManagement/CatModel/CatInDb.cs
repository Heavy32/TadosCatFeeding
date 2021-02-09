using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatManagement
{
    public class CatInDb
    {
        public int Id { get; }
        public string Name { get; }
        public int OwnerId { get; }

        public CatInDb(int id, string name, int ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }
    }
}
