using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ConsoleHealthCheck.Services
{

    public interface IDiscordBotService
    {
        public Task MainAsync();
        public Task Client_Ready();
    }

    public class DiscordBotService : IDiscordBotService
    {
        private ISchedulerService _schedulerService;
        private DiscordSocketClient _client;

        public DiscordBotService(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();


            _client.Log += Log;
            _client.SlashCommandExecuted += SlashCommandHandler;
            _client.Ready += Client_Ready;

            var token = File.ReadAllText("auth.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            //ClearList();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            if (command.CommandName == "add_console")
            {
                try
                {
                    await command.DeferAsync();
                    var cmdData = command.Data.Options.ToArray();
                    Console.WriteLine($"Scheduled Console: {cmdData[0].Value.ToString()} was scheduled with schedule {cmdData[1].Value.ToString()}");
                    _schedulerService.ScheduleJob(cmdData[0].Value.ToString(), cmdData[1].Value.ToString());
                    await command.ModifyOriginalResponseAsync( msg => msg.Content = "Console is now scheduled!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await command.ModifyOriginalResponseAsync(msg => msg.Content = "Failed to schedule.");
                }
            }
        }

        public async Task Client_Ready()
        {
            ulong guildId = 1063949689591959722;

            var guildCommand = new SlashCommandBuilder()
                .WithName("add_console")
                .WithDescription("adds a console to the schedule with provided cron job")
                .AddOption("console_name", ApplicationCommandOptionType.String, "Name of the console", isRequired: true)
                .AddOption("cron_schedule", ApplicationCommandOptionType.String, "Cron expression for schedule",
                    isRequired: true);
            try
            {
                await _client.Rest.CreateGuildCommand(guildCommand.Build(), guildId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}