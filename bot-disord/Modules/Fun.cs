using bot_disord.Common;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bot_disord.Modules
{
     public class Fun : ModuleBase<SocketCommandContext>
    {
        [Command("meme")]
        public async Task Meme ()
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
    }
}
