using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
