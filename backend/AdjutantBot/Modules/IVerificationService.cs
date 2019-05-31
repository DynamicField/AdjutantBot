using System.Collections.Generic;
using System.Threading.Tasks;
using AdjutantBot.Models;
using Discord;

namespace AdjutantBot.Modules
{
    public interface IVerificationService
    {
        Task<TeamModel> CreateTeamAsync(string teamName, ulong guildId, ulong teamMember);
        
        Task<(AddTeamMemberModificationResponse Result, TeamModel Team)> AddToTeamAsync(string teamName, ulong guildId, ulong teamMember);

        Task<(SetLimitModifierResponse Result, TeamModel Team)> SetTeamLimitAsync(string teamName, ulong guildId, ulong teamMember, int teamLimit);

        Task<List<TeamModel>> RevealTeamListAsync();
    }
}