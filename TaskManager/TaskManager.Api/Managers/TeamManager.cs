using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ResponseDTO<bool>> AddUserToTheTeam(int teamCreatorId, int userToAddId)
        {
            var result = await _dbAccessor.AddUserToTheTeamAsync(teamCreatorId, userToAddId);
            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<bool>> CheckIfTeamNameUnique(CreateTeamDTO teamDto)
        {
            var result = await _dbAccessor.CheckIfTeamNameUniqueAsync(teamDto);
            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<bool>> CreateTeam(int userId, CreateTeamDTO createTeamDTO)
        {

            Team teamToCreate = new Team()
            {
                CreatorId = userId,
                TeamName = createTeamDTO.TeamName,
                DateCreated = DateTime.Now,
            };

            var result = await _dbAccessor.CreateTeamAsync(teamToCreate);

            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<TeamInfoDTO>> GetTeamMainInfoByName(string teamName)
        {
            var result = await _dbAccessor.GetTeamMainInfoByNameAsync(teamName);

            var mappedTeam = _mapper.Map<TeamInfoDTO>(result);

            var teamMembers = await _dbAccessor.GetTeamMembertsByIdAsync(mappedTeam.Id);

            var mappedTeamMembers = _mapper.Map<List<UserDTO>>(teamMembers);

            mappedTeam.TeamMembers = mappedTeamMembers;

            return ResponseFormater.OK(mappedTeam);
        }

        public async Task<ResponseDTO<bool>> InvitePersonToTeam(int inviterId, int teamId, string personToInviteUserName)
        {
            var result = await _dbAccessor.InvitePersonToTeamAsync(inviterId, teamId, personToInviteUserName);

            //TODO: Add email invitation in future here
            return ResponseFormater.OK(result);

        }
    }
}
