using System.Collections.Generic;
using System.Threading.Tasks;
using AdjutantBot.Models;
using AdjutantBot.Modules;
using Discord;

namespace AdjutantBot.Services
{
    public class DummyGameJamService : IVerificationService
    {
        private List<TeamModel> _teams = new List<TeamModel>();
        public async Task<TeamModel> CreateTeamAsync(string teamName, ulong guildId, ulong teamCaptain)
        {
            var team = new TeamModel(teamName, guildId, 123456, teamCaptain);
            _teams.Add(team);
            return team;
        }

        public async Task<(AddTeamMemberModificationResponse Result, TeamModel Team)> AddToTeamAsync(string teamName, ulong guildId, ulong teamMember)
        {
            foreach (var TEAM in _teams)
            {
                if (TEAM.TeamName != teamName || TEAM.GuildId != guildId) continue;
                
                var team = TEAM;
                if (team.TeamMemberIds.Count >= team.TeamLimit) return (AddTeamMemberModificationResponse.FullMembership, team);
                foreach (var TeamMember in team.TeamMemberIds)
                {
                    if (TeamMember != teamMember) continue;
                    return (AddTeamMemberModificationResponse.AlreadyMember, team);
                }
                team.TeamMemberIds.Add(teamMember);
                return (AddTeamMemberModificationResponse.Success, team);
            }
            return (AddTeamMemberModificationResponse.NotFound, null);
        }

        public async Task<(SetLimitModifierResponse Result, TeamModel Team)> SetTeamLimitAsync(string teamName, ulong guildId, ulong teamMember, int teamLimit)
        {
            foreach (var TEAM in _teams)
            {
                if (TEAM.TeamName != teamName || TEAM.GuildId != guildId) continue;

                var team = TEAM;
                if (team.TeamCaptainId != teamMember) return (SetLimitModifierResponse.Unauthorized, team);
                team.TeamLimit = teamLimit;
                return (SetLimitModifierResponse.Success, team);
            }

            return (SetLimitModifierResponse.Failure, null);
        }

        public async Task<List<TeamModel>> RevealTeamListAsync()
        {
            return _teams;
        }
    }
}