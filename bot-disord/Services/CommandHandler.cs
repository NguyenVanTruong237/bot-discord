using bot_disord.Utilities;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Infrastructure.ApplicationDbContext;

namespace bot_disord.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;
        private readonly Severs _severs;
        private readonly Images _images;

        public CommandHandler(IServiceProvider provider
            , DiscordSocketClient client
            , CommandService service
            , IConfiguration config
            ,Severs severs
            ,Images images)
        {
            _provider = provider;
            _config = config;
            _client = client;
            _service = service;
            _severs = severs;
            _images = images;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            //_client.ChannelCreated += OnChannelCreated;
            _client.UserJoined += OnUserJoined;


            //_client.ReactionAdded += OnReactionAdded;       

            _service.CommandExecuted += OnCommandExecuted;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private async Task OnUserJoined(SocketGuildUser arg)
        {
            var newTask = new Task(async () => await HandleUserJoined(arg));
            newTask.Start();
        }

        private async Task HandleUserJoined(SocketGuildUser arg)
        {
            var channelId = await _severs.GetWelcomeAsync(arg.Guild.Id);
            if (channelId == 0)
            {
                return;
            }

            var channel = arg.Guild.GetTextChannel(channelId);
            if (channel == null)
            {
                await _severs.ClearWelcomeAsync(arg.Guild.Id);
                return;
            }
            var background = await _severs.GetBackgroundAsync(arg.Guild.Id);
            var path = await _images.CreateImageAsync(arg, background);

            await channel.SendFileAsync(path, null);
            File.Delete(path);

        }

        //private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        //{
        //    if (arg3.MessageId != 895866399568842832) return;
        //    if (arg3.Emote.Name != "💩") return;

        //    var role = (arg2 as SocketGuildChannel).Guild.Roles.FirstOrDefault(c => c.Id == 895652719241625661);
        //    await (arg3.User.Value as SocketGuildUser).AddRoleAsync(role);
        //}

        private async Task OnJoinedGuid(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync("Thanks for using.");
        }

        //private async Task OnChannelCreated(SocketChannel arg)  //event tạo channel text
        //{
        //    if ((arg as ITextChannel) == null) return;

        //    var channel = arg as ITextChannel;
        //    await channel.SendMessageAsync("The event test called");

        //}

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
