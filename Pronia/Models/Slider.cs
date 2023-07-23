using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;
using Pronia.Models.Common;

namespace Pronia.Models
{
    public class Slider :BaseEntity
    {

        [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string Suptitle { get; set; }
      
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
