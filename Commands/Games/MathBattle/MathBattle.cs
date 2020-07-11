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
using System.Collections.Generic;

namespace CowboyBot
{
	public class MathBattle : InteractiveBase
	{
		public static List<string> UserJoined = new List<string>();
		[Command("mathbattle",RunMode = RunMode.Async)]
		public async Task MathBattleCommand(IGuildUser user)
		{
			if (!isOnAccount(user.Id))
			{
				await SendEmbed("WHOOPS", "That user doesn't even have a " +
					"cowboy account!");
				return;
			}
			else if (UserJoined.Contains(accName(user.Id)))
			{
				await SendEmbed("WHOOPS", "That user is battling someone right" +
					" now! Please wait");
				return;
			}
			else
			{
				await ReplyAsync("Sending request...");
				await SendEmbed("Confirmation","User " + Context.User  
					+ " asked you to Math Battle! Type \"accept\" if you accept!",
					user);
				try
				{
					EnsureFromUserCriterion e = new EnsureFromUserCriterion(user);
					string answer = (await NextMessageAsync(
						e, TimeSpan.FromSeconds(30))).ToString();

					if (answer.ToLower() == "yes" || answer.ToLower() == "accept")
					{
						int yourCount = 0;
						int enemyCount = 0;
						for(int i = 1; i <= 5; i++)
						{
							Random r = new Random();
							int firstNum = r.Next(50, 201),
								secondNum = r.Next(50, 201),
								sum = firstNum + secondNum;
							await SendEmbed("ROUND " + i, "What is " + firstNum + " + " + secondNum + "?", user);
							await SendEmbed("ROUND " + i, "What is " + firstNum + " + " + secondNum + "?", Context.User as IGuildUser);
							int count = 0;
							while(count < 1)
							{
								try
								{
									var answerMathh = (await NextMessageAsync(false, false, TimeSpan.FromSeconds(30)));
									if (answerMathh.ToString() == sum.ToString())
									{
										await SendEmbed("Someone Answered!", answerMathh.Author + " answer" +
											"ed first!", Context.User as IGuildUser);
										await SendEmbed("Someone Answered!", answerMathh.Author + " answer" +
											"ed first!", user);
										if (answerMathh.Author == Context.User) yourCount++;
										if (answerMathh.Author == user) enemyCount++;
										count++;
										break;
									}
									else
									{
										answerMathh = (await NextMessageAsync(false, false, TimeSpan.FromSeconds(30)));
									}
								}
								catch { continue; }
							}
						}

						if(enemyCount > yourCount)
						{
							await SendEmbed("You lose!", "Go learn more math son", Context.
								User as IGuildUser);
							await SendEmbed("You win!", "Nice one mate", user);
						}
						else
						{
							await SendEmbed("You lose!", "Go learn more math son", user);
							await SendEmbed("You win!", "Nice one mate", Context.User as
								 IGuildUser);
						}
					}
					else
					{
						await SendEmbed("Cancelled", "User " + user + " didn't accept" +
						" your request.", Context.User as IGuildUser);
						await SendEmbed("Cancelled", "You declined " +
							"the request.", user);
						return;
					}
				}
				catch
				{
					await SendEmbed("Cancelled", "User " + user + " didn't accept" +
						" your request.", Context.User as IGuildUser);
					await SendEmbed("Cancelled", "You declined " +
						"the request.", user);
					return;
				}
			}
		}

		public async Task SendEmbed(string title, string description,IGuildUser user = null)
		{
			if (user != null)
			{
				Random r = new Random();
				EmbedBuilder e = new EmbedBuilder();
				e.AddField(title, description)
					.WithAuthor("Math Battle")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await user.SendMessageAsync("", false, e.Build());
			}
			else
			{
				Random r = new Random();
				EmbedBuilder e = new EmbedBuilder();
				e.AddField(title, description)
					.WithAuthor("Math Battle")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
			}
		}
	}
}
