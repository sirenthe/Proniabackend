﻿using Microsoft.Build.Framework;

namespace Pronia.Areas.Admin.ViewModels.ProductViewModel
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int CategoryId { get; set; }
    }
}
