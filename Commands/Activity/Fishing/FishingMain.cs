using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class FishingMain : InteractiveBase
	{
		[Command("fishing",RunMode = RunMode.Async)]
		[Alias("fish")]
		public async Task fishing()
		{
			string currentUser = accName(Context.User.Id);
			Random r = new Random();

			if (!lastFishing(currentUser,5) && Context.User.ToString() != "kevz#2073")
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Slowdown a bit", "The default cooldown is 5 minutes for " +
					"normal roles and 2 minutes for VIP.")
					.WithAuthor("COOLDOWN")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));
				await ReplyAsync("", false, e.Build());
			}
			else
			{
				int numOne = r.Next(50,301), numTwo = r.Next(50, 301), sum = numOne + numTwo, answer = 0;
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Answer The Question!", "What is " + numOne + " + " + numTwo + "?\n" +
					"You have 3 seconds...")
					.WithAuthor("Fishing Fishy YaY Yay")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

				await ReplyAsync("", false, e.Build());

				try
				{
					answer = Convert.ToInt32((await NextMessageAsync(true, true, TimeSpan.FromSeconds(5))).Content);
				}
				catch
				{
					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("RIP", "You didn't either answer the question or " +
						"you didn't answer with `number!` sad...")
						.WithAuthor("Fishing Fishy YaY Yay")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

					await ReplyAsync("", false, ee.Build());
					return;
				}

				if (answer == sum)
				{

					List<string> FishList = new List<string>();

					JObject jasdasd = JObject.Parse(File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json"));

					if ((int)jasdasd["player_rod"] == 2)
					{
						for (int i = 0; i < 35; i++) FishList.Add("Small Fish");
						for (int i = 35; i < 70; i++) FishList.Add("Medium Fish");
						for (int i = 70; i < 95; i++) FishList.Add("Big Fish");
						for (int i = 95; i < 100; i++) FishList.Add("shark");
					}
					else if ((int)jasdasd["player_rod"] == 3)
					{
						for (int i = 0; i < 25; i++) FishList.Add("Small Fish");
						for (int i = 25; i < 60; i++) FishList.Add("Medium Fish");
						for (int i = 60; i < 90; i++) FishList.Add("Big Fish");
						for (int i = 90; i < 100; i++) FishList.Add("shark");
					}
					else
					{
						for (int i = 0; i < 100; i++)
						{
							if (i >= 0 && i < 65) FishList.Add("Small Fish");
							else if (i >= 65 && i < 90) FishList.Add("Medium Fish");
							else if (i >= 90 && i < 99) FishList.Add("Big Fish");
							else if (i == 99) FishList.Add("shark");
						}
					}
					string FishCollected = FishList[r.Next(0, 100)];
					int AmountOfFishCollected = r.Next(1, 5);

					string f = "";

					if (FishCollected.Contains(" "))
					{
						f = FishCollected.ToLower().Remove(FishCollected.IndexOf(" "), 1);
					}
					else f = FishCollected.ToLower();

					string text = File.ReadAllText(@"database\account\" + currentUser + ".json");

					dynamic j = JsonConvert.DeserializeObject(text);
					j[f] += AmountOfFishCollected;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);

					File.WriteAllText(@"database\account\" + currentUser + ".json", output);

					dynamic j123 = JsonConvert.DeserializeObject(File.ReadAllText(@"database\achievement\" + currentUser + ".json"));
					j123["fishingcount"] += 1;
					string outputttkik = JsonConvert.SerializeObject(j123, Formatting.Indented);

					File.WriteAllText(@"database\achievement\" + currentUser + ".json", outputttkik);

					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("YEAYYY", "You answered the correct answer which is " +
						$"`{sum}!` You got " + AmountOfFishCollected + " " + FishCollected + "!")
						.WithAuthor("Fishing Fishy YaY Yay")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

					await ReplyAsync("", false, ee.Build());

					JObject asd = JObject.Parse(File.ReadAllText(@"database\achievement\" + currentUser + ".json"));
					if ((int)asd["fishingcount"] >= 300)
					{
						EmbedBuilder eeee = new EmbedBuilder();
						eeee.AddField("YEAYYY", "User " + Context.User.Mention + "" +
							" with account name " + accName(Context.User.Id) + " have just achieved" +
							" Fishing Expert Achievement!")
							.WithAuthor("Fishing Fishy YaY Yay")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

						var channel = Context.Guild.GetChannel(720269935833776239) as ITextChannel;
						await channel.SendMessageAsync("",false,eeee.Build());

						dynamic j1234 = JsonConvert.DeserializeObject(File.ReadAllText(@"database\achievement\" + currentUser + ".json"));
						j1234["fishingexpert"] = true;
						string outputttt123 = JsonConvert.SerializeObject(j1234, Formatting.Indented);

						File.WriteAllText(@"database\achievement\" + accName(Context.User.Id) + ".json", outputttt123);
						return;
					}
					else return;
				}
				else
				{
					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("RIP", "Your answer was wrong! The right one is `"+ sum+"!`")
						.WithAuthor("Fishing Fishy YaY Yay")
						.WithFooter("Better Learn More Math")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

					await ReplyAsync("", false, ee.Build());
					return;
				}
			}
		}

		[Command("sellfish",RunMode = RunMode.Async)]
		[Alias("tradefish","fishsell")]
		public async Task SellFish()
		{
			Random r = new Random();
			string currentuser = accName(Context.User.Id);
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + currentuser + ".json"));

			if((int) j["smallfish"] == 0 && (int) j["mediumfish"] == 0 && (int) j["bigfish"] == 0 && (int) j["shark"] == 0)
			{
				await ReplyAsync("You have no fish to be sell!");
			}
			else
			{
				EmbedBuilder ee = new EmbedBuilder();
				ee.AddField("What fish would you like to sell?", "" +
					"[1] Small Fish : " + j["smallfish"] +
					"\n[2] Medium Fish : " + j["mediumfish"] +
					"\n[3] Big Fish : " + j["bigfish"] +
					"\n[4] shark : " + j["shark"] +
					"\n\nPlease answer by number.")
					.AddField("Price :", "Small Fish - 5 Coins\n" +
					"Medium Fish - 40 Coins\n" +
					"Big Fish - 300 Coins\n" +
					"shark - 3500 Coins")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithAuthor("Fishing Trade");

				await ReplyAsync("", false, ee.Build());

				int type = 0;
				try
				{
					type = Convert.ToInt32((await NextMessageAsync(true,true,TimeSpan.FromSeconds(10))).Content);
				}
				catch
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("WHOOPS", "You didn't either" +
						" answer or you answered in a wrong format.")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithAuthor("Fish Trade Cancelled");
					await ReplyAsync("", false, e.Build());
					return;
				}

				if (type == 1)
				{
					if ((int)j["smallfish"] > 0)
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("Amount Of Fish", "How many fish would you like to sell?")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade");
						await ReplyAsync("", false, e.Build());

						int asd = 0;
						try
						{
							asd = Convert.ToInt32((await NextMessageAsync(true, true, TimeSpan.FromSeconds(10))).Content);
						}
						catch
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You didn't either" +
								" answer or you answered in a wrong format.")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}

						if (asd > (int)j["smallfish"])
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You only have " + j["smallfish"] + "" +
								" SMALL FISH!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}
						else
						{
							dynamic jasd = JsonConvert.DeserializeObject(File.ReadAllText(
								@"database\account\" + currentuser + ".json"));
							jasd["smallfish"] -= asd;
							jasd["coins"] += (asd * 5);
							string output = JsonConvert.SerializeObject(jasd, Formatting.Indented);
							File.WriteAllText(@"database\account\" + currentuser + ".json", output);

							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("Trade Succeed", "You sold " + asd + " small fish for " + (asd*5) + " coins!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Succeed");
							await ReplyAsync("", false, eee.Build());
							return;
						}
					}
					else
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("WHOOPS", "You don't even have that fish!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade Cancelled");
						await ReplyAsync("", false, e.Build());
						return;
					}
				}

				else if (type == 2)
				{
					if ((int)j["mediumfish"] > 0)
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("Amount Of Fish", "How many fish would you like to sell?")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade");
						await ReplyAsync("", false, e.Build());

						int asd = 0;
						try
						{
							asd = Convert.ToInt32((await NextMessageAsync(true, true, TimeSpan.FromSeconds(10))).Content);
						}
						catch
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You didn't either" +
								" answer or you answered in a wrong format.")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}

						if (asd > (int)j["mediumfish"])
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You only have " + j["mediumfish"] + "" +
								" MEDIUM FISH!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}
						else
						{
							dynamic jasd = JsonConvert.DeserializeObject(File.ReadAllText(
								@"database\account\" + currentuser + ".json"));
							jasd["mediumfish"] -= asd;
							jasd["coins"] += (asd * 40);
							string output = JsonConvert.SerializeObject(jasd, Formatting.Indented);
							File.WriteAllText(@"database\account\" + currentuser + ".json", output);

							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("Trade Succeed", "You sold " + asd + " medium fish for " + (asd * 40) + " coins!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Succeed");
							await ReplyAsync("", false, eee.Build());
							return;
						}
					}
					else
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("WHOOPS", "You don't even have that fish!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade Cancelled");
						await ReplyAsync("", false, e.Build());
						return;
					}
				}
				else if (type == 3)
				{
					if ((int)j["bigfish"] > 0)
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("Amount Of Fish", "How many fish would you like to sell?")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade");
						await ReplyAsync("", false, e.Build());

						int asd = 0;
						try
						{
							asd = Convert.ToInt32((await NextMessageAsync(true, true, TimeSpan.FromSeconds(10))).Content);
						}
						catch
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You didn't either" +
								" answer or you answered in a wrong format.")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}

						if (asd > (int)j["bigfish"])
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You only have " + j["bigfish"] + "" +
								" BIG FISH!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}
						else
						{
							dynamic jasd = JsonConvert.DeserializeObject(File.ReadAllText(
								@"database\account\" + currentuser + ".json"));
							jasd["bigfish"] -= asd;
							jasd["coins"] += (asd * 300);
							string output = JsonConvert.SerializeObject(jasd, Formatting.Indented);
							File.WriteAllText(@"database\account\" + currentuser + ".json", output);

							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("Trade Succeed", "You sold " + asd + " big fish for " + (asd * 300) + " coins!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Succeed");
							await ReplyAsync("", false, eee.Build());
							return;
						}
					}
					else
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("WHOOPS", "You don't even have that fish!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade Cancelled");
						await ReplyAsync("", false, e.Build());
						return;
					}
				}
				else if (type == 4)
				{
					if ((int)j["shark"] > 0)
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("Amount Of Fish", "How many fish would you like to sell?")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade Cancelled");
						await ReplyAsync("", false, e.Build());

						int asd = 0;
						try
						{
							asd = Convert.ToInt32((await NextMessageAsync(true, true, TimeSpan.FromSeconds(10))).Content);
						}
						catch
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You didn't either" +
								" answer or you answered in a wrong format.")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}

						if (asd > (int)j["shark"])
						{
							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("WHOOPS", "You only have " + j["shark"] + "" +
								" shark!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Cancelled");
							await ReplyAsync("", false, eee.Build());
							return;
						}
						else
						{
							dynamic jasd = JsonConvert.DeserializeObject(File.ReadAllText(
								@"database\account\" + currentuser + ".json"));
							jasd["shark"] -= asd;
							jasd["coins"] += (asd * 3500);
							string output = JsonConvert.SerializeObject(jasd, Formatting.Indented);
							File.WriteAllText(@"database\account\" + currentuser + ".json", output);

							EmbedBuilder eee = new EmbedBuilder();
							eee.AddField("Trade Succeed", "You sold " + asd + " shark for " + (asd * 3500) + " coins!")
								.WithFooter("Bot made by kevz#2073")
								.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
								.WithAuthor("Fish Trade Succeed");
							await ReplyAsync("", false, eee.Build());
							return;
						}
					}
					else
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("WHOOPS", "You don't even have that fish!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithAuthor("Fish Trade Cancelled");
						await ReplyAsync("", false, e.Build());
						return;
					}
				}
			}
		}
	}
}
