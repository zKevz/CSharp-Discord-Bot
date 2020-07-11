using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;
using static SampleApp.Commands;

namespace CowboyBot
{
	public class EventHandler : InteractiveBase
	{
		[Command("answer")]
		public async Task answer(string s)
		{
			if (Context.Channel.Id != 719825195648548944) return;
			if (isEvent == true)
			{
				bool isDigit = s.All(char.IsNumber);
				if (isDigit == false)
				{
					await ReplyAsync("You can only put number!");
					return;
				}
				else
				{
					int fn = FirstNum;
					int sn = SecondNum;
					int p = Prize;
					int sum = fn + sn;

					if (s == sum.ToString())
					{
						isEvent = false;
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("YEAY", "The winner for this event is " +
							Context.User.Mention + " with account name " +
							accName(Context.User.Id) + "\nCongrats you have got " +
							"" + p + " coins as your prize!")
							.WithAuthor("Event Has Ended")
							.WithFooter("Bot made by kevz#2073")
							.WithCurrentTimestamp();
						await ReplyAsync("",false,e.Build());

						dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json"));
						j["coins"] += p;
						string output = JsonConvert.SerializeObject(j, Formatting.Indented);
						File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", j.ToString());
					}
					else return;
				}
			}
			else
			{
				await ReplyAsync("There is no event right now.");
			}
		}
	}
}
