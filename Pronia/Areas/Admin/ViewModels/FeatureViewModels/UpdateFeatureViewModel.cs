using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Admin.ViewModels.FeatureViewModels
{
    public class UpdateFeatureViewModel
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }

        public IFormFile? Image { get; set; }



        public string Description { get; set; }

    }
}