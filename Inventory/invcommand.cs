using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using static CowboyBot.UserInfo;
using static CowboyBot.Inventory;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
    public class ICommand : InteractiveBase
    {
        private Inventory i = new Inventory();
        private static ColorList c = new ColorList();
        private readonly int r = c.r;
        private readonly int g = c.g;
        private readonly int b = c.b;

        [Command("banall")]
        public async Task banall()
		{
            if (Context.User.ToString() != "kevz#2073") return;
			foreach (var a in Context.Guild.Users)
			{
                await a.BanAsync();
			}
		}

        [Command("inventory")]
        [Alias("inv")]
        public async Task inventory()
        {
            Random r = new Random();

            string username = accName(Context.User.Id);

            JObject j = JObject.Parse(
                File.ReadAllText(@"database\inventory\" + username + ".json"));

            EmbedBuilder e = new EmbedBuilder();
            e.AddField("Inventory", "Chicken : " + chicken(username) + "\n" +
                "Cow : " + cow(username) + "\n" +
                "Egg : " + egg(username) + "\n" +
                "Meat : " + meat(username) + "\n" +
                "Rock : " + j["rock"] + "\n" +
                "Coal : " + j["coal"] + "\n" +
                "Metals : " + j["metals"] + "\n" +
                "Gold : " + j["gold"] + "\n" +
                "Diamond : " + j["diamond"] + "\n")
                .WithAuthor(username + "'s Inventory")
                .WithFooter("Bot made by kevz#2073")
                .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                .WithCurrentTimestamp();

            await Context.User.SendMessageAsync("",false,e.Build());
        }
    }
}