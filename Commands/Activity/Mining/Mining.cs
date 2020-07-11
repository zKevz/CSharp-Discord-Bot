using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using static CowboyBot.Timer;
using Newtonsoft.Json;
using System.IO;

namespace CowboyBot
{
	public class Mining : InteractiveBase
	{
		[Command("mining")]
		public async Task MiningCommand()
		{
			string username = accName(Context.User.Id);
			if (!lastMining(username,20) && Context.User.ToString() != "kevz#2073")
			{
				await SendEmbed("WHOOPS", "Slowdown a bit! " +
					"Cooldown is 20 minutes.");
				return;
			}
			else
			{
				Random r = new Random();
				int randint = r.Next(0, 6);

				int rockint = 0;
				int coalint = 0;
				int metalsint = 0;
				int goldint = 0;
				int diamondint = 0;

				string rock = "";
				string coal = "";
				string metals = "";
				string gold = "";
				string diamond = "";
				string result = "You got ";

				if(randint < 4)
				{
					rockint = r.Next(10, 31);
					rock += rockint + " rocks";
				}
				randint = r.Next(0, 51);
				if(randint < 10)
				{
					coalint = r.Next(10, 31);
					coal += ", " + coalint + " coals";
				}
				randint = r.Next(0, 151);
				if(randint > 135)
				{
					metalsint += r.Next(10, 31);
					metals += ", " +  metalsint + " metals";
				}
				randint = r.Next(0, 501);
				if(randint > 490)
				{
					goldint += r.Next(10, 31);
					gold += ", " + goldint + " golds";
				}
				randint = r.Next(0, 1001);
				if(randint > 990)
				{
					diamondint = r.Next(10, 31);
					diamond += " and " + diamondint + " diamonds";
				}
				result += rock + coal + metals + gold + diamond;
				if (result == "You got ") result += "nothing!\nSAD..";
				await SendEmbed("Mining Result", result + "!");

				dynamic j = JsonConvert.DeserializeObject(
					File.ReadAllText(@"database\inventory\" + username +
					".json"));
				j["rock"] += rockint;
				j["coal"] += coalint;
				j["metals"] += metalsint;
				j["gold"] += goldint;
				j["diamond"] += diamondint;
				string output = JsonConvert.SerializeObject(
					j, Formatting.Indented);
				File.WriteAllText(@"database\inventory\" + username +
					".json",output);
			}
		}

		public async Task SendEmbed(string title,string description)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, description)
				.WithAuthor("Cowboy's Mine")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}
	}
}
