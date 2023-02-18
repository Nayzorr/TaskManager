using AutoMapper;
using TaskManager.Api.DO;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Team, TeamInfoDTO>().ReverseMap();
        }
    }
}
