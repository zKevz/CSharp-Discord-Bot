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
	public class CSNMain : InteractiveBase
	{
		[Command("casino")]
		[Alias("csn")]
		public async Task casino(int s = -120)
		{
			if (s == -120)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Syntax : \"c casino [bet]\"", "#CowboyBot")
					.WithFooter("Bot made by kevz#2073")
					.WithAuthor("Having a problem?")
					.WithColor(0, 255, 0)
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}

			if (!lastBet(accName(Context.User.Id), 5))
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
			else if (s < 1)
			{
				await ReplyAndDeleteAsync("You can't bet lower than 1 coin", timeout: TimeSpan.FromSeconds(3));
				return;
			}

			else if(s > 10)
			{
				await ReplyAndDeleteAsync("You can't bet more than 10 coins", timeout: TimeSpan.FromSeconds(3));
				return;

			}

			else if(s > coins(accName(Context.User.Id)))
			{
				await ReplyAndDeleteAsync("You only have " +
					+coins(accName(Context.User.Id)) + " bro..", timeout: TimeSpan.FromSeconds(10));
				return;

			}

			else
			{
				Random r = new Random();
				string path = File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json");

				int yourNumber = r.Next(0, 37);
				int enemyNumber = r.Next(0, 37);

				if(yourNumber > enemyNumber && enemyNumber != 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU WIN", "You rolled the ball and got " + yourNumber + "!\n" +
						"Your enemy rolled the ball and got " + enemyNumber + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s Casino Game")
						.WithColor(0,255, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] += s;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber > enemyNumber && enemyNumber == 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU LOSE", "You rolled the ball and got " + yourNumber + "!\n" +
						"Your enemy rolled the ball and got " + enemyNumber + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s Casino Game")
						.WithColor(255, 0, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] -= s;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber < enemyNumber && yourNumber == 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU WIN", "You rolled the ball and got " + yourNumber + "!\n" +
						"Your enemy rolled the ball and got " + enemyNumber + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s Casino Game")
						.WithColor(0,255, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] += s;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber < enemyNumber && yourNumber != 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU LOSE", "You rolled the ball and got " + yourNumber + "!\n" +
						"Your enemy rolled the ball and got " + enemyNumber + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s Casino Game")
						.WithColor(255, 0, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] -= s;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber == enemyNumber)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("TIE", "You rolled the ball and got " + yourNumber + "!\n" +
						"Your enemy rolled the ball and got " + enemyNumber + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s Casino Game")
						.WithColor(255, 255,255)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

				}
			}
		}
	}
}
