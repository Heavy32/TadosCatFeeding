using System;
using System.ComponentModel.DataAnnotations;

namespace TadosCatFeeding.Models
{
    public class FeedingTimePeriod
    {
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime Finish { get; set; }
    }
}
