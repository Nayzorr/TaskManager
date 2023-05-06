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

        public async Task<ResponseDTO<bool>> AcceptTeamInvitationAsync(int userToAddId, string teamName)
        {
            var result = await _dbAccessor.AcceptTeamInvitationAsync(userToAddId, teamName);
            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<bool>> RejectTeamInvitationAsync(int userId, string teamName)
        {
            var result = await _dbAccessor.RejectTeamInvitationAsync(userId, teamName);
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

        public async Task<ResponseDTO<bool>> DeleteUserFromTheTeamAsync(int teamCreatorId, int userToDeleteId, string teamName)
        {
            var result = await _dbAccessor.DeleteUserFromTheTeamAsync(teamCreatorId, userToDeleteId, teamName);
            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<List<TeamInfoDTO>>> GetMyTeamInvitationsAsync(int userId)
        {
            var userTeamInvitations = await _dbAccessor.GetUserTeamInvitationsAsync(userId);

            var teams = await _dbAccessor.GetTeamsByTeamIds(userTeamInvitations.Select(e => e.TeamId).ToList());

            var mappedTeams = _mapper.Map<List<TeamInfoDTO>>(teams);

            foreach (var mappedTeam in mappedTeams)
            {
                var teamMembers = await _dbAccessor.GetTeamMembersByIdAsync(mappedTeam.Id);
                var mappedTeamMembers = _mapper.Map<List<BaseUserDTO>>(teamMembers);
                mappedTeam.TeamMembers = mappedTeamMembers;
            }

            return ResponseFormater.OK(mappedTeams);
        }

        public async Task<ResponseDTO<TeamInfoDTO>> GetTeamMainInfoByName(string teamName)
        {
            var result = await _dbAccessor.GetTeamMainInfoByNameAsync(teamName);

            var mappedTeam = _mapper.Map<TeamInfoDTO>(result);

            var teamMembers = await _dbAccessor.GetTeamMembersByIdAsync(mappedTeam.Id);

            var mappedTeamMembers = _mapper.Map<List<BaseUserDTO>>(teamMembers);

            mappedTeam.TeamMembers = mappedTeamMembers;

            return ResponseFormater.OK(mappedTeam);
        }

        public async Task<ResponseDTO<bool>> InvitePersonToTeam(int inviterId, int teamId, string personToInviteUserName)
        {
            var result = await _dbAccessor.InvitePersonToTeamAsync(inviterId, teamId, personToInviteUserName);

            //TODO: Add email invitation in future here
            return ResponseFormater.OK(result);

        }

        public async Task<ResponseDTO<bool>> СhangeTeamName(int teamCreatorId, ChangeTeamNameDTO changeTeamNameDTO)
        {
            var result = await _dbAccessor.СhangeTeamNameAsync(teamCreatorId, changeTeamNameDTO);

            return ResponseFormater.OK(result);
        }

        public async Task<ResponseDTO<bool>> DeleteTeamAsync(int userId,int teamIdToDelete)
        {
            var result = await _dbAccessor.DeleteTeamAsync(userId, teamIdToDelete);

            return ResponseFormater.OK(result);
        }
    }
}
