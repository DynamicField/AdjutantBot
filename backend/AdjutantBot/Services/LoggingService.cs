using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace AdjutantBot.Services
{
    public class LoggingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        private readonly string _logDirectory;
        private readonly string _logFile;
        private readonly object _lockObj;

        // DiscordSocketClient and CommandService are injected automatically from the IServiceProvider
        public LoggingService(DiscordSocketClient client, CommandService commands)
        {
            _logFile = Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyy.MM.dd")}.log");
            _lockObj = new object();
            // Possibly change to log to SysLog on Linux
            _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            _client = client;
            _commands = commands;

            _client.Log += OnLogAsync;
            _commands.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            // Create log directory or file if they do not exist
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
            if (!File.Exists(_logFile))
                File.Create(_logFile).Dispose();

            string logText = $"{DateTime.UtcNow:hh:mm:ss} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";

            lock (_lockObj)
            {
                File.AppendAllText(_logFile, logText + Environment.NewLine);
            }

            return Console.Out.WriteLineAsync(logText);
        }
    }
}