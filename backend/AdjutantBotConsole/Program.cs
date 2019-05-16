using System;
using System.Threading.Tasks;
using AdjutantBot;

namespace AdjutantBotConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tcs = new TaskCompletionSource<bool>();
            
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                tcs.SetResult(true);
            };

            var t = new AdjutantBotClient().LaunchBotAsync();

            await Task.WhenAll(t, tcs.Task);
        }
    }
}