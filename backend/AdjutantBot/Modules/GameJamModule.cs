using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdjutantBot.Models;
using Discord;
using Discord.Commands;

namespace AdjutantBot.Modules
{
    public class GameJamModule : ModuleBase<SocketCommandContext>
    {
        private IVerificationService _verificationService;

        public GameJamModule(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [Command("createteam")]
        public async Task CreateTeam([Remainder] string teamName = null)
        {
            if (teamName != null)
            {
                TeamModel team = await _verificationService.CreateTeamAsync(teamName, Context.Guild.Id, Context.User.Id);
                await ReplyAsync($"Created team {team.TeamName}. Team Leader is {Context.User.Mention}. Game jam ID is {team.GameJamId}.");
            }
            else await ReplyAsync("You must give a team name.");
        }

        [Command("enterteam")]
        public async Task EnterTeam([Remainder] string teamName = null)
        {
            if (teamName != null)
            {
                (AddTeamMemberModificationResponse Result, TeamModel Team) team;
                team = await _verificationService.AddToTeamAsync(teamName, Context.Guild.Id, Context.User.Id);
                switch(team.Result)
                {
                    case AddTeamMemberModificationResponse.Success:
                        await ReplyAsync($"Added to team {team.Team.TeamName}. Your Team Leader is {Context.Client.GetUser(team.Team.TeamCaptainId).Mention}. Game jam ID is {team.Team.GameJamId}.");
                        break;
                    case AddTeamMemberModificationResponse.NotFound:
                        await ReplyAsync($"There was no team by the name {teamName}.");
                        break;
                    case AddTeamMemberModificationResponse.AlreadyMember:
                        await ReplyAsync($"You are already a member of {team.Team.TeamName}");
                        break;
                    case AddTeamMemberModificationResponse.FullMembership:
                        await ReplyAsync(
                            $"{team.Team.TeamName} is already full on members. Please contact the Team Leader {Context.Client.GetUser(team.Team.TeamCaptainId).Mention} if this is a mistake");
                        break;
                }
            }
            else await ReplyAsync("You must give a team name.");
        }

        [Command("setlimit")]
        public async Task SetTeamLimit(int limit = default, [Remainder] string teamName = null)
        {
            if (teamName != null && limit != default)
            {
                (SetLimitModifierResponse Result, TeamModel Team) team;
                team = await _verificationService.SetTeamLimitAsync(teamName, Context.Guild.Id, Context.User.Id, limit);
                switch (team.Result)
                {
                    case SetLimitModifierResponse.Success:
                        await ReplyAsync(
                            $"The team limit for {team.Team.TeamName} has been modified to accept {team.Team.TeamLimit} users");
                        break;
                    case SetLimitModifierResponse.Failure:
                        await ReplyAsync("Sorry, I couldn't find that team.");
                        break;
                    case SetLimitModifierResponse.Unauthorized:
                        await ReplyAsync(
                            $"You are not authorized to perform that action. Only {Context.Client.GetUser(team.Team.TeamCaptainId).Mention} can change the team size");
                        break;
                }
            }

            if (teamName == null) await ReplyAsync("You need to input a team name");
            if (limit == default) await ReplyAsync("You need to input a team limit");
        }
    }
}