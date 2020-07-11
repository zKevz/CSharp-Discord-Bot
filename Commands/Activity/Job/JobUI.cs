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
using Newtonsoft.Json;

namespace CowboyBot
{
	public class JobUI : InteractiveBase
	{
		[Command("joblist")]
		[Alias("listjob","jobhelp","helpjob")]
		public async Task JobList()
		{
			Random r = new Random();

			EmbedBuilder e = new EmbedBuilder();
			e.AddField("JOB LIST", "\n" +
				"[1] Farmer				[10 Coins per Hour]\n" +
				"[2] Security Guard		[20 Coins per Hour]\n" +
				"[3] Athlete			[30 Coins per Hour]\n" +
				"[4] Youtuber			[40 Coins per Hour]\n" +
				"[5] Boss				[50 Coins per Hour]\n")
				.WithAuthor("Cowboy's Job")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}

		[Command("getjob")]
		[Alias("registerjob","jobget")]
		public async Task GetJob(int i = -1)
		{
			Random r = new Random();

			string username = accName(Context.User.Id);

			if (!lastRegisterJob(username, 180) && Context.User.ToString()!="kevz#2073")
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "Wait 3 Hours before get a job again!")
					.WithAuthor("COOLDOWN")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			if (i == -1)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "You put the wrong syntax.\n" +
					"**SYNTAX** : \"c getjob (number)\"\n" +
					"To get the list of job and its number, do \"c joblist\"")
					.WithAuthor("Having a problem?")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
			}
			else
			{
				bool isValid = false;
				for (int a = 1; a <= 5; a++) if (i == a) isValid = true;
				if (!isValid)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("WHOOPS", "You put the valid number!")
						.WithAuthor("Having a problem?")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
				else
				{
					int chance = r.Next(0,4*i);
					if(chance == i)
					{
						EmbedBuilder ee = new EmbedBuilder();
						ee.AddField("YAY", "You have been accepted and now you are " +
							"working as a " + GetJobByNumber(i) + "!")
							.WithAuthor("You have been accepted")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithCurrentTimestamp();
						await ReplyAsync("", false, ee.Build());

						dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
							@"database\account\" + username + ".json"));
						j["job"] = GetJobByNumber(i);
						string output = JsonConvert.SerializeObject(j, Formatting.Indented);

						File.WriteAllText(@"database\account\" + username + ".json", output);
					}
					else
					{
						List<string> funnyQuotes = new List<string>()
						{
							"The boss didn't like your face and declined you.",
							"Your stomach hurted and you went to doctor instead",
							"You didn't get your job as " + GetJobByNumber(i),
							"The interviewer is actually your ex! She literally kicked you out"
						};

						EmbedBuilder ee = new EmbedBuilder();
						ee.AddField("WHOOPS", funnyQuotes[r.Next(0,4)])
							.WithAuthor("You have been declined")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithCurrentTimestamp();
						await ReplyAsync("", false, ee.Build());
						return;
					}
				}
			}
		}
		[Command("work",RunMode = RunMode.Async)]
		public async Task work()
		{
			string username = accName(Context.User.Id);
			if (!lastJob(username, 60) && Context.User.ToString()!="kevz#2073")
			{
				await ReplyAsync("Its not your time to work! Cooldown is 1 hour");
				return;
			}

			Random r = new Random();

			JObject lol = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));

			if ((string)lol["job"] != "none")
			{
				int CoinsCollected = 0;
				try
				{
					CoinsCollected = GetNumberByJob((string)lol["job"]);
				}
				catch { Console.WriteLine("error"); }
				string random = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
				string result = "";

				for(int i = 0; i < 10; i++)
				{
					result += random[r.Next(0, 52)];
				}

				string finalResult = Reverse(result);

				int randomTimeSpan = r.Next(1, 11);

				await ReplyAsync("TYPE THIS WORD WITH BACKWARDS / DESCENDING!\n" +
					"`" + result + "`\nYou have " + randomTimeSpan + " seconds.");

				string s = "";
				try
				{
					s = (await NextMessageAsync(true, true, TimeSpan.FromSeconds(randomTimeSpan))).ToString();
				}
				catch (NullReferenceException)
				{
					await ReplyAsync("You didn't answer in time!");
					return;
				}

				if (s == finalResult.ToString())
				{
					await ReplyAsync("Succeed! You got " + CoinsCollected + " from your hardwork!");
					dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
						@"database\account\" + username + ".json"));
					j["coins"] += CoinsCollected;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);

					File.WriteAllText(@"database\account\" + username + ".json", output);
				}
				else
				{
					await ReplyAsync("You didn't answer correctly! You didn't get the your coins");
					return;
				}
			}
			else
			{
				await ReplyAsync("You don't even have a job!");
			}
		}

		public static string Reverse(string s)
		{
			char[] cArr = s.ToCharArray();
			Array.Reverse(cArr);
			return new string(cArr);
		}

		public static int GetNumberByJob(string job)
		{
			switch (job)
			{
				case "farmer":
					return 10;
				case "security guard":
					return 20;
				case "athlete":
					return 30;
				case "youtuber":
					return 40;
				case "boss":
					return 50;
			}
			return -1;
		}

		public static string GetJobByNumber(int i)
		{
			switch (i)
			{
				case 1:
					return "farmer";
				case 2:
					return "security guard";
				case 3:
					return "athlete";
				case 4:
					return "youtuber";
				case 5:
					return "boss";
			}
			return null;
		}
	}
}