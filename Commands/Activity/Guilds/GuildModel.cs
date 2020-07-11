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
using System.Net;

namespace CowboyBot
{
	public class GuildModel : InteractiveBase
	{
		[Command("createguild", RunMode = RunMode.Async)]
		public async Task CreateGuild()
		{
			Random r = new Random();

			string username = accName(Context.User.Id);
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));

			if ((int)j["coins"] < 5000)
			{
				int moneyLeft = 5000 - (int)j["coins"];
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("WHOOPS", "You need " + moneyLeft + " coins more!\n" +
					"The price is 5000 coins to make guild!")
					.WithAuthor("Guild Creation")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
			}
			else
			{
				await Context.User.SendMessageAsync("What Guild Name would" +
					" you like to use?");

				string FrenzyVampIsPro = (await NextMessageAsync(true, false)).ToString();

				if (File.Exists(@"database\guilds\" + FrenzyVampIsPro + ".json"))
				{
					await Context.User.SendMessageAsync("That guild name has already exists!");
					return;
					// LETS TRY ITTTTTTTTTTTTTTTTTTTTT
				}

				await Context.User.SendMessageAsync("Guild Description?");

				string FrenzyVampKing = (await NextMessageAsync(true, false)).ToString();

				await Context.User.SendMessageAsync("Are you sure you want to create guild with :\n" +
					"Name : " + FrenzyVampIsPro + "\n" +
					"Description : " + FrenzyVampKing + "\n" +
					"This will costs you **5000** coins!\n" +
					"Type \"yes\" if you want to create!");

				string YesOrNo = (await NextMessageAsync(true, false)).ToString();
				if (YesOrNo.ToLower() == "yes" || YesOrNo.ToLower()=="accept")
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Guild Created", "You created Guild with :" +
						"\nName : " + FrenzyVampIsPro + "\n" +
						"Description : " + FrenzyVampKing + "\n\n")
						.WithAuthor("Guild Creation")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithCurrentTimestamp();
					await Context.User.SendMessageAsync("", false, e.Build());

					dynamic FrenzyFrenzyXDDD = JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + username + ".json"));
					FrenzyFrenzyXDDD["coins"] -= 5000;
					string output = JsonConvert.SerializeObject(FrenzyFrenzyXDDD, Formatting.Indented);
					File.WriteAllText(@"database\account\" + username + ".json", output);

					// LETS ADD THE GUILD DATABASE!

					CreateGuildDatabase(FrenzyVampIsPro, FrenzyVampKing, username, Context.User.Id,Context.User);
				}
				else
				{
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("Guild Creation Failed", "You cancelled the creation of Guild!")
						.WithAuthor("Guild Creation")
						.WithFooter("Bot made by kevz#2073")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithCurrentTimestamp();
					await Context.User.SendMessageAsync("", false, e.Build());
				}
			}
		}

		[Command("myguild")]
		public async Task MyGuild()
		{
			bool found = false;

			string guildName = "";
			string guildDescription = "";
			string guildOwner = "";
			int bossHealth = 0;
			int bossLevel = 0;
			string member = "";

			string[] a = Directory.GetFiles(@"database\guilds");
			foreach(string s in a)
			{
				JObject j = JObject.Parse(File.ReadAllText(s));
				List<string> asd = new List<string>();
				for(int i = 0; i < j["member"].ToList().Count; i++)
				{
					asd.Add((string)j["member"][i]);
				}

				for(int q =0;q<asd.Count;q++) 
				{
					if (asd[q].Contains(accName(Context.User.Id)))
					{
						found = true;
						guildName = (string)j["guildname"];
						guildDescription = (string)j["guilddescription"];
						guildOwner = (string)j["guildowner"];
						bossHealth = (int)j["bosshealth"];
						bossLevel = (int)j["bosslevel"];

						for(int i = 0; i < j["member"].ToList().Count; i++)
						{
							member += "- " + j["member"][i] + "\n";
						}
						break;
					}
				}
			}

			if (!found)
			{
				await ReplyAsync("You are not in anyone guild!");
				return;
			}
			else
			{
				Random r = new Random();
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("GUILD INFO", "" +
					"\nGuild Name : " + guildName + "\n" +
					"Guild Description : " + guildDescription + "\n" +
					"Guild Owner : " + guildOwner + "\n" +
					"Member : " + member + 
					"Boss Health : " + bossHealth + "\n" +
					"Boss Level : " + bossLevel)
					.WithAuthor(guildName + "'s Info")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
				await ReplyAsync("", false, e.Build());
			}
		}

		[Command("guildinfo")]
		public async Task GuildInfo(string WongzyRETARDED = null)
		{
			if (WongzyRETARDED == null)
			{

				bool foundIdiot = false;
				string[] wongzyIdiot = Directory.GetFiles(@"database\guilds");

				string path = "";

				foreach (var a in wongzyIdiot)
				{
					JObject j = JObject.Parse(File.ReadAllText(a));
					if ((string)j["guildowner"] == accName(Context.User.Id) || (ulong)j["ownerid"] == Context.User.Id)
					{
						foundIdiot = true;
						path = a;
					}
				}

				if (!foundIdiot)
				{
					await ReplyAsync("You don't even have a guild IDIOT RETARD");
					return;
				}
				else
				{
					JObject j = JObject.Parse(File.ReadAllText(path));

					string member = "";

					for (int i = 0; i < j["member"].ToList().Count; i++)
					{
						member += "- " + j["member"][i] + "\n";
					}

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("GUILD INFO", "\n\nName : " + j["guildname"] + "\n" +
						"Description : " + j["guilddescription"] + "\n" +
						"Owner account : " + j["guildowner"] + "\n" + 
						"Member : \n" + member + "\n" +
						"Boss Health : " + j["bosshealth"] + "\n" +
						"Boss Level : " + j["bosslevel"])
						.WithAuthor("Guild")
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
				}
			}
			else
			{
				string[] wongzyhahaidiot = Directory.GetFiles(@"database\guilds");
				bool frenzyvamppro = false;
				foreach (var a in wongzyhahaidiot)
				{
					Console.WriteLine(a.Substring(16, a.IndexOf(".") - 16));
					if (a.Substring(16, a.IndexOf(".") - 16) == WongzyRETARDED) ;
					{
						frenzyvamppro = true;
					}
				}

				if (!frenzyvamppro)
				{
					await ReplyAsync("There is no guild with name " + WongzyRETARDED);
					return;
				}

				else
				{
					JObject j = JObject.Parse(File.ReadAllText(@"database\guilds\" + WongzyRETARDED + ".json"));

					string member = "";

					for(int i = 0; i < j["member"].ToList().Count; i++)
					{
						member += "- " + j["member"][i] + "\n";
					}

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("GUILD INFO", "\n\nName : " + j["guildname"] + "\n" +
						"Description : " + j["guilddescription"] + "\n" +
						"Owner account : " + j["guildowner"] + "\n" +
						"Member : \n" + member + "\n" +
						"Boss Health : " + j["bosshealth"] + "\n" +
						"Boss Level : " +j["bosslevel"])
						.WithAuthor("Guild")
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					return;
				}
			}
		}

		[Command("invite",RunMode = RunMode.Async)]
		public async Task Invite(IGuildUser user)
		{
			string username = accName(Context.User.Id);

			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));

			if ((string)j["guild"] != "none")
			{

				if (!isOnAccount(user.Id))
				{
					await ReplyAsync("That user doesn't have cowboy account!");
				}
				else if (user == Context.User)
				{
					await ReplyAsync("You can't invite yourself!");
				}
				else
				{
					await ReplyAsync("Sending request...");
					await user.SendMessageAsync("User " + Context.User + " invited you to his guild!\n" +
						"Name : " + j["guild"] + "\nType \"accept\" if u want to join!\nYou have 30 seconds.");

					EnsureFromUserCriterion easd = new EnsureFromUserCriterion(user);

					string answer = "";
					try
					{
						answer = (await NextMessageAsync(easd, TimeSpan.FromSeconds(30))).ToString();
					}
					catch
					{
						await user.SendMessageAsync("You rejected his request!");
						await Context.User.SendMessageAsync("User " + user + " rejected your request.");
						return;
					}

					if (answer.ToLower() == "accept")
					{
						try
						{
							JObject a = new JObject();
							try
							{
								Console.WriteLine("lol");
								Console.WriteLine("database\\guilds\\" + j["guild"] + ".json");
								a = JObject.Parse(File.ReadAllText("database\\guilds\\" + j["guild"] + ".json"));
							}
							catch { Console.WriteLine("ERRO AT PART 1"); }
							try
							{
								JObject retard = JObject.Parse(File.ReadAllText("database\\guilds\\" + j["guild"] + ".json"));
								dynamic k = JsonConvert.DeserializeObject(File.ReadAllText("database\\guilds\\" + j["guild"] + ".json"));

								JArray myArray = (JArray)retard["member"];
								myArray.Add(accName(user.Id) + " [" + user + "]");

								File.WriteAllText("database\\guilds\\" + j["guild"] + ".json", retard.ToString());
								await user.SendMessageAsync("You accepted his request!");
								await Context.User.SendMessageAsync("User " + user + " accepted ur request.");
							}
							catch { Console.WriteLine("error part 2"); }
						}
						catch { Console.WriteLine("error"); }
					}
					else
					{
						await user.SendMessageAsync("You rejected his request!");
						await Context.User.SendMessageAsync("User " + user + " rejected your request.");
						return;
					}
				}
			}
			else
			{
				await ReplyAsync("You don't even have a guild!");
			}
		}

		public void CreateGuildDatabase(string guildName, string description, string owner, ulong userid, SocketUser user)
		{
			string[] member = { owner + " [" + user + "]" };

			JObject j = new JObject(
				new JProperty("guildname", guildName),
				new JProperty("guilddescription", description),
				new JProperty("guildowner", owner),
				new JProperty("ownerid", userid),
				new JProperty("member", member),
				new JProperty("bosshealth",10000),
				new JProperty("bosslevel",1),
				new JProperty("isbattle",false)
				);
			File.WriteAllText(@"database\guilds\" + guildName + ".json", j.ToString());

			dynamic ui = JsonConvert.DeserializeObject(File.ReadAllText(
				@"database\account\" + owner + ".json"));
			ui["guild"] = guildName;
			string output = JsonConvert.SerializeObject(ui, Formatting.Indented);
			File.WriteAllText(@"database\account\" + owner + ".json",output);
		}
	}
}