using System;
using System.Collections.Generic;
using System.Text;
using Discord.Addons.Interactive;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using static CowboyBot.Timer;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CowboyBot
{
	public class Sprayer : InteractiveBase
	{
		[Command("boostharvest",RunMode = RunMode.Async)]
		public async Task BoostHarvest()
		{
			string username = accName(Context.User.Id);
			JObject xd = JObject.Parse(File.ReadAllText(
				@"database\account\" + username + ".json"));

			if ((int)xd["sprayer"] <= 0)
			{
				await ReplyAsync("You don't have enough sprayer!");
				return;
			}

			if (File.Exists(@"database\farming\" + username + ".json"))
			{
				if (!lastSpray(username, 30))
				{
					await ReplyAsync("Wait 30 minutes before " +
						"boosting a seed again!");
					return;
				}
				else
				{
					await ReplyAsync("Are you sure you want to boost your seed?\n" +
						"It will consumes you 1 `Cowboy's Sprayer`\nType " +
						"\"yes\" if you want to use!");

					string a = "";
					try
					{
						a = (await NextMessageAsync(true, true, TimeSpan.FromSeconds(30))).ToString();
					}
					catch
					{
						await ReplyAsync("You didn't answer!\n**BOOST CANCELLED**");
						return;
					}

					if (a.ToLower() == "yes" || a.ToLower() == "accept")
					{

						JObject jajaja = JObject.Parse(File.ReadAllText(
							@"database\farming\" +
							username + ".json"));

						sprayTarget.Add(username);
						sprayTimer.Add(DateTimeOffset.Now);

						dynamic j = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\farming\" +
							username + ".json"));
						j["time"] = ((DateTimeOffset)jajaja["time"]).AddMinutes(-30);
						string output = JsonConvert.SerializeObject(j,
							Formatting.Indented);
						File.WriteAllText(@"database\farming\" +
							username + ".json", output);

						dynamic jj = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\account\" +
							username + ".json"));
						jj["sprayer"] -= 1;
						string outputj = JsonConvert.SerializeObject(jj,
							Formatting.Indented);
						File.WriteAllText(@"database\account\" +
							username + ".json", outputj);

						await ReplyAsync("SEED BOOSTED BY 30 MINUTES!");
					}
					else
					{
						await ReplyAsync("You cancelled the boost.");
						return;
					}
				}
			}
			else
			{
				await ReplyAsync("You don't even plant a " +
					"seed!");
			}
		}
	}
}
