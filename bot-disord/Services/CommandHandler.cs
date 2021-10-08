using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bot_disord.Services
{
    public class CommandHandler : InitializedService
    {
        public static IServiceProvider _provider;
        public static DiscordSocketClient _client;
        public static CommandService _service;
        public static IConfiguration _config;

        public CommandHandler(IServiceProvider provider
            , DiscordSocketClient client
            , CommandService service
            , IConfiguration config)
        {
            _provider = provider;
            _config = config;
            _client = client;
            _service = service;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _client.ChannelCreated += OnChannelCreated;
            _client.JoinedGuild += OnJoinedGuid;            // event bot discord join sever
            _client.ReactionAdded += OnReactionAdded;       //event react message to get role

            _service.CommandExecuted += OnCommandExecuted;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg3.MessageId != 895866399568842832) return;
            if (arg3.Emote.Name != "💩") return;

            var role = (arg2 as SocketGuildChannel).Guild.Roles.FirstOrDefault(c => c.Id == 895652719241625661);
            await (arg3.User.Value as SocketGuildUser).AddRoleAsync(role);
        }

        private async Task OnJoinedGuid(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync("Thanks for using.");
        }

        private async Task OnChannelCreated(SocketChannel arg)  //event tạo channel text
        {
            if ((arg as ITextChannel) == null) return;

            var channel = arg as ITextChannel;
            await channel.SendMessageAsync("The event test called");

        }

        private async Task OnCommandExecuted(Optional<CommandInfo> commandInfo  //thực thi command
            ,ICommandContext commandContext,
            IResult result)
        {
            if (result.IsSuccess)
            {
                return;
            }
            await commandContext.Channel.SendMessageAsync(result.ErrorReason);
        }
        private async Task OnMessageReceived(SocketMessage socketMessage)   // nhận message từ user
        {
            if (!(socketMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            if (!message.HasStringPrefix(_config["prefix"], ref argPos)
                && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);
        }
    }
}
