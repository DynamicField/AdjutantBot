using System;
using System.Threading.Tasks;
using AdjutantBot.Modules;
using AdjutantBot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdjutantBot
{
    public class AdjutantBotClient
    {
        private DiscordSocketClient _client;
        private IConfigurationRoot _config;

        public async Task LaunchBotAsync()
        {
            _client = new DiscordSocketClient();
            var services = ConfigureServices();
            services.GetRequiredService<LoggingService>();
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync(services).ConfigureAwait(false);
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("BOT_TOKEN")).ConfigureAwait(false);
            await _client.StartAsync().ConfigureAwait(false);
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<LoggingService>()
                .AddSingleton<IVerificationService, DummyGameJamService>()
                .BuildServiceProvider();
        }
    }
}