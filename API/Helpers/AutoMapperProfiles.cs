using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl,  //Select the unmapped property
            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))// Find the photo with IsMain equal to true and map that url to the property
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())); 
            CreateMap<Photo, PhotoDto>();

        }
    }
}