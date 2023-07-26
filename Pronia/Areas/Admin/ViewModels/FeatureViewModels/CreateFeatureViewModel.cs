using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels.FeatureViewModels
{
    public class CreateFeatureViewModel
    {
        public string Title { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public IFormFile Image { get; set; }
        [MaxLength(100)]

        public string Description { get; set; }


    }
}
