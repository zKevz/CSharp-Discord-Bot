using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;
using static CowboyBot.unlockedPets;

namespace CowboyBot
{
    public class pShop : InteractiveBase
    {
        private static ColorList c = new ColorList();
        private readonly int r = c.r;
        private readonly int g = c.g;
        private readonly int b = c.b;

        [Command("petinfo")]
        public async Task petshop()
        {
            var user = accName(Context.User.Id);

            EmbedBuilder e = new EmbedBuilder();
            e.AddField("PET SHOP","\n\n" +
            "\u200b")
            .AddField("Your default pet : Cat","This pet is given to you by default.")
            .AddField(petList(2,user), petPrice(2,user))
            .AddField(petList(3,user),petPrice(3,user))
            .AddField(petList(4,user),petPrice(4,user))
            .AddField(petList(5,user), petPrice(5,user))
            .WithAuthor("Cowboy's Pet Shop")
            .WithColor(r,g,b)
            .WithFooter("Bot made by kevz#2073")
            .WithCurrentTimestamp();
            await Context.User.SendMessageAsync("",false,e.Build());
        }
    }
}