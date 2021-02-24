using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.CatManagement
{
    public class CatGetServiceModel
    {
        public int Id { get; }
        //[Required(ErrorMessage = "Pet's name is required")]
        public string Name { get; }
        //[Required(ErrorMessage = "Pet should have an owner!")]
        public int OwnerId { get; }

        public CatGetServiceModel(int id, string name, int ownerId)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
        }
    }
}
