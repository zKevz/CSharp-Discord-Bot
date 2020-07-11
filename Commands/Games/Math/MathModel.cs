using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using static CowboyBot.Timer;
using Discord;

namespace CowboyBot
{
	public class MathModel: InteractiveBase
	{
		[Command("mathgames", RunMode = RunMode.Async)]
		[Alias("mathgame","mathquiz","quizmath","gamemath","gamesmath")]
		public async Task MathGames()
		{
			string username = accName(Context.User.Id);

			if (!lastMath(username, 20))
			{
				await SendEmbed("You need to wait before doing math " +
					"games again! [COOLDOWN IS 20 MINUTES]", "WHOOPS");
				return;
			}
			else
			{
				Random r = new Random();
				int round = 1;
				int prize = 5;

				while (true) 
				{
					int firstNum = r.Next(50, 101), secondNum =
						r.Next(50, 101), sum = firstNum + secondNum;

					await SendEmbed("What is " + firstNum +
						" + " + secondNum + " ?", "ROUND " + round);

					string answer = "";
					try
					{
						answer = (await NextMessageAsync(true, true, TimeSpan.FromSeconds(5))).ToString();
					}
					catch
					{
						int fixedPrice = round * prize;
						if (round == 1)
						{

							await SendEmbed("You didn't answer!\nYou didn't get anything.", "WHOOPS");
							break;
							return;
						}
						else
						{
							await SendEmbed("You didn't answer!\n" + 
								"ROUND : " + round + "\n" +
								"You got " + fixedPrice + " coins!", "WHOOPS");
							dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
								@"database\account\" + username + ".json"));
							j["coins"] += fixedPrice;
							string output = JsonConvert.SerializeObject(j, Formatting.Indented);
							File.WriteAllText(@"database\account\" + username + "" +
								".json", output);
							break;
						}
					}
					if(answer == sum.ToString())
					{
						round++;
						await SendEmbed("CORRECT! Next Round!","ROUND " + round);
					}
					else
					{
						int fixedPrice = round * prize;
						if (round == 1)
						{

							await SendEmbed("WRONG! The answer was " + sum +"\nYou didn't get anything.", "WHOOPS");
							break;
						}
						else
						{
							await SendEmbed("The answer was " + sum +"\n" +
								"ROUND : " + round + "\n" +
								"You got " + fixedPrice + " coins!", "WHOOPS");
							dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
								@"database\account\" + username + ".json"));
							j["coins"] += fixedPrice;
							string output = JsonConvert.SerializeObject(j, Formatting.Indented);
							File.WriteAllText(@"database\account\" + username + "" +
								".json", output);
							break;
						}
					}
				}
			}
		}

		public async Task SendEmbed(string text,string title)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, text)
				.WithAuthor("MATH GAMES")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}
	}
}
