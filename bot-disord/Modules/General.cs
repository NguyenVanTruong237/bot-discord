using bot_disord.Common;
using bot_disord.Utilities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_disord.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private readonly Images _images;

        public General(Images images)
        {
            _images = images;
        }

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


        [Command("image", RunMode =RunMode.Async)]
        public async Task Image(SocketGuildUser user)
        {
            var path = await _images.CreateImageAsync(user);
            await Context.Channel.SendFileAsync(path);
            File.Delete(path);
        }

    }
}
