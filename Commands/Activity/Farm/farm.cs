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

namespace CowboyBot
{
	public class farm : InteractiveBase
	{
		private static ColorList color = new ColorList();
		private int r = color.r;
		private int g = color.g;
		private int b = color.b;
		private DateTimeOffset ok = DateTimeOffset.Now;		
		private int xdlol;
		private DateTimeOffset wow = new DateTimeOffset();

		[Command("farm", RunMode = RunMode.Async)]
		public async Task farmact()
		{
			var user = accName(Context.User.Id);
			//asdasdasdad
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Hello,", "What do you want to do?\n[1] Plant\n[2] Harvest")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor(user + "'s farm");

				await ReplyAsync("",false,embed:e.Build());

				string type = (await NextMessageAsync()).ToString().ToLower();
				if (type == "1" || type == "plant")
				{
					JObject xd = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));
					if ((bool)xd["haveplanted"])
					{
						EmbedBuilder eee = new EmbedBuilder();
						eee.AddField("WHOOPS", "You have already planted a seeds! Try harvest it?")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r, g, b)
						.WithCurrentTimestamp()
						.WithAuthor(user + "'s farm");

						await ReplyAsync("", false, embed: eee.Build());
						return;
					}
					await sendPlant(user);
				}
				else if (type == "2" || type == "harvest")
				{
					await harvestingnew(user);
				}
				else
				{
					await ReplyAndDeleteAsync("Invalid argument!", timeout:TimeSpan.FromSeconds(3));
				}
			}
		}
		public void sendHarvest(string user)
		{

		}

		public async Task sendPlant(string user)
		{
			string result = "";
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));
			result += (int)j["wheat"] > 1 ? "[1] " + j["wheat"] + " wheats\n" : "[1] " + j["wheat"] + " wheat\n";
			result += (int)j["fern"] > 1 ? "[2] " + j["fern"] + " ferns\n" : "[2] " + j["fern"] + " fern\n";
			result += (int)j["corn"] > 1 ? "[3] " + j["corn"] + " corns\n" : "[3] " + j["corn"] + " corn\n";
			result += (int)j["mushroom"] > 1 ? "[4] " + j["mushroom"] + " mushrooms\n" : "[4] " + j["mushroom"] + " mushroom\n";
			result += (int)j["apple"] > 1 ? "[5] " + j["apple"] + " apples" : "[5] " + j["apple"] + " apple";

			EmbedBuilder ee = new EmbedBuilder();
			ee.AddField("Your seeds", "You have :\n" + result + "\nWhat seeds do you want to plant?")
			.WithFooter("Bot made by kevz#2073")
			.WithColor(r, g, b)
			.WithCurrentTimestamp()
			.WithAuthor(user + "'s farm");

			await ReplyAsync("", false, embed: ee.Build());

			string typePlant = (await NextMessageAsync()).ToString();

			await seeds(typePlant);
		}

		public async Task seeds(string t)
		{
			t = t.ToLower();

			if(t == "1" || t.Contains("wheat"))
			{
				var user = accName(Context.User.Id);

				JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));

				if((int)j["wheat"] == 0)
				{
					await ReplyAndDeleteAsync("You don't have much wheat seeds!", timeout: TimeSpan.FromSeconds(3));
					return;
				}

				string asd = (int)j["wheat"] > 1 ? j["wheat"] + " wheats" : j["wheat"] + " wheat";

				EmbedBuilder ee = new EmbedBuilder();
				ee.AddField("Your seeds", "How much wheats do you want to use? (You have " + asd + ")")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor(user + "'s farm");

				await ReplyAsync("", false, embed: ee.Build());

				int xdasdf = Convert.ToInt32((await NextMessageAsync()).Content);

				await seedamount(xdasdf, "wheat");
			}
			else if (t == "2" || t.Contains("fern"))
			{
				var user = accName(Context.User.Id);

				JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));

				if ((int)j["fern"] == 0)
				{
					await ReplyAndDeleteAsync("You don't have much fern seeds!", timeout: TimeSpan.FromSeconds(3));
					return;
				}

				string asd = (int)j["fern"] > 1 ? j["fern"] + " ferns" : j["fern"] + " fern";

				EmbedBuilder ee = new EmbedBuilder();
				ee.AddField("Your seeds", "How much fern do you want to use? (You have " + asd + ")")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor(user + "'s farm");

				await ReplyAsync("", false, embed: ee.Build());

				int xdasdf = Convert.ToInt32((await NextMessageAsync()).Content);

				await seedamount(xdasdf, "fern");
			}
			else if (t == "3"|| t.Contains("corn"))
			{
				var user = accName(Context.User.Id);

				JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));

				if ((int)j["corn"] == 0)
				{
					await ReplyAndDeleteAsync("You don't have much corn seeds!", timeout: TimeSpan.FromSeconds(3));
					return;
				}

				string asd = (int)j["corn"] > 1 ? j["corn"] + " corns" : j["fern"] + " corn";

				EmbedBuilder ee = new EmbedBuilder();
				ee.AddField("Your seeds", "How much corn do you want to use? (You have " + asd + ")")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor(user + "'s farm");

				await ReplyAsync("", false, embed: ee.Build());

				int xdasdf = Convert.ToInt32((await NextMessageAsync()).Content);

				await seedamount(xdasdf, "corn");
			}
			else if (t  == "4" || t.Contains("mushroom"))
			{
				var user = accName(Context.User.Id);

				JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));

				if ((int)j["mushroom"] == 0)
				{
					await ReplyAndDeleteAsync("You don't have much mushroom seeds!", timeout: TimeSpan.FromSeconds(3));
					return;
				}

				string asd = (int)j["mushroom"] > 1 ? j["mushroom"] + " mushrooms" : j["mushroom"] + " mushroom";

				EmbedBuilder ee = new EmbedBuilder();
				ee.AddField("Your seeds", "How much corn do you want to use? (You have " + asd + ")")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor(user + "'s farm");

				await ReplyAsync("", false, embed: ee.Build());

				int xdasdf = Convert.ToInt32((await NextMessageAsync()).Content);

				await seedamount(xdasdf, "mushroom");
			}
			else if (t == "5"|| t.Contains("apple"))
			{
				var user = accName(Context.User.Id);

				JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));

				if ((int)j["apple"] == 0)
				{
					await ReplyAndDeleteAsync("You don't have much apple seeds!", timeout: TimeSpan.FromSeconds(3));
					return;
				}

				string asd = (int)j["apple"] > 1 ? j["apple"] + " apples" : j["apple"] + " apple";

				EmbedBuilder ee = new EmbedBuilder();
				ee.AddField("Your seeds", "How much apple do you want to use? (You have " + asd + ")")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r, g, b)
				.WithCurrentTimestamp()
				.WithAuthor(user + "'s farm");

				await ReplyAsync("", false, embed: ee.Build());

				int xdasdf = Convert.ToInt32((await NextMessageAsync()).Content);

				await seedamount(xdasdf, "apple");
			}
			else
			{
				await ReplyAndDeleteAsync("Invalid type of seeds!",timeout:TimeSpan.FromSeconds(3));
			}
		}
		
		public async Task seedamount(int seeds,string type)
		{
			var user = accName(Context.User.Id);
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));
			if((int)j[type] >= seeds)
			{
				// WORKING
				string path = File.ReadAllText(@"database\account\" + user + ".json");

				dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
				jsonObj["isplanted"] = true;
				jsonObj[type] -= seeds;
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
				File.WriteAllText(@"database\account\" + user + ".json", output);

				//await a.plantDone(user,type,seeds);
				ulong id = Context.Channel.Id;

				await Planting(user, type, seeds, id);

			}
			else
			{
				await ReplyAndDeleteAsync("You only have " + j[type] + " " + type + " seeds!");
			}
		}
		[Command("bypasscooldown")]
		public async Task bypasscooldown()
		{
			if (Context.User.ToString() != "kevz#2073") return;
			string context = accName(Context.User.Id);

			string type = plant[planter.IndexOf(context)];

			int time;
			if (type == "wheat") time = 15;
			else if (type == "fern") time = 20;
			else if (type == "corn") time = 25;
			else if (type == "mushroom") time = 30;
			else time = 35;
			plantTimer[planter.IndexOf(context)] = DateTimeOffset.Now.AddHours(time);
		}

		[Command("edithp")]
		public async Task edithp()
		{
			string context = accName(Context.User.Id);
			if(Context.User.ToString() == "kevz#2073")
			{
				string path = File.ReadAllText(@"database\account\" + context + ".json");

				dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
				jsonObj["haveplanted"] = false;
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
				File.WriteAllText(@"database\account\" + context + ".json", output);
			}
		}

		public async Task Planting(string context, string type, int amount, ulong id)
		{
			plant.Add(type);
			planter.Add(context);
			plantAmount.Add(amount);
			plantTimer.Add(DateTimeOffset.Now);

			await ReplyAsync("You planted " + amount + " " + type + "!");

			string path = File.ReadAllText(@"database\account\" + context + ".json");

			dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
			jsonObj["haveplanted"] = true;
			jsonObj["isplanted"] = false;
			string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
			File.WriteAllText(@"database\account\" + context + ".json", output);

			JObject j = new JObject(
				new JProperty("seeds", type),
				new JProperty("amount", amount),
				new JProperty("context", context),
				new JProperty("time", DateTime.Now),
				new JProperty("bool", false)
				);
			File.WriteAllText(@"database\farming\" + context + ".json", j.ToString());
		}

		public async Task harvesting(string context)
		{
			JObject q = JObject.Parse(File.ReadAllText(@"database\farming\" + context + ".json"));

			string awowlol = context;

			if ((bool) q["bool"] == false)
			{
				string[] a = Directory.GetFiles(@"database\farming");
				foreach (string s in a)
				{
					if (s.Contains(".json"))
					{
						JObject j = JObject.Parse(File.ReadAllText(s));
						plant.Add((string)j["seeds"]);
						planter.Add((string)j["context"]);
						plantAmount.Add((int)j["amount"]);
						plantTimer.Add((DateTimeOffset)j["time"]);
					}
					else continue;
				}

				JObject xd = JObject.Parse(File.ReadAllText(@"database\farming\" + context + ".json"));
				xdlol = (int)(ok - (DateTimeOffset) xd["time"]).TotalSeconds;
				//xdlol = (int)((DateTimeOffset)xd["time"] - ok).TotalSeconds;

				string path = File.ReadAllText(@"database\farming\" + context + ".json");

				dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
				jsonObj["bool"] = true;
				string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
				File.WriteAllText(@"database\farming\" + context + ".json", output);

				wow = DateTimeOffset.Now.AddSeconds(-xdlol);
				await harvesting(awowlol) ;
				return;

				/*if (type == "wheat") time = 15;
				else if (type == "fern") time = 20;
				else if (type == "corn") time = 25;
				else if (type == "mushroom") time = 30;
				else time = 35;*/
			}

			string type = plant[planter.IndexOf(context)];
			int amount = plantAmount[planter.IndexOf(context)];

			int time;
			if (type == "wheat") time = 15;
			else if (type == "fern") time = 20;
			else if (type == "corn") time = 25;
			else if (type == "mushroom") time = 30;
			else time = 35;

			if (planter.Contains(context))
			{
				//If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
				if (plantTimer[planter.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now.AddSeconds(-xdlol))
				{
					int minutesLeft = (int)(plantTimer[planter.IndexOf(context)].AddMinutes(time) - DateTimeOffset.Now.AddSeconds(-xdlol)).TotalMinutes;
					//If enough time hasn't passed, reply letting them know how much longer they need to wait, and end the code.
					int secondsLeft = (int)(plantTimer[planter.IndexOf(context)].AddMinutes(time) - DateTimeOffset.Now.AddSeconds(-xdlol)).TotalSeconds;

					secondsLeft -= (minutesLeft * 60);

					string minutes = minutesLeft + " minutes, ";
					if (minutesLeft == 0) minutes = "";

					await ReplyAsync("You need to wait " + minutes + secondsLeft + " seconds to harvest " + amount + " " + type + "!");
				}
				else
				{
					//If enough time has passed, set the time for the user to right now.
					plantTimer.RemoveAt(planter.IndexOf(context));
					plant.Remove(type);
					planter.Remove(context);
					plantAmount.Remove(amount);

					File.Delete(@"database\farming\" + context + ".json");

					Random r = new Random();

					int profit = r.Next((amount * 3) / 2, (amount * 5) / 2);
					await ReplyAsync("You harvested " + amount + " " + type + " and you got " + (profit - amount) + " seeds profit!");

					string path = File.ReadAllText(@"database\account\" + context + ".json");

					JObject j = JObject.Parse(path);
					int seedBalances = (int)j[type];
					seedBalances += profit;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj[type] = seedBalances;
					jsonObj["haveplanted"] = false;
					jsonObj["isplanted"] = false;

					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);
				}
			}
			else
			{
				await ReplyAndDeleteAsync("There is nothing to be harvested!");
			}
		}

		public async Task newHarvesting(string context)
		{
			string[] a = Directory.GetFiles(@"database\farming");
			foreach (string s in a)
			{
				if (s.Contains(".json"))
				{
					JObject j = JObject.Parse(File.ReadAllText(s));
					plant.Add((string)j["seeds"]);
					planter.Add((string)j["context"]);
					plantAmount.Add((int)j["amount"]);
					plantTimer.Add((DateTime)j["time"]);
				}
				else continue;
			}

			string type = plant[planter.IndexOf(context)];
			int amount = plantAmount[planter.IndexOf(context)];

			int time;
			if (type == "wheat") time = 15;
			else if (type == "fern") time = 20;
			else if (type == "corn") time = 25;
			else if (type == "mushroom") time = 30;
			else time = 35;

			if (planter.Contains(context))
			{
				if (plantTimer2[planter.IndexOf(context)].AddMinutes(time) >= DateTime.Now)
				{
					await ReplyAsync("Your " + amount + " " + type + " will be ready in " + (plantTimer2[planter.IndexOf(context)].AddMinutes(time)).ToLongTimeString());
				}
				else
				{
					plantTimer2.RemoveAt(planter.IndexOf(context));
					plant.Remove(type);
					planter.Remove(context);
					plantAmount.Remove(amount);

					File.Delete(@"database\farming\" + context + ".json");

					Random r = new Random();

					int profit = r.Next((amount * 3) / 2, (amount * 5) / 2);
					await ReplyAsync("You harvested " + amount + " " + type + " and you got " + profit + " seeds profit!");

					string path = File.ReadAllText(@"database\account\" + context + ".json");

					JObject j = JObject.Parse(path);
					int seedBalances = (int)j[type];
					seedBalances += profit;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj[type] = seedBalances;
					jsonObj["haveplanted"] = false;
					jsonObj["isplanted"] = false;

					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);
				}
			}
			else
			{
				await ReplyAndDeleteAsync("There is nothing to be harvested!");
			}
		}

		public async Task harvestingnew (string context)
		{
			string[] a = Directory.GetFiles(@"database\farming");

			foreach (string s in a)
			{
				if (s.Contains(".json"))
				{
					if (File.Exists(s))
					{
						JObject j = JObject.Parse(File.ReadAllText(s));
						if (!planter.Contains(context))
						{
							plant.Add((string)j["seeds"]);
							planter.Add((string)j["context"]);
							plantAmount.Add((int)j["amount"]);
							plantTimer.Add((DateTimeOffset)j["time"]);
						}
						else continue;
					}
					else continue;
				}
				else continue;
			}
			JObject xd = new JObject();
			if(File.Exists(@"database\farming\" + context + ".json"))
				xd = JObject.Parse(File.ReadAllText(@"database\farming\" + context + ".json"));

			string type = "";
			int amount = 0;

			//okay cool

			string path = File.ReadAllText(@"database\account\" + context + ".json");
			try
			{
				if (File.Exists(@"database\farming\" + context + ".json"))
				{
					type = plant[planter.IndexOf(context)];
					amount = plantAmount[planter.IndexOf(context)];
				}
			}
			catch { Console.WriteLine("error in type and amount"); }

			int time;
			if (type == "wheat") time = 15;
			else if (type == "fern") time = 20;
			else if (type == "corn") time = 25;
			else if (type == "mushroom") time = 30;
			else time = 35;

			if(amount > 5) { time *= (amount / 5); }

			if (planter.Contains(context))
			{
				//If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
				if (time >= (DateTimeOffset.Now - (DateTimeOffset) xd["time"]).TotalMinutes)
				{
					int hoursLeft = 0;
					if (time >= 60) { hoursLeft = (int)(time / 60 + 1 - (DateTimeOffset.Now - (DateTimeOffset)xd["time"]).TotalHours); }

					int minutesLeft = (int)(time - (DateTimeOffset.Now - (DateTimeOffset)xd["time"]).TotalMinutes);
					//If enough time hasn't passed, reply letting them know how much longer they need to wait, and end the code.
					//int secondsLeft = (int)((time * 1000) - (DateTimeOffset.Now - (DateTimeOffset)xd["time"]).TotalSeconds);

					int secondsLeft = Convert.ToInt32(-1 * (DateTimeOffset.Now - ((DateTimeOffset)xd["time"]).AddMinutes(time)).TotalSeconds);

					secondsLeft -= (minutesLeft * 60);
					minutesLeft -= (hoursLeft * 60);

					string minutes = minutesLeft + " minutes, ";
					if (minutesLeft == 0) minutes = "";

					string hours = hoursLeft + " hours, ";
					if (hoursLeft == 0) hours = "";

					await ReplyAsync("You need to wait " + hours +  minutes + secondsLeft + " seconds to harvest " + amount + " " + type + "!");
				}
				else
				{
					//If enough time has passed, set the time for the user to right now.
					plantTimer.RemoveAt(planter.IndexOf(context));
					plant.Remove(type);
					planter.Remove(context);
					plantAmount.Remove(amount);

					File.Delete(@"database\farming\" + context + ".json");

					Random r = new Random();

					int profit = r.Next((amount * 3) / 2, (amount * 5) / 2);
					await ReplyAsync("You harvested " + amount + " " + type + " and you got " + profit + " seeds profit!");


					JObject j = JObject.Parse(path);
					int seedBalances = (int)j[type];
					seedBalances += profit;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj[type] = seedBalances;
					jsonObj["haveplanted"] = false;
					jsonObj["isplanted"] = false;

					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);
				}
			}
			else
			{
				await ReplyAndDeleteAsync("There is nothing to be harvested!");
			}
		}
	}
}