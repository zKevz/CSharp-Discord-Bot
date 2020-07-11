using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class BJMain : InteractiveBase
	{
		[Command("bj",RunMode = RunMode.Async)]
		[Alias("blackjack", "bjack","blackj")]
		public async Task bj(int bet = -129)
		{
			if (bet == -129)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Syntax : \"c bj [bet]\"", "#CowboyBot")
					.WithFooter("Bot made by kevz#2073")
					.WithAuthor("Having a problem?")
					.WithColor(0, 255, 0)
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}

			else if (!lastBet(accName(Context.User.Id), 5) && Context.User.ToString() != "kevz#2073")
			{
				int ml = minutesLeft;
				int sl = secondsLeft;

				string minutes = ml + " minutes, ";

				if (ml == 0) minutes = "";
				sl -= (ml * 60);

				await ReplyAsync("You need to wait " + minutes + sl + " seconds " +
					"to bet again.");
				return;
			}

			else if (bet < 1)
			{
				await ReplyAndDeleteAsync("You can't bet lower than 1 coin", timeout: TimeSpan.FromSeconds(5));
				return;

			}

			else if (bet > 10)
			{
				await ReplyAndDeleteAsync("You can't bet more than 10 coins", timeout: TimeSpan.FromSeconds(5));
				return;

			}

			else if (bet > coins(accName(Context.User.Id)))
			{
				await ReplyAndDeleteAsync("You only have " +
					+coins(accName(Context.User.Id)) + " bro..", timeout: TimeSpan.FromSeconds(10));
				return;

			}

			else
			{
				Random r = new Random();

				int enemyNum = r.Next(2, 11);
				int yourNum = r.Next(2, 11);

				int round = 1;

				string path = File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json");

				while(enemyNum < 21 && yourNum < 21 && round <= 3)
				{
					if (enemyNum >= 21) break;
					if (yourNum >= 21) break;
					if (round > 3) break;

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("NUMBERS", "\u200bRound left : " + (4 - round) + "\nYour number : " + yourNum + "" +
						"\nEnemy number : " + enemyNum + "\u200b")
						.AddField("Do you want to hit or stand? " +
						"\n[1] Hit\n[2] Stand\nYou have 20 seconds.", "\u200b")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s BJ Game")
						.WithColor(0, 255, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					string answer = "";
					try
					{
						answer = (await NextMessageAsync(timeout:TimeSpan.FromSeconds(20))).ToString();
					}
					catch
					{
						EmbedBuilder ee = new EmbedBuilder();
						ee.AddField("WHOOPS", "You lost " + bet + " coins because " +
							"you didn't answer!")
							.WithFooter("Bot made by kevz#2073")
							.WithAuthor(accName(Context.User.Id) + "'s BJ Game")
							.WithColor(0, 255, 0)
							.WithCurrentTimestamp();
						await ReplyAsync("", false, ee.Build());
						return;
					}

					if (answer.Contains("hit") || answer == "1")
					{
						yourNum += r.Next(2, 11);
						if(r.Next(0,2) == 0 || enemyNum < 10 && enemyNum < 16)
						{
							enemyNum += r.Next(2, 11);
						}
					}
					else if (answer.Contains("sta") || answer == "2")
					{
						if(r.Next(0,2) == 0 || enemyNum < 10 && enemyNum < 16)
						{
							enemyNum += r.Next(2, 11);
						}
					}
					round++;
				}

				if(enemyNum > yourNum && enemyNum <= 21)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU LOSE", "Your final number : " + yourNum + "\n" +
						"Your final enemy number : " + enemyNum)
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s BJ Game")
						.WithColor(255,0, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] -= bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (enemyNum > yourNum && enemyNum > 21)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU WIN", "Your final number : " + yourNum + "\n" +
						"Your final enemy number : " + enemyNum)
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s BJ Game")
						.WithColor(0, 255, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] += bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (enemyNum < yourNum && yourNum <= 21)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU WIN", "Your final number : " + yourNum + "\n" +
						"Your final enemy number : " + enemyNum)
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s Casino Game")
						.WithColor(0, 255, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] += bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (enemyNum <yourNum && yourNum > 21)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU LOSE", "Your final number : " + yourNum + "\n" +
						"Your final enemy number : " + enemyNum)
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s BJ Game")
						.WithColor(255,0, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] -= bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (enemyNum == yourNum)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("TIE", "Your final number : " + yourNum + "\n" +
						"Your final enemy number : " + enemyNum)
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s BJ Game")
						.WithColor(255, 255, 255)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
				}
			}
		}
	}
}
