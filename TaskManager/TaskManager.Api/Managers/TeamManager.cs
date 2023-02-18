using AutoMapper;
using TaskManager.Api.Accessors.Interfaces;
using TaskManager.Api.DO;
using TaskManager.Api.Managers.Interfaces;
using TaskManager.Api.Models.DTOs;

namespace TaskManager.Api.Managers
{
    public class TeamManager : ITeamManager
    {
        private readonly IDBAccessor _dbAccessor;
        private readonly IMapper _mapper;

        public TeamManager(IDBAccessor dBAccessor, IMapper mapper)
        {
            _dbAccessor = dBAccessor;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<bool>> CreateTeam(int userId, CreateTeamDTO createTeamDTO)
        {

            Team teamToCreate = new Team()
            {
                CreatorId = userId,
                TeamName = createTeamDTO.TeamName,
                DateCreated = DateTime.Now,
            };

            var result = await _dbAccessor.CreateTeam(teamToCreate);

            return ResponseFormater.OK(result);
        }
    }
}
