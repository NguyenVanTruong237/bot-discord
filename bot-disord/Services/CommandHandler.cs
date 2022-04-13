using bot_disord.Utilities;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace bot_disord.Services
{
    public class CommandHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;
        private readonly Severs _severs;
        private readonly Images _images;
        private string previousMessage;

        public CommandHandler(IServiceProvider provider
            , DiscordSocketClient client
            , CommandService service
            , IConfiguration config
            ,Severs severs
            ,Images images
            , ILogger<CommandHandler> logger) : base(client, logger)
        {
            _provider = provider;
            _config = config;
            _client = client;
            _service = service;
            _severs = severs;
            _images = images;
        } 

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _service.CommandExecuted += OnCommandExecuted;

            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        
        private async Task OnCommandExecuted(Optional<CommandInfo> commandInfo  //check command đã success hay chưa
            ,ICommandContext commandContext,
            IResult result)
        {
            Logger.LogInformation("User {user} attempted to use command {command}", commandContext.User, commandInfo.Value.Name);

            if (!commandInfo.IsSpecified || result.IsSuccess)
                return;

            await commandContext.Channel.SendMessageAsync($"Error: {result}");
        }
        private async Task OnMessageReceived(SocketMessage socketMessage)   // Check nhận message từ user or bot
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
