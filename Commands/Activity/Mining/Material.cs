using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using static CowboyBot.Timer;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
	public class Material : InteractiveBase
	{
		[Command("sellmaterial",RunMode = RunMode.Async)]
		public async Task SellMaterial(string type, int amount)
		{
			string username = accName(Context.User.Id);
			JObject j = JObject.Parse(
				File.ReadAllText(@"database\inventory\" + username + ".json"));
			Console.WriteLine(f(type));
			string l = f(type);

			if (l != "rock" && l != "coal" && l != "metals" && l != "gold" && l != "diamond")
			{
				await ReplyAsync("Invalid type of materials!");
			}
			else if((int)j[f(type)] < amount)
			{
				await ReplyAsync("You only have " + j[f(type)] + " " + type + "!");
				return;
			}
			else if(f(type) == "rock" && amount < 200)
			{
				await ReplyAsync("The price of rock is 200 per coins. You need " + (200 - amount) +
					" rocks more if you want to sell");
				return;
			}
			else if (f(type)=="rock"&& amount % 200 != 0)
			{
				await ReplyAsync("You need to pass the rock amount that is multiple by 200.\n" +
					"Example : 200,400,600,800");
				return;
			}
			else
			{
				int price = 0;
				int moneyCollected = 0;

				string s = f(type);
				if (s == "coal" || s == "rock")
				{
					price = 1;
				}
				else if (s == "metals")
				{
					price = 5;
				}
				else if (s == "gold")
				{
					price = 300;
				}
				else price = 1000;
				
				if(s == "rock")
				{
					moneyCollected = amount / 200;
				}
				else
				{
					moneyCollected = amount * price;
				}
				Random r = new Random();

				string shit = price.ToString();
				if (s == "rock") shit = "1 / 200";

				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Confirmation", "Are you sure you want to sell " + s + "?\n" +
					"Price per item : " + shit + " coins\n" +
					"Total Coins : " + moneyCollected + "\n" +
					"Type : " + s + "\n" +
					"Amount : " + amount + "\nType" +
					" yes if you want to do it!")
					.WithAuthor("Cowboy's Trade")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				try
				{
					string answer = (await NextMessageAsync(true, true,
						TimeSpan.FromSeconds(30))).ToString();
					if (answer.ToLower() == "yes" || answer.ToLower() == "accept")
					{
						dynamic j1 = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\account\" + username + ".json"));
						dynamic j2 = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\inventory\" + username + ".json"));
						j1["coins"] += moneyCollected;
						j2[s] -= amount;
						string output1 = JsonConvert.SerializeObject(j1, Formatting.Indented);
						string output2 = JsonConvert.SerializeObject(j2, Formatting.Indented);
						File.WriteAllText(@"database\account\" + username + ".json", output1);
						File.WriteAllText(@"database\inventory\" + username + ".json", output2);

						EmbedBuilder ee = new EmbedBuilder();
						ee.AddField("Trade Success", "You sold " + s + "!\n" +
							"Price per item : " + shit + " coins\n" +
							"Total Coins : " + moneyCollected + "\n" +
							"Type : " + s + "\n" +
							"Amount : " + amount)
							.WithAuthor("Cowboy's Trade")
							.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
							.WithFooter("Bot made by kevz#2073")
							.WithCurrentTimestamp();
						await ReplyAsync("", false, ee.Build());
					}
					else
					{
						await ReplyAsync("You cancelled the trade.");
						return;
					}
				}
				catch
				{
					await ReplyAsync("You cancelled the trade.");
					return;
				}
			}
		}
		public static string f(string s)
		{
			if (s[s.Length - 1] == 's' && s != "metals")
				return s.Remove(s.IndexOf("s"));
			else return s;
		}
	}
}
