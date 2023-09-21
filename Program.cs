using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
            };
            _client = new DiscordSocketClient(config);
            _client.Log += Log;

            DotNetEnv.Env.Load();
            var token = Environment.GetEnvironmentVariable("token");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _client.MessageUpdated += MessageUpdated;
            _client.MessageReceived += MessageReceived;

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine($"Uma mensagem foi editada! {message} -> {after}");
        }

        private async Task MessageReceived(SocketMessage msg)
        {
            if (msg.Author.Id == _client.CurrentUser.Id) return;

            var channel = _client.GetChannel(msg.Channel.Id) as IMessageChannel;
            await channel.SendMessageAsync("Para de falar ai seu doente");

            // var channel = msg.Channel as SocketGuildChannel;
            // Console.WriteLine($"[{msg.CreatedAt.DateTime.ToLongTimeString()}] {channel.Guild.Name}, no canal {channel.Name}\n{msg.Author} disse: {msg.Content}");
        }
    }
}
