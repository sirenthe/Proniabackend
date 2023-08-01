using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;
using Pronia.Models.Common;

namespace Pronia.Models
{
    public  class Category :BaseSectionEntity
    {
        [Required]
        public string Name { get; set; }
        

        public ICollection<Product>?   Products { get; set; }

        //[NotMapped]
        //public override DateTime CreatedDate { get; set; }
    }
}
