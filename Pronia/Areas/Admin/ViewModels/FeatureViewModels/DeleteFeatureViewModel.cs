using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels.FeatureViewModels
{
    public class DeleteFeatureViewModel
    {
   
        public string Title { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Image { get; set; }
        [MaxLength(100)]

        public string Description { get; set; }
    }
}
