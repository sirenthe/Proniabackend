using AutoMapper;
using Pronia.Areas.Admin.ViewModels.ProductViewModel;
using Pronia.Models;

namespace Pronia.Mappers
{
    public class ProductMapperProfile : Profile
    {
        public  ProductMapperProfile()
            {
            CreateMap<CreateProductViewModel, Product>();
          CreateMap<Product,UpdateProductViewModel >().ForMember(pwm=>pwm.Image,x=>x.Ignore())
                .ReverseMap();
            
        }

            
    }
}
