using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class Steal : InteractiveBase
	{
		private static ColorList c = new ColorList();
		private readonly int r = c.r;
		private readonly int g = c.g;
		private readonly int b = c.b;

		[Command("steal")]
		public async Task steal(IGuildUser user)
		{
			int time = 3;

			if (!isOnAccount(user.Id))
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "That user doesn't even have cowboy account!")
					.WithColor(r, g, b)
					.WithAuthor("No Account Found")
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (lastSteal(accName(Context.User.Id), time) == false && Context.User.ToString()!="kevz#2073")
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "Slowdown a bit!\nYou will either lose all your money or" +
					" take all money if you steal again and again!\n")
					.WithColor(r, g, b)
					.WithAuthor("Slowdown")
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (coins(accName(user.Id)) < 100)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "The person doesn't even have 100 coins! Not worth it!")
					.WithColor(r, g, b)
					.WithAuthor("RIP")
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (coins(accName(Context.User.Id)) < 50)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "You need atleast 50 coins to steal someone!")
					.WithColor(r, g, b)
					.WithAuthor("Not Enough Coins")
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else
			{
				Random r = new Random();
				int a = r.Next(0, 2);
				if(a == 0)
				{
					int b = r.Next(0, 4);
					if(b == 3)
					{
						int c = r.Next(0, 3);
						int timejailed = 0;
						string format ="seconds";

						int randoms = r.Next(2, 1001);
						int randomm = r.Next(2, 501);
						int randomh = r.Next(2, 8);

						switch (c)
						{
							case 0:
								timejailed = randoms;
								format = "seconds";
								break;
							case 1:
								timejailed = randomm;
								format = "minutes";
								break;
							case 2:
								timejailed = randomh;
								format = "hours";
								break;
						}

						EmbedBuilder eee = new EmbedBuilder();
						eee.AddField("WHOOPS", "You failed to steal!\n" +
							"A cop saw what you did!\n" +
							"You are jailed for " + timejailed + " " + format + "!")
							.WithAuthor("Failed!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(Color.Red)
							.WithCurrentTimestamp();
						await ReplyAsync("", false, eee.Build());

						jailtimer.Add(DateTimeOffset.Now);
						jailtarget.Add(accName(Context.User.Id));

						JObject j = new JObject(
							new JProperty("time",DateTimeOffset.Now),
							new JProperty("format", format),
							new JProperty("jailtime", timejailed)
							);
						File.WriteAllText(@"database\jailed\" + accName(Context.User.Id) + ".json", j.ToString());

						return;
					}
					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("WHOOPS", "You failed to steal! XD LOL!\n" +
						"But luckily the person didn't notice and didn't call the cops!")
						.WithAuthor("Failed!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(Color.Red)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, ee.Build());
				}
				else
				{
					int getCoins = r.Next(0, 51);
					string path = File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json");
					int jackpot = r.Next(1,1001);
					if (jackpot == 1000)
					{
						getCoins = r.Next(10000, 30001);

						EmbedBuilder eeee = new EmbedBuilder();
						eeee.AddField("YAY", "You succeed to steal " + user + " with account name " + accName(user.Id) + "\nYou also stole" +
							" the richest man in the world! You got jackpot " + getCoins + " coins!")
							.WithAuthor("Success!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(Color.Red)
							.WithCurrentTimestamp();
						await ReplyAsync("", false, eeee.Build());

						dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);

						jsonObj["coins"] += getCoins;

						string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
						return;
					}

					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("YAY", "You succeed to steal " + user + " with account name " + accName(user.Id) + "" +
						"\nYou got " + getCoins + " coins!")
						.WithAuthor("Success!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(Color.Red)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, ee.Build());
					try
					{
						dynamic jsonObjj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);

						dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + accName(user.Id) + ".json"));

						string outt = "";
						try
						{
							j["coins"] -= getCoins;

							jsonObjj["coins"] += getCoins;

							outt = JsonConvert.SerializeObject(j, Formatting.Indented);
						}
						catch { Console.WriteLine("error here ffs"); }
						try
						{
							File.WriteAllText(@"database\account\" + accName(user.Id) + ".json", outt);

							string outputt = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObjj, Newtonsoft.Json.Formatting.Indented);
							File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", outputt);
						}
						catch { Console.WriteLine("ERROR HERE FUCKKKKKK"); }
					}
					catch { Console.WriteLine("ERRRRRRRRORRRRRRRRRRRRRRRRRRRRRRR"); }
				}
			}
		}

		public async Task jailTime(string user, IGuildUser userhaha)
		{
			int formattime = 0;
			JObject j = JObject.Parse(File.ReadAllText(@"database\jailed\" + user + ".json"));

			string format = (string)j["format"];
			int time = (int)j["jailtime"];

			switch (format)
			{
				case "seconds":
					formattime = 1000;
					break;
				case "minutes":
					formattime = 1000 * 60;
					break;
				case "hours":
					formattime = 1000 * 60 * 60;
					break;
				case "days":
					formattime = 1000 * 60 * 60 * 24;
					break;
			}

			string[] jailFile = Directory.GetFiles(@"database\jailed");
			foreach(string s in jailFile)
			{
				if (s.Contains(".json"))
				{
					if (!jailtarget.Contains(user))
					{
						jailtarget.Add(user);
						jailtimer.Add((DateTimeOffset) j["time"]);
						
					}
					else continue;
				}
				else continue;
			}

			if (jailtarget.Contains(user))
			{
				if((formattime * time) >= (DateTimeOffset.Now - (DateTimeOffset)j["time"]).TotalMilliseconds)
				{
					int hoursLeft = 0;
					int minutesLeft = (int)(((DateTimeOffset)j["time"]).AddMilliseconds(formattime * time) - DateTimeOffset.Now).TotalMinutes;
					int secondsLeft = (int)(((DateTimeOffset)j["time"]).AddMilliseconds(formattime * time) - DateTimeOffset.Now).TotalSeconds;

					string hours = "";

					if(minutesLeft >= 60)
					{
						hoursLeft = (int)(((DateTimeOffset)j["time"]).AddMilliseconds(formattime * time) - DateTimeOffset.Now).TotalHours;
					}

					secondsLeft = secondsLeft - (minutesLeft * 60);

					if (hoursLeft > 0)
					{
						minutesLeft -= (hoursLeft * 60);
						hours = hoursLeft + " hours, ";
					}

					string minutes = minutesLeft + " minutes and ";
					if (minutesLeft == 0)
					{
						minutes = "";
					}

					await userhaha.SendMessageAsync("You are in jail! You need to wait " + hours + minutes + secondsLeft + " seconds to be free!");
				}
				else
				{
					File.Delete(@"database\jailed\" + user + ".json");
				}
			}
			else return;
		}

		public bool isOnJail(string username) { return File.Exists(@"database\jailed\" + username + ".json"); }
	}
}
