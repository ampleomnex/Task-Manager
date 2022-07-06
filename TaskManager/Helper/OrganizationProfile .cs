using AutoMapper;
using TaskManager.Models;

namespace TaskManager.Helper
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<User, AppUser>().ConstructUsing(u => new AppUser { FirstName = u.FirstName, LastName = u.LastName  });
            CreateMap<AppUser, User>().ConstructUsing(au => new User(au.FirstName,au.LastName,au.Email,au.UserName,Guid.NewGuid()));

        }
    }
}
