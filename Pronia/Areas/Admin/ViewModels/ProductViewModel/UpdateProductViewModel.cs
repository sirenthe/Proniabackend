namespace Pronia.Areas.Admin.ViewModels.ProductViewModel
{
    public class UpdateProductViewModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile?  Image { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int CategoryId { get; set; }
    }
}
