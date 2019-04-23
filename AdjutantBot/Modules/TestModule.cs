using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdjutantBot.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        public const string MuteRoleName
    = "Cone-of-Shame";
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

        [Command("mute"), Summary("Mute the specified user")]
        public async Task MuteUser(IGuildUser targetUser)
        {
            var guild = targetUser.Guild;
            var muteRole = await GetOrCreateMuteRoleAsync(guild);

            if (targetUser.RoleIds.Contains(muteRole.Id))
                throw new InvalidOperationException($"Discord user {targetUser} is already muted");

            foreach (var channel in await guild.GetChannelsAsync())
                await ConfigureChannelMuteRolePermissionsAsync(channel, muteRole);

            await targetUser.AddRoleAsync(muteRole);
            await ReplyAsync(targetUser + " has been muted");
        }

        [Command("unmute"), Summary("Mute the specified user")]
        public async Task UnMuteUser(IGuildUser targetUser)
        {
            var guild = targetUser.Guild;
            var unMuteRole = await GetOrCreateMuteRoleAsync(guild);

            if (!targetUser.RoleIds.Contains(unMuteRole.Id))
                throw new InvalidOperationException($"Discord user {targetUser} is not currently muted");

            await targetUser.RemoveRoleAsync(unMuteRole);
            await ReplyAsync(targetUser + " has been unmuted");
        }

        private async Task ConfigureChannelMuteRolePermissionsAsync(IGuildChannel channel, IRole muteRole)
        {
            var permissionOverwrite = channel.GetPermissionOverwrite(muteRole);
            if (permissionOverwrite != null)
            {
                if ((permissionOverwrite.Value.AllowValue == _mutePermissions.AllowValue) &&
                    (permissionOverwrite.Value.DenyValue == _mutePermissions.DenyValue))
                    return;

                await channel.RemovePermissionOverwriteAsync(muteRole);
            }

            await channel.AddPermissionOverwriteAsync(muteRole, _mutePermissions);
        }

        private async Task<IRole> GetOrCreateMuteRoleAsync(IGuild guild)
            => guild.Roles.FirstOrDefault(x => x.Name == MuteRoleName)
                ?? await guild.CreateRoleAsync(MuteRoleName);


        private static readonly OverwritePermissions _mutePermissions
            = new OverwritePermissions(
                sendMessages: PermValue.Deny,
                sendTTSMessages: PermValue.Deny,
                createInstantInvite: PermValue.Deny,
                embedLinks: PermValue.Deny,
                attachFiles: PermValue.Deny,
                mentionEveryone: PermValue.Deny,
                useExternalEmojis: PermValue.Deny,
                addReactions: PermValue.Deny,
                useVoiceActivation: PermValue.Deny,
                muteMembers: PermValue.Deny,
                deafenMembers: PermValue.Deny,
                moveMembers: PermValue.Deny,
                speak: PermValue.Deny);
    }
}
