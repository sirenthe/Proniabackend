using Microsoft.Build.Framework;
using Pronia.Models.Common;

namespace Pronia.Models
{
    public class Product :BaseSectionEntity

    {
        public string Name { get; set; }
        
        public decimal Price{ get; set; }
        [Required]
        public string Description { get;set; }

        public int Rating { get;set; }



        public int CategoryId { get; set; }
        public Category Category { get; set; }



        public string Image { get; set; }

   
    }
}
