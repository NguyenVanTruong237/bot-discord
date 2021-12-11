using bot_disord.Common;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_disord.Modules
{
    public class InteractiveModule : InteractiveBase
    {
        private readonly CommandService _service;

        public InteractiveModule(CommandService service)
        {
            _service = service;
        }

        // DeleteAfterAsync will send a message and asynchronously delete it after the timeout has popped
        // This method will not block.
        [Command("delete")]
        [RequireRole("Admin")]
        public async Task<RuntimeResult> Test_DeleteAfterAsync()
        {
            await ReplyAndDeleteAsync("this message will delete in 10 seconds", timeout: new TimeSpan(0,0,10));
            return Ok();
        }

        // NextMessageAsync will wait for the next message to come in over the gateway, given certain criteria
        // By default, this will be limited to messages from the source user in the source channel
        // This method will block the gateway, so it should be ran in async mode.
        [Command("next", RunMode = RunMode.Async)]
        [RequireRole("test")]
        public async Task Test_NextMessageAsync()
        {
            await ReplyAsync("What is 2+2?");
            var response = await NextMessageAsync();
            if (response != null)
                await ReplyAsync($"You replied: {response.Content}");
            else
                await ReplyAsync("You did not reply before the timeout");
        }

        // PagedReplyAsync will send a paginated message to the channel
        // You can customize the paginator by creating a PaginatedMessage object
        // You can customize the criteria for the paginator as well, which defaults to restricting to the source user
        // This method will not block.
        [Command("paginator")]
        [Summary("test")]
        [RequireRole("test")]
        public async Task Test_Paginator()
        {
            var pages = new[] { "Page 1", "Page 2", "Page 3", "aaaaaa", "Page 5" };
            await PagedReplyAsync(pages);
        }

        [Command("help")]
        public async Task Help()
        {
            List<string> Pages = new List<string>();

                var modules = _service.Modules.FirstOrDefault(c => c.Name == "General");
                string page = "1 xíu đồ chơi sương sương 😆.\n";

                foreach (var command in modules.Commands )
                {
                    page += $"`!{command.Name}` - {command.Summary ?? "empty"}\n";
                }
                Pages.Add(page);

            await PagedReplyAsync(Pages);
        }

        [Command("guild")]
       // [RequireRole("Admin")]
        public async Task GuildEvent()
        {
            var builder = new MyBotEmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithDescription("🍀 The guild only for member in alpha test 🍀" +
                "\n🌈 Chat the name of guild you want join 🌈\n" +
                "💫 The mod will verify and add you in your guild selected 💫")
                .WithTitle($"🎉 WELCOME TO GUILD IN FARM ME 🎉").WithUrl("https://farmme.io/")
                .AddField("GUILD LIST", "⬇️⬇️⬇️⬇️")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🍑 peaches-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🌽 corn-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🥔 potato-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🍇 grape-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🍓 strawberry-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🍅 persimmon-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🍌 banana-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🥕 carrot-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🍊 orange-guild")
                .AddField("⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤", "🍋 lemon-guild"+ "\n ⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤⏤")
                .AddField("🎯 Explore more about our social channels 🎯",
                "[📌 Fanpage](https://www.facebook.com/FarmMeOfficial)\n" +
                "[📌 Twitter](https://twitter.com/FarmMeOfficiall)\n" +
                "[📌 Youtube](https://www.youtube.com/c/FarmMeOfficial)\n" +
                "[📌 Telegram Global Group](https://t.me/farmmeOFFICIALGlobal)\n" +
                "[📌 Medium](https://farmme.medium.com/)\n")
                .WithColor(new Color(0, 255, 136));
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}
