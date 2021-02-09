using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatManagement
{
    public class CatCreateModel
    {
        public string Name { get; }
        public int OwnerId { get; }

        [JsonConstructor]
        public CatCreateModel(string name, int ownerId)
        {
            Name = name;
            OwnerId = ownerId;
        }
    }
}
