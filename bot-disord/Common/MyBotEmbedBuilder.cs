using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_disord.Common
{
    internal class MyBotEmbedBuilder : EmbedBuilder
    {
        public MyBotEmbedBuilder()
        {
            WithColor(new Color(216, 86, 62));
        }
    }
}
