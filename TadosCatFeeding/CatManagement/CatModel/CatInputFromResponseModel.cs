using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.CatManagement
{
    public class CatInputFromResponseModel
    {
        public string Name { get; set;  }

        public CatInputFromResponseModel() { }

        //[JsonConstructor]
        //public CatInputFromResponseModel(string name)
        //{
        //    Name = name;
        //}
    }
}
