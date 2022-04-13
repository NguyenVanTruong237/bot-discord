using bot_disord.Services;
using bot_disord.Utilities;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(x =>
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        x.AddConfiguration(configuration);
    })
    .ConfigureLogging(x =>
    {
        x.AddConsole();
        x.SetMinimumLevel(LogLevel.Debug);
    })
    .ConfigureDiscordHost((context, config) =>
    {
        config.SocketConfig = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildBans,
            UseInteractionSnowflakeDate = false,
            LogLevel = LogSeverity.Verbose,
            AlwaysDownloadUsers = true,
            MessageCacheSize = 200
        };

        config.Token = context.Configuration["Token"];
    })
    //Omit this if you don't use the command service
    .UseCommandService((context, config) =>
    {
        config.DefaultRunMode = RunMode.Async;
        config.LogLevel = LogSeverity.Debug;
        config.CaseSensitiveCommands = false;
    })
    .UseInteractionService((context, config) =>
    {
        config.LogLevel = LogSeverity.Info;
        config.UseCompiledLambda = true;
    })
    .ConfigureServices((context, services) =>
    {
        //Add any other services here
        services.AddHostedService<CommandHandler>();
        services.AddHostedService<InteractionHandler>();
        services.AddDbContext<ApplicationDbContext>();
        services.AddSingleton<Images>();
        services.AddSingleton<Severs>();
    })
    .UseConsoleLifetime().Build();


await host.RunAsync();