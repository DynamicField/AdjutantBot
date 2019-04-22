using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdjutantBot.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping"), Summary("pong!")]
        public async Task Ping()
        {
            await ReplyAsync("pong!");
        }

        [Command("kick"), Summary("Kick the specified user")]
        public async Task KickUser(IGuildUser targetUser, [Remainder] string reason = null)
        {
            await targetUser.SendMessageAsync("You have been kicked from the Game Development Society " + reason);
            await targetUser.KickAsync();
        }

        [Command("ban"), Summary("Ban the specified user")]
        public async Task BanUser(IGuildUser targetUser, [Remainder] string reason = null)
        {
            await targetUser.SendMessageAsync("You have been banned from the Game Development Society " + reason);
            await targetUser.BanAsync();
        }
    }
}
