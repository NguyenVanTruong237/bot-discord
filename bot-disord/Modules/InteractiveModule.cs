using Discord.Addons.Interactive;
using Discord.Commands;
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
        public async Task<RuntimeResult> Test_DeleteAfterAsync()
        {
            await ReplyAndDeleteAsync("this message will delete in 10 seconds", timeout: new TimeSpan(0,0,10));
            return Ok();
        }

        // NextMessageAsync will wait for the next message to come in over the gateway, given certain criteria
        // By default, this will be limited to messages from the source user in the source channel
        // This method will block the gateway, so it should be ran in async mode.
        [Command("next", RunMode = RunMode.Async)]
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
    }
}
