using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace Pronia.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Image { get; set; }
        [MaxLength(100)]
        
        public string Description { get; set; }

    }
}
