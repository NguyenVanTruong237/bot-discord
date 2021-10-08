using Discord;
using Discord.Commands;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_disord.Modules
{
    public class Configuration : ModuleBase<SocketCommandContext>
    {
        private readonly Severs _servers;

        public Configuration(Severs severs)
        {

            _servers = severs;
        }

        [Command("welcome")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Welcome(string option = null, string value = null)
        {
            if (option ==null && value == null)
            {
                var fetchChannelId = await _servers.GetWelcomeAsync(Context.Guild.Id);
                if (fetchChannelId == 0 || fetchChannelId.ToString() == null)
                {
                    await ReplyAsync("Không có channel để set");
                    return;
                }

                var fetchedChannel = Context.Guild.GetTextChannel(fetchChannelId);
                if (fetchedChannel == null)
                {
                    await ReplyAsync("Không có channel để set");
                    await _servers.ClearWelcomeAsync(Context.Guild.Id);
                }

                var fetchBackGround = await _servers.GetBackgroundAsync(Context.Guild.Id);

                if (fetchBackGround != null)
                {
                    await ReplyAsync($"Channel sử dụng để welcome member là {fetchedChannel.Mention}.\n Background set là {fetchBackGround}.");
                }
                else
                {
                    await ReplyAsync($"Channel sử dụng để Welcome member là {fetchedChannel.Mention}.");
                }
                return;
            }

            if (option == "channel" && value != null)
            {
                if (!MentionUtils.TryParseChannel(value, out ulong parsedId))
                {
                    await ReplyAsync("vui lòng điền channel vào!");
                    return;
                }

                var parsedChannel = Context.Guild.GetTextChannel(parsedId);
                if (parsedChannel == null)
                {
                    await ReplyAsync("vui lòng điền channel vào!");
                    return;
                }

                await _servers.ModifyWelcomeAsync(Context.Guild.Id, parsedId);
                await ReplyAsync($"Set channel thành để welcome thành công {parsedChannel.Mention}.");
                return;
            }

            if (option == "background" && value != null)
            {
                if (value == "clear")
                {
                    await _servers.ClearBackgroundAsync(Context.Guild.Id);
                    await ReplyAsync("Background xóa thành công.");
                    return;
                }

                await _servers.ModifyBackgroundAsync(Context.Guild.Id, value);
                await ReplyAsync($"Set background thành công: {value}.");
                return;
            }

            if (option == "clear" && value == null)
            {
                await _servers.ClearWelcomeAsync(Context.Guild.Id);
                await ReplyAsync("Xóa channel set thành công!");
                return;
            }

            await ReplyAsync("Sai command, nhập thêm # channel cần set vào");
        }
    }
}
