using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.Models
{
    public class Pet
    {
        [Required(ErrorMessage = "Pet's name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pet should have an owner!")]
        public int OwnerId { get; set; }
    }
}
