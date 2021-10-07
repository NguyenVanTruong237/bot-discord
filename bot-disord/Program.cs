﻿using bot_disord.Services;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace bot_disord
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder()
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
                        LogLevel = LogSeverity.Debug,
                        AlwaysDownloadUsers = false,
                        MessageCacheSize = 200
                    };
                    config.Token = context.Configuration["token"];
                })
                .UseCommandService((context, config) =>
                {
                    config.CaseSensitiveCommands = false;
                    config.LogLevel = LogSeverity.Debug;
                    config.DefaultRunMode = RunMode.Sync;
                })
                .ConfigureServices((context, services) =>
                {
                    services
                    .AddHostedService<CommandHandler>();
                })
                .UseConsoleLifetime();
            var host = builder.Build();
            using(host)
            {
                await host.RunAsync();
            }
        }
    }
}
