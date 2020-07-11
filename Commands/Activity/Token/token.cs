using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class token : InteractiveBase
	{
		private static ColorList c = new ColorList();
		private readonly int r = c.r;
		private readonly int g = c.g;
		private readonly int b = c.b;

		[Command("tokeninfo")]
		public async Task tokeninfo([Remainder] string s = null)
		{
			if(s == null)
			{
				File.ReadAllText(@"database\token\token.txt");
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Current price : "+ File.ReadAllText(@"database\token\token.txt") + " coins per token", "To buy, type \"c buytoken [amount]\"")
					.WithAuthor("Token Price")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073");
				await ReplyAsync("", false, e.Build());
			}
			else
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("This command doesn't take argument.", "Syntax : **c tokeninfo**")
					.WithAuthor("Having a problem?")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073");
				await ReplyAsync("", false, e.Build());
				return;
			}
		}

		[Command("buytoken", RunMode = RunMode.Async)]
		public async Task buytoken(int amount = default)
		{
			bool isDigit = false;
			try
			{
				isDigit = amount.ToString().All(char.IsDigit);
			}
			catch { Console.WriteLine("isdigit"); }
			if (amount == default)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("You forgot to put the amount of token you want to buy!", "Syntax : **c tokeninfo**")
					.WithAuthor("Having a problem?")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073");
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (!isDigit)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Put only number!", "Syntax : **c buytoken [amount]**")
					.WithAuthor("Having a problem?")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073");
				await ReplyAsync("", false, e.Build());
				return;
			}

			{
				JObject J = JObject.Parse(File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json"));
				int totalpayment = (amount * Convert.ToInt32(File.ReadAllText(@"database\token\token.txt")));

				if(coins(accName(Context.User.Id)) < totalpayment && coins(accName(Context.User.Id)) != totalpayment)
				{
					EmbedBuilder e1 = new EmbedBuilder();
					e1.AddField("WHOOPS", "You don't have much coins!\n" + "Current price of token : " + File.ReadAllText(@"database\token\token.txt") + " coins per token")
						.WithAuthor("TOKEN")
						.WithColor(r, g, b)
						.WithFooter("Bot made by kevz#2073");
					await ReplyAsync("", false, e1.Build());
					return;
				}

				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Trade Confirm", "Current price of token : " + File.ReadAllText(@"database\token\token.txt") + " coins per token" +
					"\nYour coins : " + J["coins"] + "\n" +
					"Total payment : " + totalpayment + "\n" +
					"If you want to do it, type \"yes\", if you don't, type \"no\"")
					.WithAuthor("TOKEN")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073");
				await ReplyAsync("", false, e.Build());

				string yesorno = "";
				try
				{
					yesorno = (await NextMessageAsync()).ToString().ToLower();
				}
				catch 
				{
					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("WHOOPS", "Did you sleep? It's been 10 seconds since i asked you..\n" +
						"Type this command again if you want to buy token.")
						.WithAuthor("TOKEN")
						.WithColor(r, g, b)
						.WithFooter("Bot made by kevz#2073");
					await ReplyAsync("", false, ee.Build());
					return;
				}

				if (yesorno == "yes")
				{
					int before = coins(accName(Context.User.Id));
					int currentcoins = coins(accName(Context.User.Id));
					currentcoins -= totalpayment;
					int token = (int) J["token"];
					token += amount;
					var context = accName(Context.User.Id);

					string path = File.ReadAllText(@"database\account\" + context + ".json");

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["token"] = token;
					jsonObj["coins"] = currentcoins;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);


					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("Trade Confirm", "Current price of token : " + File.ReadAllText(@"database\token\token.txt") + " coins per token" +
						"\nYour previous coins : " + before.ToString("#,##0") + "\n" +
						"Total payment : " + totalpayment.ToString("#,##0") + "\n" +
						"**PAYMENT SUCCEED**\n" +
						"Your coins is now " + currentcoins.ToString("#,##0"))
						.WithAuthor("TOKEN")
						.WithColor(r, g, b)
						.WithFooter("Bot made by kevz#2073");
					await ReplyAsync("", false, ee.Build());
				}
				else if (yesorno == "no")
				{
					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("Trade Declined", "Current price of token : " + File.ReadAllText(@"database\token\token.txt") + " coins per token" +
						"Total payment : " + totalpayment.ToString("#,##0") + "\n" +
						"**PAYMENT DECLINED**\n")
						.WithAuthor("TOKEN")
						.WithColor(r, g, b)
						.WithFooter("Bot made by kevz#2073");
					await ReplyAsync("", false, ee.Build());
					return;
				}
				else
				{
					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("Trade Declined", "Current price of token : " + File.ReadAllText(@"database\token\token.txt") + " coins per token" +
						"Total payment : " + totalpayment.ToString("#,##0") + "\n" +
						"Payment Declined by default.\n" +
						"**PAYMENT DECLINED**\n")
						.WithAuthor("TOKEN")
						.WithColor(r, g, b)
						.WithFooter("Bot made by kevz#2073");
					await ReplyAsync("", false, ee.Build());
				}
			}
		}
	}
}
