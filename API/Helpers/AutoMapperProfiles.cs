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
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>() // Create a map for sender and recipient photo urls which are their main photos
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos
                    .FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos
                    .FirstOrDefault(x => x.IsMain).Url));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc)); // mapt date time to utc now
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? //For date read since it is optional create another mapping
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null); 

        }
    }
}