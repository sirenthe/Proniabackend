using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;
using Pronia.Models.Common;

namespace Pronia.Models
{
    public class Feature: BaseEntity
    {

        public string Title { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Image { get; set; }
        [MaxLength(100)]
        
        public string Description { get; set; }
        public bool IsDeleted { get; set; }  
    }
}
