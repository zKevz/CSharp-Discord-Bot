using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class SendCoins :InteractiveBase
	{
		[Command("givecoin",RunMode = RunMode.Async)]
		[Alias("givecoins")]
		public async Task GiveCoins(IGuildUser user, int amount)
		{
			string username = accName(Context.User.Id);

			if(Context.User == user)
			{
				await ReplyAsync("You can't give yourself!");
				return;
			}
			else if (!isOnAccount(user.Id))
			{
				await ReplyAsync("That user doesn't even have cowboy account!");
				return;
			}
			else if (amount <= 0)
			{
				await ReplyAsync("You can't give lower than 1 coin!");
				return;
			}
			else if (coins(username) < amount)
			{
				await ReplyAsync("Sorry! You only have " +
					coins(username) + " coins!");
				return;
			}
			else
			{
				await ReplyAsync("Are you sure you want " +
					"to give user " + user + " " +
					amount + " coins?\n" +
					"Type \"yesiwant\" if you really want!");
				try
				{
					string answer = (await NextMessageAsync(true,
						true,TimeSpan.FromSeconds(30))).ToString(); ;
					if (answer.ToLower() == "yesiwant")
					{
						dynamic j1 = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\account\" +
							username + ".json"));
						dynamic j2 = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\account\" +
							accName(user.Id) + ".json"));

						j1["coins"] -= amount;
						j2["coins"] += amount;
						string output1 = JsonConvert.SerializeObject(j1, Formatting.Indented);
						string output2 = JsonConvert.SerializeObject(j2, Formatting.Indented);

						File.WriteAllText(@"database\account\" + username + ".json", output1);
						File.WriteAllText(@"database\account\" + accName(user.Id) + ".json", output2);

						await ReplyAsync("You gave user " + user + " " + amount + " coins!");

					}
					else
					{
						await ReplyAsync("Giving coins cancelled!");
					}
				}
				catch
				{
					await ReplyAsync("Giving coins cancelled!");
				}
			}
		}
	}
}
