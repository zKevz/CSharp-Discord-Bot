using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class QQMain : InteractiveBase
	{
		[Command("qq")]
		public async Task qq(int bet = -129)
		{
			if (bet == -129)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Syntax : \"c qq [bet]\"", "#CowboyBot")
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

				int yourNum = r.Next(0, 37);
				int enemyNum = r.Next(0, 37);

				int yourNumber = 0;
				int enemyNumber = 0;

				if (mt2char(yourNum)) yourNumber = qqNum(yourNum);
				else yourNumber = yourNum;
				if (mt2char(enemyNum)) enemyNumber = qqNum(enemyNum);
				else enemyNumber = enemyNum;

				string path = File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json");

				if (yourNumber > enemyNumber && enemyNumber != 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU WIN", "You rolled the ball and got " + yourNum + "!\n" +
						"Your enemy rolled the ball and got " + enemyNum + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s QQ Game")
						.WithColor(0,255, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] += bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber > enemyNumber && enemyNumber == 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU LOSE", "You rolled the ball and got " + yourNum + "!\n" +
						"Your enemy rolled the ball and got " + enemyNum + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s QQ Game")
						.WithColor(255, 0, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] -= bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber < enemyNumber && yourNumber == 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU WIN", "You rolled the ball and got " + yourNum + "!\n" +
						"Your enemy rolled the ball and got " + enemyNum + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s QQ Game")
						.WithColor(0,255, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] += bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber < enemyNumber && yourNumber != 0)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("YOU LOSE", "You rolled the ball and got " + yourNum + "!\n" +
						"Your enemy rolled the ball and got " + enemyNum + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s QQ Game")
						.WithColor(255, 0, 0)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					dynamic j = JsonConvert.DeserializeObject(path);
					j["coins"] -= bet;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else if (yourNumber == enemyNumber)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("TIE", "You rolled the ball and got " + yourNum + "!\n" +
						"Your enemy rolled the ball and got " + enemyNum + "!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor(accName(Context.User.Id) + "'s QQ Game")
						.WithColor(255,255,255)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

				}
			}
		}
		public int qqNum(int s)
		{
			string a = s.ToString().Substring(1);
			return Convert.ToInt32(a);
		}

		public bool mt2char(int s)
		{
			if (s.ToString().Length > 1 && s.ToString().Length != 1) return true;
			else return false;
		}
	}
}
