using Pronia.Identity;
using Pronia.ViewModels;
using AutoMapper;

namespace Pronia.Mappers
{
    public class UserMapperProfile :Profile
    {
        public UserMapperProfile() {
            CreateMap<RegisterViewModel,AppUser>().ReverseMap();
        }

       
    }
}
