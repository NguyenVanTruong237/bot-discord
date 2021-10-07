using bot_disord.Common;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_disord.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("p")] //command ghi tắt
        [RequireUserPermission(GuildPermission.Administrator)] //chỉ admin mới gọi command này đc
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
            await Context.User.SendMessageAsync("Private Message!"); // gửi tin nhắn private cho user
        }

        [Command("info")]
        public async Task Info(SocketGuildUser socketGuidUser = null)
        {
            if (socketGuidUser == null)
            {
                socketGuidUser = Context.User as SocketGuildUser;
            }
            var embed = new MyBotEmbedBuilder()
                .WithTitle("Info của mày nè:")
                .AddField("Id nè", socketGuidUser.Id, true)
                .AddField("Tên nè",$"{socketGuidUser.Username}#{socketGuidUser.Discriminator}", true)
                .AddField("Ngày tạo acc", socketGuidUser.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Ngày vào sever", socketGuidUser.JoinedAt.Value.ToString("dd/MM/yyyy"), true)
                .AddField("Role", string.Join(" ",socketGuidUser.Roles.Select(x => x.Mention)))
                .WithThumbnailUrl(socketGuidUser.GetAvatarUrl() ?? socketGuidUser.GetDefaultAvatarUrl())
                .WithCurrentTimestamp()
                .Build();

            await ReplyAsync(embed: embed);
        }
        [Command("xoa")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge (int amount)
        {

            var messages = await Context.Channel.GetMessagesAsync(amount+1).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            var message = await Context.Channel.SendMessageAsync($" Đã xóa {messages.Count()} tin nhắn!");
            await Task.Delay(2500);
            await message.DeleteAsync();
        }
    }
}
