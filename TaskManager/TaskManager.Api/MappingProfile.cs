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
            CreateMap<User, BaseUserDTO>();
            CreateMap<BaseUserDTO, User>().ForMember(dest => dest.PasswordHash, opt => opt.UseDestinationValue());
            CreateMap<Team, TeamInfoDTO>().ReverseMap();
        }
    }
}
