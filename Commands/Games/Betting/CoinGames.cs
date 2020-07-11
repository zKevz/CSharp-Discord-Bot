using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using static CowboyBot.Timer;
using Newtonsoft.Json;
using System.IO;

namespace CowboyBot
{
	public class CoinGames : InteractiveBase
	{
		[Command("playcoin",RunMode = RunMode.Async)]
		public async Task PlayCoin(int bet)
		{
			string username = accName(Context.User.Id);
			if (!lastBet(username, 10))
			{
				await SendEmbed("WHOOPS",
					"Chill out buddy! The cooldown is 10 minutes.");
				return;
			}
			else if (coins(username) < bet)
			{
				await SendEmbed("WHOOPS", "You don't have enough" +
					" coins!");
				return;
			}
			else if (bet > 5 || bet < 2)
			{
				await SendEmbed("WHOOPS",
					"You only can bet 2 to 5 coins.");
				return;
			}
			else
			{
				await SendEmbed("Heads Or Tails?",
					"**Choose!**\n" +
					"[1] Heads\n" +
					"[2] Tails\n" +
					"\nChoose !");
				try
				{
					Random r = new Random();
					int a = r.Next(0, 2);

					string answer = (await NextMessageAsync(true,
						true, TimeSpan.FromSeconds(30))).ToString();
					if (answer.ToLower().Contains("head") || answer == "1")
					{
						if (a == 0)
						{
							await SendEmbed("You win!",
								"The coins was flipped and landed on heads!");
							dynamic j = JsonConvert.DeserializeObject(
								File.ReadAllText(@"database\account\" + username +
								".json"));
							j["coins"] += bet;
							string output = JsonConvert.SerializeObject(
								j, Formatting.Indented);
							File.WriteAllText(@"database\account\" +
								username + ".json", output);
							return;
						}
						else
						{
							await SendEmbed("You lost!",
								"The coins was flipped and landed on tails!");

							dynamic j = JsonConvert.DeserializeObject(
								File.ReadAllText(@"database\account\" + username +
								".json"));
							j["coins"] -= bet;
							string output = JsonConvert.SerializeObject(
								j, Formatting.Indented);
							File.WriteAllText(@"database\account\" +
								username + ".json", output);
							return;
						}
					}
					else if (answer.ToLower().Contains("tail") || answer == "2")
					{
						if (a == 1)
						{
							await SendEmbed("You win!",
								"The coins was flipped and landed on tails!");
							dynamic j = JsonConvert.DeserializeObject(
								File.ReadAllText(@"database\account\" + username +
								".json"));
							j["coins"] += bet;
							string output = JsonConvert.SerializeObject(
								j, Formatting.Indented);
							File.WriteAllText(@"database\account\" +
								username + ".json", output);
							return;
						}
						else
						{
							await SendEmbed("You lost!",
								"The coins was flipped and landed on heads!");

							dynamic j = JsonConvert.DeserializeObject(
								File.ReadAllText(@"database\account\" + username +
								".json"));
							j["coins"] -= bet;
							string output = JsonConvert.SerializeObject(
								j, Formatting.Indented);
							File.WriteAllText(@"database\account\" +
								username + ".json", output);
							return;
						}
					}
					else
					{
						await SendEmbed("WHOOPS",
						"You lose you didn't answer the correct format!");
						dynamic j = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\account\" + username +
							".json"));
						j["coins"] -= bet;
						string output = JsonConvert.SerializeObject(
							j, Formatting.Indented);
						File.WriteAllText(@"database\account\" +
							username + ".json", output);
						return;
					}
				}
				catch
				{
					await SendEmbed("WHOOPS",
						"You lose your coins because you didn't answer!");
					dynamic j = JsonConvert.DeserializeObject(
						File.ReadAllText(@"database\account\" + username +
						".json"));
					j["coins"] -= bet;
					string output = JsonConvert.SerializeObject(
						j, Formatting.Indented);
					File.WriteAllText(@"database\account\" +
						username + ".json", output);
					return;
				}
			}
		}

		public async Task SendEmbed(string title, string description)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, description)
				.WithAuthor("Coins Games")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}
	}
}
