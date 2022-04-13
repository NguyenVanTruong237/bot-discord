using Discord.Interactions;
using System.Threading.Tasks;

namespace bot_disord.Modules
{
    public class SlashCommand : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("echo", "Echo an input")]
        public async Task Echo(string input)
        {
            await RespondAsync(input);
        }
    }
}
