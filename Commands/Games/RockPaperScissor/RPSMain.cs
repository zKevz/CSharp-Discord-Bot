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
using System.Linq;
using System.Collections.Generic;

namespace CowboyBot
{
	public class RPSMain : InteractiveBase
	{
		private static ColorList c = new ColorList();
		private readonly int r = c.r;
		private readonly int g = c.g;
		private readonly int b = c.b;

		[Command("playrps", RunMode = RunMode.Async)]
		public async Task playrps(IGuildUser user)
		{
			if (!isOnAccount(user.Id))
			{
				await ReplyAsync("That user doesn't have cowboy " +
					"account! Perhaps, ask him?");
				return;
			}
			else if (user == Context.User) { await ReplyAndDeleteAsync("You can't trade yourself!", timeout: TimeSpan.FromSeconds(10)); }

			string isAccept = "";
			try
			{
				await user.SendMessageAsync("User " + Context.User + " asked you to play **Rock " +
					"Paper Scissor!** Type anything if you want to accept. If you don't" +
					" just ignore this messages for 30 seconds.");
				isAccept = (await NextMessageAsync(new EnsureFromUserCriterion(user), TimeSpan.FromSeconds(30))).ToString();
			}
			catch
			{
				await Context.User.SendMessageAsync("The user didn't accept your request.");
			}

			EmbedBuilder e = new EmbedBuilder();
			e.AddField("Choose!", "\u200b")
				.WithAuthor("Rock Paper Scissors")
				.AddField("[1] Rock\n[2] Paper\n[3] Scissors", "You can choose by number or type.")
				.WithColor(r, g, b)
				.WithCurrentTimestamp();

			EmbedBuilder ee = new EmbedBuilder();
			ee.AddField("Choose!", "\u200b")
				.AddField("[1] Rock\n[2] Paper\n[3] Scissors", "You can choose by number or type.")
				.WithAuthor("It's your turn now!")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp();

			await Context.User.SendMessageAsync("", false, e.Build());
			await user.SendMessageAsync("Wait for your opponent to choose...");

			EnsureFromUserCriterion you = new EnsureFromUserCriterion(Context.User);
			EnsureFromUserCriterion enemy = new EnsureFromUserCriterion(user);

			string yourChoose = (await NextMessageAsync(you)).ToString();

			string[] rps = { "rock", "paper", "scissor" };
			string fixedChoose = "";
			string fixedChooseEnemy = "";

			while (true)
			{
				if (yourChoose == "1" || yourChoose.Contains("rock")) { fixedChoose = rps[0]; break; }
				else if (yourChoose == "2" || yourChoose.Contains("paper")) { fixedChoose = rps[1]; break; }
				else if (yourChoose == "3" || yourChoose.Contains("sciss")) { fixedChoose = rps[2]; break; }
				else
				{
					await ReplyAsync("Invalid Type! Please choose again.");
					yourChoose = (await NextMessageAsync(you)).ToString();
				}
			}

			await user.SendMessageAsync("", false, ee.Build());
			await Context.User.SendMessageAsync("Waiting for your opponent to choose...");

			string enemyChoose = (await NextMessageAsync(enemy)).ToString();
			while (true)
			{
				if (enemyChoose == "1" || enemyChoose.Contains("rock")) { fixedChooseEnemy = rps[0]; break; }
				else if (enemyChoose == "2" || enemyChoose.Contains("paper")){  fixedChooseEnemy = rps[1];break; }
				else if (enemyChoose == "3" || enemyChoose.Contains("sciss")) {fixedChooseEnemy = rps[2];break; }
				else
				{
					await ReplyAsync("Invalid Type! Please choose again");
					enemyChoose = (await NextMessageAsync(enemy)).ToString();
				}
			}

			if(fixedChoose == fixedChooseEnemy)
			{
				EmbedBuilder eee = new EmbedBuilder();
				eee.AddField("Tie!", "You choose " + fixedChoose + " and your opponent" +
					" choose " + fixedChooseEnemy)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();
				await user.SendMessageAsync("", false, eee.Build());
				await Context.User.SendMessageAsync("", false, eee.Build());
			}
			else if (fixedChoose == "rock" && fixedChooseEnemy == "paper")
			{
				EmbedBuilder eee = new EmbedBuilder();
				eee.AddField("You lose!", "You choose " + fixedChoose + " and your opponent" +
					" choose " + fixedChooseEnemy)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				EmbedBuilder eeee = new EmbedBuilder();
				eeee.AddField("You Win!", "You choose " + fixedChooseEnemy + " and your opponent" +
					" choose " + fixedChoose)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				await user.SendMessageAsync("", false, eeee.Build());
				await Context.User.SendMessageAsync("", false, eee.Build());
			}

			else if (fixedChoose == "scissor" && fixedChooseEnemy == "paper")
			{
				EmbedBuilder eee = new EmbedBuilder();
				eee.AddField("You lose!", "You choose " + fixedChooseEnemy + " and your opponent" +
					" choose " + fixedChoose)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				EmbedBuilder eeee = new EmbedBuilder();
				eeee.AddField("You Win!", "You choose " + fixedChoose + " and your opponent" +
					" choose " + fixedChooseEnemy)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				await user.SendMessageAsync("", false, eee.Build());
				await Context.User.SendMessageAsync("", false, eeee.Build());
			}

			else if (fixedChoose == "rock" && fixedChooseEnemy == "scissor")
			{
				EmbedBuilder eee = new EmbedBuilder();
				eee.AddField("You lose!", "You choose " + fixedChooseEnemy + " and your opponent" +
					" choose " + fixedChoose)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				EmbedBuilder eeee = new EmbedBuilder();
				eeee.AddField("You Win!", "You choose " + fixedChoose + " and your opponent" +
					" choose " + fixedChooseEnemy)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				await user.SendMessageAsync("", false, eee.Build());
				await Context.User.SendMessageAsync("", false, eeee.Build());
			}

			else if (fixedChoose == "paper" && fixedChooseEnemy == "scissor")
			{
				EmbedBuilder eee = new EmbedBuilder();
				eee.AddField("You lose!", "You choose " + fixedChoose + " and your opponent" +
					" choose " + fixedChooseEnemy)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				EmbedBuilder eeee = new EmbedBuilder();
				eeee.AddField("You Win!", "You choose " + fixedChooseEnemy + " and your opponent" +
					" choose " + fixedChoose)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				await user.SendMessageAsync("", false, eeee.Build());
				await Context.User.SendMessageAsync("", false, eee.Build());
			}

			else if (fixedChoose == "paper" && fixedChooseEnemy == "rock")
			{
				EmbedBuilder eee = new EmbedBuilder();
				eee.AddField("You lose!", "You choose " + fixedChooseEnemy + " and your opponent" +
					" choose " + fixedChoose)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				EmbedBuilder eeee = new EmbedBuilder();
				eeee.AddField("You Win!", "You choose " + fixedChoose + " and your opponent" +
					" choose " + fixedChooseEnemy)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				await user.SendMessageAsync("", false, eee.Build());
				await Context.User.SendMessageAsync("", false, eeee.Build());
			}

			else if (fixedChoose == "scissor" && fixedChooseEnemy == "rock")
			{
				EmbedBuilder eee = new EmbedBuilder();
				eee.AddField("You lose!", "You choose " + fixedChoose + " and your opponent" +
					" choose " + fixedChooseEnemy)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				EmbedBuilder eeee = new EmbedBuilder();
				eeee.AddField("You Win!", "You choose " + fixedChooseEnemy + " and your opponent" +
					" choose " + fixedChoose)
					.WithAuthor("Rock Paper Scissor")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();

				await user.SendMessageAsync("", false, eeee.Build());
				await Context.User.SendMessageAsync("", false, eee.Build());
			}
			else
			{
				Console.WriteLine("Invalid argument at " + fixedChoose + " and " + fixedChooseEnemy);
			}
		}
	}
}