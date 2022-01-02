using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;

namespace NoiseMachine.Services
{
    public class MusicService
    {
        private readonly LavaRestClient _lavaRestClient;
        private readonly LavaSocketClient _lavaSocketClient;
        private readonly DiscordSocketClient _client;
        private readonly LogService _logService;
        private Configuration _configuration;

        public MusicService(LavaRestClient lavaRestClient, DiscordSocketClient client,
            LavaSocketClient lavaSocketClient, LogService logService, Configuration configuration)
        {
            _client = client;
            _lavaRestClient = lavaRestClient;
            _configuration = configuration;
            new LavaSocketClient();
            _lavaSocketClient = lavaSocketClient;
            _logService = logService;
        }

        public Task InitializeAsync()
        {
            _client.Ready += ClientReadyAsync;
            _lavaSocketClient.Log += LogAsync;
            _lavaSocketClient.OnTrackFinished += TrackFinished;
            return Task.CompletedTask;
        }

        public async Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel)
            => await _lavaSocketClient.ConnectAsync(voiceChannel, textChannel);

        public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
            => await _lavaSocketClient.DisconnectAsync(voiceChannel);

       
        private async Task LogAsync(LogMessage logMessage)
        {
            await _logService.LogAsync(logMessage);
        }
    }
}
