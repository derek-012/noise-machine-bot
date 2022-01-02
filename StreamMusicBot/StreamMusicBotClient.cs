using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Victoria;
using NoiseMachine.Services;
using NoiseMachine.Entities;

namespace NoiseMachine
{
    public class NoiseMachineClient
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private IServiceProvider _services;
        private readonly LogService _logService;
        private readonly ConfigService _configService;
        private readonly Config _config;
        private Configuration _configuration;

        public NoiseMachineClient()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = LogSeverity.Debug
            });


            _cmdService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false
            });

            _logService = new LogService();
            _configService = new ConfigService();
            _config = _configService.GetConfig();
        }

        public async Task InitializeAsync()
        {



            _configuration = new Configuration();
            _configuration.Host = "lava.link";
            _configuration.Port = 80;
            _configuration.Password = "12345678";

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            _client.Log += LogAsync;
            _services = SetupServices();

            var cmdHandler = new CommandHandler(_client, _cmdService, _services);
            await cmdHandler.InitializeAsync();

            await _services.GetRequiredService<MusicService>().InitializeAsync();
            await Task.Delay(-1);
        }

        private async Task LogAsync(LogMessage logMessage)
        {
            await _logService.LogAsync(logMessage);
        }

        private IServiceProvider SetupServices()
            => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_cmdService)
            .AddSingleton(_logService)
            .AddSingleton(_configuration)
            .AddSingleton<LavaRestClient>()
            .AddSingleton<LavaSocketClient>()


            /*.AddSingleton(new LavaRestClient(_configuration))
            .AddSingleton(new LavaSocketClient())*/


            .AddSingleton<MusicService>()
            .BuildServiceProvider();
    }
}
