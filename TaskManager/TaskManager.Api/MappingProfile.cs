using AutoMapper;
using TaskManager.Api.DO;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterUserDTO>().ReverseMap();
            CreateMap<User, BaseUserDTO>().ReverseMap();
            CreateMap<Team, TeamInfoDTO>().ReverseMap();
        }
    }
}
