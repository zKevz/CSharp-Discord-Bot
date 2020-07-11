using System;
using System.Collections.Generic;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CowboyBot.Timer;

namespace CowboyBot
{
	public class Shop : InteractiveBase
	{
		private static ColorList c = new ColorList();
		private int r = c.r;
		private int g = c.g;
		private int b = c.b;

		[Command("shop")]
		[Alias("shops","store","market")]
		public async Task shop()
		{
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("__SHOP__", "\n\nList of shop: \n" + listItem())
				.WithAuthor("Cowboy's Shop")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}

		public string listItem()
		{
			return "**[1]** Wheat seeds -> 20 Coins each\n" +
				"**[2]** Fern Seeds -> 75 Coins each\n" +
				"**[3]** Corn Seeds -> 150 Coins each\n" +
				"**[4]** Mushroom Seeds -> 400 Coins each\n" +
				"**[5]** Apple Seeds -> 1.000 Coins each\n" +
				"**[6]** Knife -> 14.000 Coins\n" +
				"**[7]** Shotgun -> 60.000 Coins\n" +
				"**[8]** Leather Shirt -> 12.000 Coins\n" +
				"**[9]** Bulletproof Shirt -> 100.000 Coins\n" +
				"**[10]** Chicken -> 500 Coins each\n" +
				"**[11]** Cow -> 1500 Coins each\n" +
				"**[12]** Golden Rod -> 10.000 Coins\n" +
				"**[13]** Marvelous Rainbow Rod -> 50.000 Coins\n" +
				"**[14]** Cowboy's VIP -> 3 Real GT Diamond Locks\n" +
				"**[15]** Cowboy Sprayer -> 500 Coins\n\n";
		}

		[Command("buy", RunMode = RunMode.Async)]
		public async Task buy(string type = null, int amount = -16942069)
		{
			var context = accName(Context.User.Id);

			if (type == null || amount == -16942069)
			{
				EmbedBuilder exd = new EmbedBuilder();
				exd.WithTitle("Having a problem?")
					.AddField("Syntax : **c buy [items] [amount]**\nDo \"c shop\" to see all the items" +
					" in shop!", "#CowboyBot")
					.WithFooter("Bot made by kevz#2073").WithColor(r, g, b);
				await ReplyAsync("", false, exd.Build());
				return;
			}

			string t = type.ToLower();
			string a = amount.ToString();

			bool onlyDigit = a.All(char.IsDigit);

			if (!onlyDigit)
			{

				EmbedBuilder exd = new EmbedBuilder();
				exd.WithTitle("Put only number!")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b);

				await ReplyAsync("", false, exd.Build());
				return;
			}

			if (t.Contains("wheat") || t == "1")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				int xd = asd * 20;
				if(coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);
					int wheatbal = (int)j["wheat"];
					wheatbal += asd;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["wheat"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought " + asd + " wheat seeds for " + xd + " coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("fern") || t == "2")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				int xd = asd * 75;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);
					int wheatbal = (int)j["fern"];
					wheatbal += asd;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["fern"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought " + asd + " fern seeds for " + xd + " coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("corn") || t == "3")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				int xd = asd * 150;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);
					int wheatbal = (int)j["corn"];
					wheatbal += asd;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["corn"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought " + asd + " corn seeds for " + xd + " coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("mushroom") || t == "4")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				int xd = asd * 400;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);
					int wheatbal = (int)j["mushroom"];
					wheatbal += asd;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["mushroom"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought " + asd + " mushroom seeds for " + xd + " coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("apple") || t == "5")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				int xd = asd * 1000;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);
					int wheatbal = (int)j["apple"];
					wheatbal += asd;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["apple"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought " + asd + " apple seeds for " + xd + " coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("knife") || t == "6")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				if (asd > 1)
				{
					await ReplyAsync("You can only buy 1 knife!");
					return;
				}

				int xd = asd * 14000;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["player_hand"] = 2;
					jsonObj["coins"] = usercoins;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought knife for 14.000 coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("shotgun") || t == "7")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				if(asd > 1)
				{
					await ReplyAsync("You can only buy 1 shotgun!");
					return;
				}

				int xd = asd * 60000;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["player_hand"] = 3;
					jsonObj["coins"] = usercoins;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought shotgun for 60.000 coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("leather") || t == "8")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				if (asd > 1)
				{
					await ReplyAsync("You can only buy 1 leather shirt!");
					return;
				}

				int xd = asd * 12000;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["player_body"] = 2;
					jsonObj["coins"] = usercoins;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought leather shirt for 12.000 coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("bulletproof") || t == "9")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				if (asd > 1)
				{
					await ReplyAsync("You can only buy 1 bulletproof shirt!");
					return;
				}

				int xd = asd * 100000;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["player_body"] = 3;
					jsonObj["coins"] = usercoins;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought bulletproof shirt for 100.000 coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("chick") || t == "10"){
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;
				
				int xd = asd * 500;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{
					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj =JsonConvert.DeserializeObject(path);
					dynamic jok = JsonConvert.DeserializeObject(File.ReadAllText(@"database\inventory\" + context + ".json"));
					jok["chicken"] += asd;
					jsonObj["coins"] = usercoins;
					jok["chickentime"] = DateTimeOffset.Now;
					string u = JsonConvert.SerializeObject(jok, Formatting.Indented);
					string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);
					File.WriteAllText(@"database\inventory\" + context + ".json",u);

					chickenTarget.Add(context);
					chickenTimer.Add(DateTimeOffset.Now);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought "+ asd +" chickens for " + xd + " coins")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("cow") || t == "11")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				int xd = asd * 1500;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{
					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj = JsonConvert.DeserializeObject(path);
					dynamic jok = JsonConvert.DeserializeObject(File.ReadAllText(@"database\inventory\" + context + ".json"));
					jok["cow"] += asd;
					jsonObj["coins"] = usercoins;
					jok["cowtime"] = DateTimeOffset.Now;
					string u = JsonConvert.SerializeObject(jok, Formatting.Indented);
					string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);
					File.WriteAllText(@"database\inventory\" + context + ".json", u);

					cowTarget.Add(context);
					cowTimer.Add(DateTimeOffset.Now);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought " + asd + " cows for " + xd + " coins")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("golden") || t == "12")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				if (asd > 1)
				{
					await ReplyAsync("You can only buy 1 Golden Rod!");
					return;
				}

				int xd = asd * 10000;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["player_rod"] = 2;
					jsonObj["coins"] = usercoins;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought Golden Rod for 10.000 coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if(t.Contains("rainbow") || t == "13")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				int asd = amount;

				if (asd > 1)
				{
					await ReplyAsync("You can only buy 1 Marvelous Rainbow Rod!");
					return;
				}

				int xd = asd * 50000;
				if (coins(context) < xd && coins(context) != xd)
				{
					EmbedBuilder exdd = new EmbedBuilder();
					exdd.WithTitle("Whoops!")
						.AddField("Sorry!", "You don't have much coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b);

					await ReplyAsync("", false, exdd.Build());
					return;
				}
				else
				{

					int usercoins = coins(context);
					usercoins -= xd;
					JObject j = JObject.Parse(path);

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["player_rod"] = 3;
					jsonObj["coins"] = usercoins;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Transactions Completed", "\nYou bought Marvelous Rainbow Rod for 50.000 coins!")
						.WithAuthor("Cowboy's Shop")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
			else if (t.Contains("vip") || t == "14")
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Cowboy's VIP", "\nDM kevz#2073 to buy this VIP privilege!")
					.WithAuthor("Cowboy's Shop")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (t.ToLower().Contains("sprayer") || t == "15")
			{
				int xd = amount * 500;

				if (amount <= 0)
				{
					EmbedBuilder exd = new EmbedBuilder();
					exd.AddField("WHOOPS", "Sorry, you can't buy lower " +
						"than 1 item.")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor("Cowboy's Shop")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();

					await ReplyAsync("", false, exd.Build());
					return;
				}
				else if (coins(context) < xd && coins(context)!=xd)
				{
					EmbedBuilder exd = new EmbedBuilder();
					exd.AddField("WHOOPS", "Sorry, you don't have enough coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor("NO COINS")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();

					await ReplyAsync("", false, exd.Build());
					return;
				}
				else
				{
					EmbedBuilder exd = new EmbedBuilder();
					exd.AddField("YEY", "You bought " + 
						amount + " cowboy sprayer for " + 
						xd + " coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithAuthor("NO COINS")
						.WithColor(r, g, b)
						.WithCurrentTimestamp();

					await ReplyAsync("", false, exd.Build());

					dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
						@"database\account\" + context + ".json"));
					j["coins"] -= xd;
					j["sprayer"] += amount;
					string output = JsonConvert.SerializeObject(j,
						Formatting.Indented);
					File.WriteAllText(@"database\account\" + context +
						".json", output);

					return;
				}
			}
			else
			{
				EmbedBuilder exd = new EmbedBuilder();
				exd.WithTitle("Sorry, there is no item matched with this. Try" +
					" using number instead.")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b);

				await ReplyAsync("", false, exd.Build());
				return;
			}
		}
	}
}
