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
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("xoa")]
        [RequireRole("test")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int amount)
        {

            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            var message = await Context.Channel.SendMessageAsync($" Đã xóa {messages.Count()} tin nhắn!");
            await Task.Delay(2500);
            await message.DeleteAsync();
        }
    }
}
