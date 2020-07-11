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
	public class RobBankModel : InteractiveBase
	{
		[Command("robbank",RunMode = RunMode.Async)]
		public async Task RobBank()
		{
			Random r = new Random();
			string username = accName(Context.User.Id);
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));
			if ((int)j["coins"] < 1000)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "You need 1000 Coins to rob a bank!")
					.WithAuthor("Robbing A Bank")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
			}
			else
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Confirmation", "Are you sure you want to rob a bank?\n" +
					"DISCLAIMER:\n" +
					"[1] You'll go to jail if you failed\n" +
					"[2] You'll lose 200 coins if you failed.\n" +
					"If you still want to do it , type \"accept\"\n")
					.WithAuthor("Robbing A Bank")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());

				string xd = (await NextMessageAsync()).ToString();

				if (xd.ToLower() == "accept")
				{
					int chance = r.Next(0, 1001);

					if(chance >= 690 && chance < 700)
					{
						int coinsGet = r.Next(5000, 10001);
						EmbedBuilder eee = new EmbedBuilder();
						eee.AddField("YEAYYY", "You succeed to rob a bank and got " + coinsGet + " coins!")
							.WithAuthor("ROB SUCCEED!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(Color.Green)
							.WithCurrentTimestamp();
						await ReplyAsync("", false, eee.Build());

						dynamic d = JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json"));
						d["coins"] += coinsGet;
						string output = JsonConvert.SerializeObject(d, Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);
					}
					else
					{
						int timejailed = r.Next(2,11);
						string format = "days";

						EmbedBuilder eee = new EmbedBuilder();
						eee.AddField("WHOOPS", "You failed to rob a bank!\nYou have been " +
							"jailed for " + timejailed + " days and you lost 200 coins." )
							.WithAuthor("Failed!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(Color.Red)
							.WithCurrentTimestamp();
						await ReplyAsync("", false, eee.Build());

						jailtimer.Add(DateTimeOffset.Now);
						jailtarget.Add(accName(Context.User.Id));

						JObject jlol = new JObject(
							new JProperty("time", DateTimeOffset.Now),
							new JProperty("format", format),
							new JProperty("jailtime", timejailed)
							);
						File.WriteAllText(@"database\jailed\" + accName(Context.User.Id) + ".json", jlol.ToString());

						dynamic d = JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json"));
						d["coins"] -= 200;
						string output = JsonConvert.SerializeObject(d, Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);

						return;
					}
				}
				else
				{
					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("Robbing a bank", "You decided to not rob a bank")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor("")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithCurrentTimestamp();
					await ReplyAsync("", false, ee.Build());
					return;
				}
			}
		}
	}
}
