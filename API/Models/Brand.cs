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

        public byte[]? ImageData { get; set; }

        // Catégorie (ex: "VOD", "Gaming", "Audio", etc.)
        public string? Category { get; set; }
        public bool? Enabled { get; set; }
    }
}
