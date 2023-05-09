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
            CreateMap<DO.Task, TaskCreateUpdateDTO>().ReverseMap();
            CreateMap<DO.Task, TaskDTO>()
                .ForMember(x => x.TaskPriority, opt => opt.MapFrom(o => o.TaskPriority.Name))
                .ForMember(x => x.TaskStatus, opt => opt.MapFrom(o => o.TaskStatus.Name))
                .ForMember(x => x.AssignedToUserIds, opt => opt.MapFrom(o => o.TaskAssignments.Select(e => e.UserId).ToList()));
        }
    }
}
