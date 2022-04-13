using bot_disord.Common;
using bot_disord.Utilities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bot_disord.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await Context.Channel.SendFileAsync("D:\\test.png");


            await ReplyAsync("Pong");
        }

        [Command("info")]
        [Summary("Xem info của bạn và người khác, !info @name .")]
        public async Task Info(SocketGuildUser socketGuidUser = null)
        {
            if (socketGuidUser == null)
            {
                socketGuidUser = Context.User as SocketGuildUser;
            }
            var embed = new MyBotEmbedBuilder()
                .WithTitle("Info của bạn nè:")
                .AddField("Id", socketGuidUser.Id, true)
                .AddField("Tên nè",$"{socketGuidUser.Username}#{socketGuidUser.Discriminator}", true)
                .AddField("Ngày tạo acc", socketGuidUser.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Ngày vào sever", socketGuidUser.JoinedAt.Value.ToString("dd/MM/yyyy"), true)
                .AddField("Role", string.Join(" ",socketGuidUser.Roles.Select(x => x.Mention)))
                .WithThumbnailUrl(socketGuidUser.GetAvatarUrl() ?? socketGuidUser.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }
        
        [Command("Sever")]
        [Summary("Xem thông tin sever")]
        public async Task Sever()
        {
            var builder = new MyBotEmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithDescription("Thông tin sever")
                .WithTitle($" Sever: {Context.Guild.Name}")
                .AddField("Ngày tạo", Context.Guild.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Tổng mem", (Context.Guild as SocketGuild).MemberCount, true)
                .AddField("Mem online", (Context.Guild as SocketGuild)
                .Users.Where(x => x.Status != UserStatus.Offline).Count(), true);

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }

        [Command("meme")]
        [Summary("Meme trên reddit")]
        public async Task Meme()
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync("https://reddit.com/r/memes/random.json?Limit=1");
            JArray arr = JArray.Parse(result);
            JObject post = JObject.Parse(arr[0]["data"]["children"][0]["data"].ToString()); //lấy 1 json data chuyển về string

            var builder = new MyBotEmbedBuilder()
                .WithImageUrl(post["url"].ToString())
                .WithTitle(post["title"].ToString())
                .WithUrl("https://reddit.com" + post["permalink"].ToString())
                .WithFooter($"🗨{post["num_comments"]} ⬆️ {post["up"]}");

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);

        }

        [Command("bit")]
        public async Task BitCoin()
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync("https://api.blockchain.com/v3/exchange/l2/BTC-USD");
            var objects = JObject.Parse(result);

            var post = objects["bids"][0];

            var builder = new MyBotEmbedBuilder()
                .WithTitle("BTC-USD")
                .AddField("$: ", post["px"].ToString())
                .AddField("BTC: ", post["num"].ToString());

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
    }
}
