using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class LotteryTicketUI : InteractiveBase
	{
		[Command("buylotteryticket",RunMode = RunMode.Async)]
		[Alias("buylottery", "buyticket")]
		public async Task BuyLottery()
		{
			await ReplyAsync("How many lottery you want" +
				" to buy," + Context.User.Mention + "?\n" +
				"The price is 10 coins each.");

			int amount = 0;
			try
			{
				amount = Convert.ToInt32((await NextMessageAsync(true,false,TimeSpan.FromSeconds(30))).Content);
			}
			catch
			{
				await ReplyAsync("You didn't answer or you put the wrong " +
					"value.");
				return;
			}

			string username = accName(Context.User.Id);

			if(amount <= 0)
			{
				await ReplyAsync("You can't buy less than 1 lottery ticket!");
				return;
			}
			else if (amount > 100)
			{
				await ReplyAsync("You can only buy less than 101 coins.");
				return;
			}
			else if (amount*10 > coins(accName(Context.User.Id)))
			{
				await ReplyAsync("You only have " + coins(username) + " bro..");
				return;
			}
			else
			{
				Random r = new Random();

				dynamic j = JsonConvert.DeserializeObject(
					File.ReadAllText(@"database\account\" + username +
					".json"));
				j["coins"] -= (amount * 10);
				string output = JsonConvert.SerializeObject(j, Formatting.Indented);
				File.WriteAllText(@"database\account\" + username + ".json",
					output);

				List<int> number = new List<int>();

				for(int i = 0; i < amount; i++)
				{
					number.Add(r.Next(0, 100001));
				}

				if (number.Contains(69420))
				{
					await ReplyAsync("YOU GOT THE JACKPOT NUMBER!!!!\n" +
						"YOU GOT 200.000 COINS!");
					dynamic jj = JsonConvert.DeserializeObject(
					File.ReadAllText(@"database\account\" + username +
					".json"));
					jj["coins"] += 200000;
					string outputj = JsonConvert.SerializeObject(jj, Formatting.Indented);
					File.WriteAllText(@"database\account\" + username + ".json",
						outputj);
					return;
				}
				else
				{
					string aasd = "";
					foreach(int i in number)
					{
						aasd += "- " + i + "\n";
					}

					await ReplyAsync("You didn't get the jackpot number. To see all the numbers," +
						"look your dm!\n[LUCKY NUMBER : 69420]");
					await Context.User.SendMessageAsync("Your number was:\n " + aasd);
				}
			}
		}
	}
}
