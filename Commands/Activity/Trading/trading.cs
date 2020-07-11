using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class trading : InteractiveBase
	{
		private static ColorList c = new ColorList();
		private readonly int r = c.r;
		private readonly int g = c.g;
		private readonly int b = c.b;

		[Command("trade", RunMode = RunMode.Async)]
		[Alias("tradetoken", "selltoken")]
		public async Task trade(IGuildUser user = null, int amount = 0, int price = 0)
		{
			bool isdigit = price.ToString().All(char.IsDigit);

			JObject asd = JObject.Parse(File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json"));
			JObject asd1 = JObject.Parse(File.ReadAllText(@"database\account\" + accName(user.Id) + ".json"));

			if (user == null)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "\n\nSyntax : \"c trade [user] [token amount] [price (coin per token)]\"")
					.WithAuthor("Having a problem?")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (user == Context.User)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "You can't trade yourself!")
					.WithAuthor("Having a problem?")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (!isdigit && amount.ToString().All(char.IsDigit))
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "You can only put numbers in amout of items or the price!")
					.WithAuthor("Having a problem?")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if (accName(user.Id) == null || accName(user.Id) == "")
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("No Account.", "That user you chose doesn't even have a cowboy account!")
					.WithAuthor("WHOOPS")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if ((int) asd["token"] < amount && (int) asd["token"] != amount)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("No Token", "You don't have " + amount + " tokens!")
					.WithAuthor("WHOOPS")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else if ((int)asd1["coins"] < price && (int)asd1["coins"] != price)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("No Token", "The person you chose doesn't have " + price + " coins!")
					.WithAuthor("WHOOPS")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}
			else
			{
				var context = accName(Context.User.Id);

				string path = File.ReadAllText(@"database\trading\" + context + ".json");

				dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
				jsonObj["lastpersontrade"] = accName(user.Id);
				jsonObj["lastamounttrade"] = amount;
				jsonObj["lastpricetrade"] = price;
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
				File.WriteAllText(@"database\trading\" + context + ".json", output);

				await user.GetOrCreateDMChannelAsync();
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Trade Confirm", "User " + Context.User.Username + " with name account " + accName(Context.User.Id) + " is selling " + amount + " tokens for " + price + " coins!\n\n" +
					"Type \"c accept " + accName(Context.User.Id) + "\"if you accept that offer. If you don't, just ignore this messages.")
					.WithAuthor("Trading")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();
				await user.SendMessageAsync("", false, e.Build());
			}
		}

		[Command("accept", RunMode = RunMode.Async)]
		public async Task accept(string username)
		{
			JObject j = JObject.Parse(File.ReadAllText(@"database\trading\" + username + ".json"));
			JObject j1 = JObject.Parse(File.ReadAllText(@"database\account\" + accName(Context.User.Id) + ".json"));

			bool found = false;
			ulong targetid = 0;

			string[] lop = Directory.GetFiles(@"database\isonaccount");
			foreach (string s in lop)
			{
				if (File.ReadAllText(s).ToLower() == username.ToLower())
				{
					found = true;
					targetid = Convert.ToUInt64(s.Substring(21, s.IndexOf(".") - 21));
				}
			}

			if (!found)
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "Account name not found! Try again?")
					.WithAuthor("No Account Found")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
				return;
			}

			if ((string)j["lastpersontrade"] == accName(Context.User.Id))
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Accept Trade", "Are you sure you want to do it?\n" +
					"That person sells " + j["lastamounttrade"] + " token for " + j["lastpricetrade"] + " coins per token.\n" +
					"If you wanna do it, type \"accept\", if you don't, type \"decline\"")
					.WithAuthor("Trade Confirm")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();

				await ReplyAsync("", false, e.Build());

				string answer = (await NextMessageAsync()).ToString().ToLower();
				if(answer == "accept")
				{
					var context = accName(Context.User.Id);

					JObject j2 = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));

					string path = File.ReadAllText(@"database\account\" + context + ".json");
					int currentusercoins = (int)j1["coins"];
					int currentusertoken = (int)j1["token"];

					currentusertoken += (int) j["lastamounttrade"];
					currentusercoins -= (int) j["lastpricetrade"];

					int targetusercoins = (int) j2["coins"];
					int targetusertoken = (int) j2["token"];

					Console.WriteLine(targetusercoins + " "+ targetusertoken);

					targetusercoins += (int) j["lastpricetrade"];
					targetusertoken -= (int) j["lastamounttrade"];

					Console.WriteLine(targetusercoins + " " + targetusertoken);

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);

					jsonObj["coins"] = currentusercoins;
					jsonObj["token"] = currentusertoken;

					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);

					string pathasd = File.ReadAllText(@"database\trading\" + username + ".json");

					dynamic jsonObjasd = Newtonsoft.Json.JsonConvert.DeserializeObject(pathasd);
					jsonObjasd["lastpersontrade"] = "";
					jsonObjasd["lastamounttrade"] = 0;
					jsonObjasd["lastpricetrade"] = 0;

					string outputasd = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObjasd, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\trading\" + username + ".json", outputasd);

					dynamic jj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + username + ".json"));

					Console.WriteLine(targetusercoins + " " + targetusertoken);

					jj["coins"] = targetusercoins;
					jj["token"] = targetusertoken;

					Console.WriteLine(targetusercoins + " " + targetusertoken);

					string outp = Newtonsoft.Json.JsonConvert.SerializeObject(jj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + username + ".json", outp);

					EmbedBuilder ee = new EmbedBuilder();
					ee.AddField("Trade Succeed", "User " + accName(Context.User.Id) + " with account name " + accName(Context.User.Id) + " accepted your trade!\n" +
						"Amount of token : " + j["lastamounttrade"] + "\n" +
						"Price per token : " + j["lastpricetrade"] + "\n\n")
						.WithAuthor("Suceed")
						.WithColor(r, g, b)
						.WithFooter("Bot made by kevz#2073")
											.WithCurrentTimestamp();


					EmbedBuilder eeee = new EmbedBuilder();
					eeee.AddField("Trade Succeed", "You accepted the offer!\n" +
						"Amount of token : " + j["lastamounttrade"] + "\n" +
						"Price per token : " + j["lastpricetrade"] + "\n\n")
						.WithAuthor("Suceed")
						.WithColor(r, g, b)
						.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();

					await ReplyAsync("", false, eeee.Build());

					IGuildUser u = null;

					foreach(var a in Context.Guild.Users)
					{
						if(a.Id == targetid)
						{
							u = a;
						}
					}

					await u.SendMessageAsync("",false,ee.Build());
				}
				else
				{

				}
			}
			else
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "You weren't trading with that person!")
					.WithAuthor("No Trade History Found")
					.WithColor(r, g, b)
					.WithFooter("Bot made by kevz#2073").WithCurrentTimestamp();

				await ReplyAsync("", false, e.Build());
				return;
			}
			
		}
	}
}
