using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
namespace CowboyBot
{
	public class SlotMachine : InteractiveBase
	{
		
		[Command("slotmachine",RunMode = RunMode.Async)]
		[Alias("sm","slotm","smachine")]
		public async Task bet(params string[] a)
		{
			await SendEmbed("Confirmation","Do you really want to" +
				" play slot machine? It costs 50 Coins.\n" +
				"Type \"accept\" if you want to play!");

			string answer = "";
			try
			{
				answer = (await NextMessageAsync(true,true,TimeSpan.FromSeconds(30))).ToString();
			}
			catch
			{
				await ReplyAsync("You didn't answer! Cancelled" +
					" by default");
				return;
			}

			if (answer.ToLower() == "yes" || answer.ToLower() == "accept")
			{
				Random r = new Random();

				dynamic jj = JsonConvert.DeserializeObject(File.ReadAllText(
							@"database\account\" + accName(Context.User.Id) + ".json"));
				jj["coins"] -= 50;
				string outputj = JsonConvert.SerializeObject(jj, Formatting.Indented);
				File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", outputj);

				List<string> randomString = new List<string>()
				{
					"apple",
					"gun",
					"jackpot",
					"banana",
					"pear",
					"glasses",
					"chair",
					"tiger",
					"duck",
					"monkey"
				};

				string sFirst = randomString[r.Next(0,10)];
				string sSecond = randomString[r.Next(0, 10)];
				string sThird = randomString[r.Next(0, 10)];
				if (sFirst==sSecond && sSecond == sThird && sThird==sFirst)
				{
					if (sFirst == "gun")
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("You Win!", "You got shotgun!")
							.AddField("ITEM 1", sFirst, true)
							.AddField("ITEM 2", sSecond, true)
							.AddField("ITEM 3", sThird, true)
							.WithAuthor(Context.User.Username + "'s SLOT MACHINE")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithCurrentTimestamp();

						await ReplyAsync("", false, e.Build());
						dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
							@"database\account\" + accName(Context.User.Id) + ".json"));
						j["player_hand"] = 3;
						string output = JsonConvert.SerializeObject(j, Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
					}
					else if (sFirst == "jackpot")
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("You Win!", "You got 3 **jackpot** in a row! You win 10.000 coins")
							.AddField("ITEM 1", sFirst, true)
							.AddField("ITEM 2", sSecond, true)
							.AddField("ITEM 3", sThird, true)
							.WithAuthor(Context.User.Username + "'s SLOT MACHINE")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithCurrentTimestamp();

						await ReplyAsync("", false, e.Build());
						dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
							@"database\account\" + accName(Context.User.Id) + ".json"));
						j["coins"] += 10000;
						string output = JsonConvert.SerializeObject(j, Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
					}
					else
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("You Win!", "You got 3 items in a row! You win 1.000 coins")
							.AddField("ITEM 1", sFirst, true)
							.AddField("ITEM 2", sSecond, true)
							.AddField("ITEM 3", sThird, true)
							.WithAuthor(Context.User.Username + "'s SLOT MACHINE")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithCurrentTimestamp();

						await ReplyAsync("", false, e.Build());
						dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
							@"database\account\" + accName(Context.User.Id) + ".json"));
						j["coins"] += 1000;
						string output = JsonConvert.SerializeObject(j, Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
					}
				}
				else if (sFirst == sSecond || sSecond == sThird || sFirst == sThird)
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("You Win!", "You got 2 items in a row! You win 80 coins")
						.AddField("ITEM 1", sFirst, true)
						.AddField("ITEM 2", sSecond, true)
						.AddField("ITEM 3", sThird, true)
						.WithAuthor(Context.User.Username + "'s SLOT MACHINE")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithCurrentTimestamp();

					await ReplyAsync("", false, e.Build());
					dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
							@"database\account\" + accName(Context.User.Id) + ".json"));
					j["coins"] += 80;
					string output = JsonConvert.SerializeObject(j, Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
				}
				else
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("You Lose!", "Better luck next time!")
						.AddField("ITEM 1", sFirst, true)
						.AddField("ITEM 2", sSecond, true)
						.AddField("ITEM 3", sThird, true)
						.WithAuthor(Context.User.Username + "'s SLOT MACHINE")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithCurrentTimestamp();
						
					await ReplyAsync("", false, e.Build());
				}
			}
			else
			{
				await ReplyAsync("You declined the request.");
				return;
			}
		}

		public async Task SendEmbed(string title, string description)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, description)
				.WithAuthor("Slot Machine")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await Context.Channel.SendMessageAsync("", false, e.Build());
		}
	}
}
