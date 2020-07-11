using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CowboyBot
{
	public class Leaderboard : InteractiveBase
	{
		[Command("leaderboard")]
		[Alias("leaderboards")]
		public async Task LeaderboardCommand()
		{
			List<int> coins = new List<int>();
			List<string> user = new List<string>();
			List<string> fixedList = new List<string>();

			string[] s = Directory.GetFiles(@"database\account");
			foreach(var a in s)
			{
				JObject j = JObject.Parse(File.ReadAllText(a));
				coins.Add((int)j["coins"]);
				user.Add((string)j["username"]);
			}

			int r = coins.Count;
			if (coins.Count > 5) r = 5;

			for(int i = 0; i < r; i++)
			{
				fixedList.Add(coins.Max().ToString());
				fixedList.Add(user[coins.IndexOf(coins.Max())].ToString());
				user.Remove(user[coins.IndexOf(coins.Max())]);
				coins.RemoveAt(coins.IndexOf(coins.Max()));
			}

			string result = "";
			int count = 1;

			for(int a = 0; a < fixedList.Count; a++)
			{
				if (a % 2 == 0)
				{
					JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + fixedList[a + 1] + ".json"));
					ulong userid = (ulong)j["currentuser"];

					string username = "";

					foreach(var q in Context.Guild.Users)
					{
						if (q.Id == userid) username = q.ToString();
					}

					result += "[" + count + "] Coins : " + fixedList[a] + "\n- Account name : " + fixedList[a + 1] + "\n- Discord : " + username + "\n";
					count++;
				}
			}
			Random ra = new Random();

			EmbedBuilder e = new EmbedBuilder();
			e.AddField("LEADERBOARD BY COINS:", result)
				.WithAuthor("Cowboy's Leaderboard")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(ra.Next(0, 256), ra.Next(0, 256), ra.Next(0, 256))
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}

		[Command("bountyleaderboard")]
		[Alias("bountyleaderboards")]
		public async Task BountyLeaderboard()
		{
			List<int> coins = new List<int>();
			List<string> user = new List<string>();
			List<string> fixedList = new List<string>();

			string[] s = Directory.GetFiles(@"database\account");
			foreach (var a in s)
			{
				JObject j = JObject.Parse(File.ReadAllText(a));
				coins.Add((int)j["bounty"]);
				user.Add((string)j["username"]);
			}

			int r = coins.Count;
			if (coins.Count > 5) r = 5;

			for (int i = 0; i < r; i++)
			{
				fixedList.Add(coins.Max().ToString());
				fixedList.Add(user[coins.IndexOf(coins.Max())].ToString());
				user.Remove(user[coins.IndexOf(coins.Max())]);
				coins.RemoveAt(coins.IndexOf(coins.Max()));
			}

			string result = "";
			int count = 1;

			for (int a = 0; a < fixedList.Count; a++)
			{
				if (a % 2 == 0)
				{
					JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + fixedList[a + 1] + ".json"));
					ulong userid = (ulong)j["currentuser"];

					string username = "";

					foreach (var q in Context.Guild.Users)
					{
						if (q.Id == userid) username = q.ToString();
					}

					result += "[" + count + "] Bounty : " + fixedList[a] + "\n- Account name : " + fixedList[a + 1] + "\n- Discord : " + username + "\n";
					count++;
				}
			}
			Random ra = new Random();

			EmbedBuilder e = new EmbedBuilder();
			e.AddField("LEADERBOARD BY BOUNTY POINTS:", result)
				.WithAuthor("Cowboy's Leaderboard")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(ra.Next(0, 256), ra.Next(0, 256), ra.Next(0, 256))
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}
	}
}