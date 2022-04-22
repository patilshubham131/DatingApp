using System.Linq;
using AutoMapper;
using API.DTOs;
using API.Entities;
namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, 
                opt=> opt.MapFrom(
                    src => src.Photos.FirstOrDefault(
                        x=> x.IsMain)
                        .Url));
            CreateMap<Photo, PhotoDto>();
        }
    }
}