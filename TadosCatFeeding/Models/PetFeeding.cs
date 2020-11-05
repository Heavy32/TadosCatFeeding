using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding.Models
{
    public class PetFeeding
    {
        public int UserId { get; set; }
        public int PetId { get; set; }
        public string FeedTime { get; set; }
    }
}
