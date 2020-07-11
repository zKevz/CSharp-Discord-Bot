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
using static SampleApp.Commands;

namespace CowboyBot
{
	public class LotteryUI : InteractiveBase
	{
		public static List<string> yo = new List<string>();
		public static List<ulong> UserLotteryJoined = new List<ulong>();

		[Command("joinlottery")]
		public async Task JoinLottery()
		{
			if (UserLotteryJoined.Contains(Context.User.Id))
			{
				await ReplyAsync("You already joined the lottery!");
				return;
			}
			else
			{
				await ReplyAsync("You joined the lottery!");
				UserLotteryJoined.Add(Context.User.Id);
			}
		}

		public void SendLottery(object s)
		{
			if (yo.Count <= 0)
			{
				yo.Add("lolol");
				return;
			}
			else
			{
				Random r = new Random();
				string winner = "";
				int loots = r.Next(1000, 5001);
				ulong winnerID = 0;

				if (UserLotteryJoined.Count <= 0)
				{
					ReplyAsync("There is no participants!");
					return;
				}
				else
				{
					winnerID = UserLotteryJoined[r.Next(0, UserLotteryJoined.Count)];
				}

				var guild = ClientPublic.GetGuild(715825843816890368);

				foreach(var a in guild.Users)
				{
					if (a.Id == winnerID) winner = a.Username;
				}

				foreach (var a in guild.Channels)
				{
					if (a.Id == 720202344750645308)
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("YEAY", "The winner for this lottery is " + winner + "!\n" +
							"He carried " + loots + " coins!")
							.WithAuthor("Lottery Winner").
							WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithFooter("Bot made by kevz#2073")
							.WithCurrentTimestamp();
						(a as ITextChannel).SendMessageAsync("",false,e.Build());

						dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + accName(winnerID) + ".json"));

						j["coins"] += loots;
						string output = JsonConvert.SerializeObject(j, Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(winnerID) + ".json", output);

						UserLotteryJoined.Clear();
					}
				}
			}
		}
	}
}
