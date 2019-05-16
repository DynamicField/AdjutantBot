using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AdjutantBot.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private IServiceProvider _provider;

        public CommandHandlingService(IServiceProvider provider, DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
            _provider = provider;
            _client.MessageReceived += MessageReceived;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _provider).ConfigureAwait(false);
            // Add additional initialization code here...
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message) || message.Source != MessageSource.User) return;

            int argPos = 0;
            if (!message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;
            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider).ConfigureAwait(false);

            if (result.Error.HasValue &&
                result.Error.Value != CommandError.UnknownCommand)
                await context.Channel.SendMessageAsync(result.ToString());
            else if (result.Error.HasValue &&
                     result.Error.Value == CommandError.UnknownCommand)
                await _commands.ExecuteAsync(context, "docs" + message.Content.Replace(_client.CurrentUser.Mention.Replace("!", ""), ""), _provider).ConfigureAwait(false);
        }

    }
}