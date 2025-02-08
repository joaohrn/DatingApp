using System;
using Api.DTOs;
using Api.Entities;
using Api.Extensions;
using AutoMapper;
using AutoMapper.Execution;

namespace Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
            .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ReverseMap();
        CreateMap<Photo, PhotoDto>().ReverseMap();
        CreateMap<MemberUpdateDto, AppUser>().ReverseMap();
        CreateMap<RegisterDto, AppUser>();
        CreateMap<string,DateOnly>().ConvertUsing((s) => DateOnly.Parse(s));
    }
}
