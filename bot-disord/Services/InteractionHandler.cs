using Discord;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace bot_disord.Services
{
    public class InteractionHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly InteractionService _interactionService;

        public InteractionHandler(DiscordSocketClient client,
            ILogger<DiscordClientService> logger,
            IServiceProvider provider,
            InteractionService interactionService) : base(client, logger)
        {
            _provider = provider;
            _interactionService = interactionService;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Process the InteractionCreated payloads to execute Interactions commands
            Client.InteractionCreated += HandleInteraction;

            // Process the command execution results 
            _interactionService.SlashCommandExecuted += SlashCommandExecuted;

            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
            await Client.WaitForReadyAsync(stoppingToken);

#if DEBUG
            await _interactionService.RegisterCommandsToGuildAsync(895386351418097734);
#else
            await _interactionService.RegisterCommandsGloballyAsync();
#endif
        }

        private async Task SlashCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await arg2.Interaction.RespondAsync($"Unmet Precondition: {arg3.ErrorReason}");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        await arg2.Interaction.RespondAsync("Unknown command");
                        break;
                    case InteractionCommandError.BadArgs:
                        await arg2.Interaction.RespondAsync("Invalid number or arguments");
                        break;
                    case InteractionCommandError.Exception:
                        await arg2.Interaction.RespondAsync($"Command exception: {arg3.ErrorReason}");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await arg2.Interaction.RespondAsync("Command could not be executed");
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(Client, arg);
                await _interactionService.ExecuteCommandAsync(ctx, _provider);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception occurred whilst attempting to handle interaction.");

                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    var msg = await arg.GetOriginalResponseAsync();
                    await msg.DeleteAsync();
                }

            }
        }
    }
}
