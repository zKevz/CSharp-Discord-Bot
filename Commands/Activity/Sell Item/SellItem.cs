using System;
using System.Collections.Generic;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
	public class SellItem : InteractiveBase
	{
		private static ColorList c = new ColorList();
		private readonly int r = c.r;
		private readonly int g = c.g;
		private readonly int b = c.b;

		[Command("sell", RunMode = RunMode.Async)]
		[Alias("sellseed","sellseeds","sellitem","sellitems")]
		public async Task sell([Remainder] string type = null)
		{
			string name = accName(Context.User.Id);
			string path = File.ReadAllText(@"database\account\" + name + ".json");

			if (type == null)
			{
				EmbedBuilder eeeee = new EmbedBuilder();
				eeeee.AddField("Syntax Error", "Syntax : **c sell [item name]**")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor("Having a problem?");

				await ReplyAsync("", false, embed: eeeee.Build());
				return;
			}
			else if (type.Contains(" "))
			{
				EmbedBuilder eeeee = new EmbedBuilder();
				eeeee.AddField("Syntax Error", "Syntax : **c sell [item name]**")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor("Having a problem?");

				await ReplyAsync("", false, embed: eeeee.Build());
				return;
			}

			string t = type.ToLower();

			if (t.Contains("wheat"))
			{
				JObject j = JObject.Parse(path);
				if ((int)j["wheat"] > 0)
				{
					try {
					EmbedBuilder eee = new EmbedBuilder();
					eee.AddField("How much wheat seeds do you want to sell?", "You have " + j["wheat"] + " wheat seeds.\nThe prices is 10 coins per seeds.")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eee.Build());

					int amount = Convert.ToInt32((await NextMessageAsync()).Content);
					if (amount == null) return;
					else if (amount < 1) return;
					if (amount > (int)j["wheat"])
					{
						EmbedBuilder eeee = new EmbedBuilder();
						eeee.AddField("WHOOPS", "You only have " + j["wheat"] + " wheat seeds!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp()
						.WithAuthor(name + "'s trade");

						await ReplyAsync("", false, embed: eeee.Build());
						return;
					}

					int usercoins = coins(name);
					usercoins += (amount * 10);
					int wheatbal = (int)j["wheat"];
					wheatbal -= amount;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["wheat"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + name + ".json", output);

					EmbedBuilder eeeee = new EmbedBuilder();
					eeeee.AddField("Trade Succeed", "You sold " + amount + " wheat seeds for " + amount * 10 + " coins!")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eeeee.Build());
					return; }
					catch {
						return;
					}
				}
				else
				{
					await wow();
				}
			}
			else if (t.Contains("fern"))
			{
				JObject j = JObject.Parse(path);
				if ((int)j["fern"] > 0)
				{
					EmbedBuilder eee = new EmbedBuilder();
					eee.AddField("How much fern seeds do you want to sell?", "You have " + j["fern"] + " fern seeds.\nThe prices is 50 coins per seeds.")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eee.Build());

					try {
					int amount = Convert.ToInt32((await NextMessageAsync()).Content);
					if (amount == null) return;
					else if (amount < 1) return;
					if (amount > (int)j["fern"])
					{
						EmbedBuilder eeee = new EmbedBuilder();
						eeee.AddField("WHOOPS", "You only have " + j["fern"] + " fern seeds!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp()
						.WithAuthor(name + "'s trade");

						await ReplyAsync("", false, embed: eeee.Build());
						return;
					}

					int usercoins = coins(name);
					usercoins += (amount * 50);
					int wheatbal = (int)j["fern"];
					wheatbal -= amount;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["fern"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + name + ".json", output);

					EmbedBuilder eeeee = new EmbedBuilder();
					eeeee.AddField("Trade Succeed", "You sold " + amount + " fern seeds for " + amount * 50 + " coins!")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eeeee.Build());
					return; }
					catch{
						return;
					}
				}
				else
				{
					await wow();
				}
			}
			else if (t.Contains("corn"))
			{
				JObject j = JObject.Parse(path);
				if ((int)j["corn"] > 0)
				{
					EmbedBuilder eee = new EmbedBuilder();
					eee.AddField("How much corn seeds do you want to sell?", "You have " + j["corn"] + " apple seeds.\nThe prices is 100 coins per seeds.")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eee.Build());
					try {
					int amount = Convert.ToInt32((await NextMessageAsync()).Content);
					if (amount == null) return;
					else if (amount < 1) return;
					if (amount > (int)j["corn"])
					{
						EmbedBuilder eeee = new EmbedBuilder();
						eeee.AddField("WHOOPS", "You only have " + j["corn"] + " corn seeds!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp()
						.WithAuthor(name + "'s trade");

						await ReplyAsync("", false, embed: eeee.Build());
						return;
					}

					int usercoins = coins(name);
					usercoins += (amount * 100);
					int wheatbal = (int)j["corn"];
					wheatbal -= amount;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["corn"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + name + ".json", output);

					EmbedBuilder eeeee = new EmbedBuilder();
					eeeee.AddField("Trade Succeed", "You sold " + amount + " corn seeds for " + amount * 100 + " coins!")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eeeee.Build());
					return;
					}
					catch {
						return;
					}
				}
				else
				{
					await wow();
				}
			}
			else if (t.Contains("mushroom"))
			{
				JObject j = JObject.Parse(path);
				if ((int)j["mushroom"] > 0)
				{
					EmbedBuilder eee = new EmbedBuilder();
					eee.AddField("How much mushroom seeds do you want to sell?", "You have " + j["mushroom"] + " mushroom seeds.\nThe prices is 300 coins per seeds.")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eee.Build());
					try
					{

					int amount = Convert.ToInt32((await NextMessageAsync()).Content);
					if (amount == null) return;
					else if (amount < 1) return;
					if (amount > (int)j["mushroom"])
					{
						EmbedBuilder eeee = new EmbedBuilder();
						eeee.AddField("WHOOPS", "You only have " + j["mushroom"] + " mushroom seeds!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp()
						.WithAuthor(name + "'s trade");

						await ReplyAsync("", false, embed: eeee.Build());
						return;
					}

					int usercoins = coins(name);
					usercoins += (amount * 300);
					int wheatbal = (int)j["mushroom"];
					wheatbal -= amount;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = usercoins;
					jsonObj["mushroom"] = wheatbal;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + name + ".json", output);

					EmbedBuilder eeeee = new EmbedBuilder();
					eeeee.AddField("Trade Succeed", "You sold " + amount + " mushroom seeds for " + amount * 300 + " coins!")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eeeee.Build());
					return;
					}
					catch {
						return;
					}
				}
				else
				{
					await wow();
				}
			}
			else if (t.Contains("apple"))
			{
				JObject j = JObject.Parse(path);
				if ((int)j["apple"] > 0)
				{
					EmbedBuilder eee = new EmbedBuilder();
					eee.AddField("How much apple seeds do you want to sell?", "You have " + j["apple"] + " apple seeds.\nThe prices is 700 coins per seeds.")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r, g, b)
					.WithCurrentTimestamp()
					.WithAuthor(name + "'s trade");

					await ReplyAsync("", false, embed: eee.Build());
					try
					{
						int amount = Convert.ToInt32((await NextMessageAsync()).Content);
						if (amount == null) return;
						else if (amount < 1) return;
						if (amount > (int)j["apple"])
						{
							EmbedBuilder eeee = new EmbedBuilder();
							eeee.AddField("WHOOPS", "You only have " + j["apple"] + " apple seeds!")
							.WithFooter("Bot made by kevz#2073")
							.WithColor(r, g, b)
							.WithCurrentTimestamp()
							.WithAuthor(name + "'s trade");

							await ReplyAsync("", false, embed: eeee.Build());
							return;
						}

						int usercoins = coins(name);
						usercoins += (amount * 700);
						int wheatbal = (int)j["apple"];
						wheatbal -= amount;

						dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
						jsonObj["coins"] = usercoins;
						jsonObj["apple"] = wheatbal;
						string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
						File.WriteAllText(@"database\account\" + name + ".json", output);

						EmbedBuilder eeeee = new EmbedBuilder();
						eeeee.AddField("Trade Succeed", "You sold " + amount + " apple seeds for " + amount * 700 + " coins!")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp()
						.WithAuthor(name + "'s trade");

						await ReplyAsync("", false, embed: eeeee.Build());
						return;
					}
					catch
					{
						return;
					}
				}
				else
				{
					await wow();
				}
			}
			else
			{
				await ReplyAndDeleteAsync("Invalid type of seeds.",timeout:TimeSpan.FromSeconds(3));
			}
		}

		public async Task wow()
		{
			await ReplyAndDeleteAsync("You don't even have that seed!", timeout: TimeSpan.FromSeconds(3));
		}
	}
}
