using System;
using System.Collections.Generic;
using System.Text;
using Discord.Addons.Interactive;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
	public class UserInfoCommands : InteractiveBase
	{
		private static ColorList color = new ColorList();
		private int r = color.r;
		private int g = color.g;
		private int b = color.b;

		[Command("enableaap",RunMode = RunMode.Async)]
		public async Task EnableAAP()
		{
			string username = accName(Context.User.Id);
			if (Context.Channel.Name != "@" + Context.User.ToString())
			{
				await ReplyAndDeleteAsync("Please do the command in my DM!\n" +
					"[FOR SECURITY]");
				return;
			}

			if (File.Exists(@"database\aap\" + username + ".txt"))
			{
				await Context.User.SendMessageAsync("You have already enabled AAP!");
				return;
			}
			else
			{
				while (true)
				{
					await Context.User.SendMessageAsync("What pin do you " +
						"want to set? (4 - 8 characters IN NUMBER)");
					try
					{
						int x;
						if (int.TryParse((await NextMessageAsync(true, false,
							TimeSpan.FromSeconds(30))).Content, out x))
						{
							if (x.ToString().Length > 3 && x.ToString().Length < 9)
							{
								await ReplyAndDeleteAsync("You set your pin " +
									"to " + x + "! Make sure to always remember it.\n" +
									"[THIS MESSAGE WILL BE DELETED IN 30 SECONDS]",
									false, null, TimeSpan.FromSeconds(30));
								File.WriteAllText(@"database\aap\" + username + ".txt", x.ToString());
								break;
							}
							else
							{
								await Context.User.SendMessageAsync("Only 4 to 8 " +
									"characters long!");
							}
						}
						else
						{
							await Context.User.SendMessageAsync("Please type only number!");
						}
					}
					catch
					{
						await Context.User.SendMessageAsync("You didn't asnwer!");
					}
				}
			}
		}
		[Command("disableaap",RunMode = RunMode.Async)]
		public async Task DisableAAP()
		{
			if(Context.Channel.Name != "@"+ Context.User.ToString())
			{
				await ReplyAndDeleteAsync("Please do this command in my DM!\n[FOR SECURITY]",false,null, TimeSpan.FromSeconds(10));
				return;
			}

			string username = accName(Context.User.Id);
			if (!File.Exists(@"database\aap\" + username + ".txt"))
			{
				await Context.User.SendMessageAsync("You don't even enable AAP!");
				return;
			}
			else
			{
				await Context.User.SendMessageAsync("To confirm, please type " +
					"your AAP pin.");
				try
				{
					string pin = (await NextMessageAsync(
						true, false, TimeSpan.FromSeconds(30))).ToString();
					if (File.ReadAllText(@"database\aap\" + username + ".txt")
						== pin)
					{
						await Context.User.SendMessageAsync("You disabled AAP.");
						File.Delete(@"database\aap\" + username + ".txt");
						return;
					}
					else
					{
						await Context.User.SendMessageAsync("Wrong PIN! Cancelling..");
						return;
					}
				}
				catch
				{
					await Context.User.SendMessageAsync("You didn't answer. " +
						"Cancelling..");
					return;
				}
			}
		}

		[Command("givecoinsto")]
		public async Task givecoins(IGuildUser user, int amount)
		{
			string context = accName(user.Id);
			if (Context.User.ToString()== "kevz#2073")
			{
				if (File.Exists(@"database\account\" + context + ".json"))
				{
					string path = File.ReadAllText(@"database\account\" + context + ".json");

					JObject j = JObject.Parse(path);
					int coins = (int)j["coins"];
					coins += amount;

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] = coins;

					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + context + ".json", output);
					await ReplyAsync(amount + " is added to " + user + " account balances.");
				}
				else
				{
					await ReplyAsync("Player not found.");
				}
			}
		}


		[Command("info")]
		public async Task info()
		{
			var userDiscord = Context.User;

			bool a = isOnAccount(userDiscord.Id);
			if (a)
			{
				string name = "";
				try
				{
					name = accName((Context.User.Id));
				}
				catch { Console.WriteLine("accName error"); }
				int coin = coins(accName(userDiscord.Id));

				JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + name + ".json"));


				string pr = ""; try { pr = getClothesName("pr", player_rod(name));  } catch { Console.WriteLine("error at player rod"); }
				string ph = ""; try { ph = getClothesName("ph", player_hand(name)); } catch { Console.WriteLine("error at player_hand(name)"); }
				string pb = ""; try { pb = getClothesName("pb", player_body(name)); } catch { Console.WriteLine("error at player_body(name)"); }

				EmbedBuilder e = new EmbedBuilder();
				e.AddField("ACCOUNT NAME: " + name,
					"\nCoins: " + coin +
					"\nHand type: " + ph +
					"\nBody type: " + pb +
					"\nRod type: " + pr +
					"\nWheat seed: " + j["wheat"] +
					"\nFern seed: " + j["fern"] +
					"\nCorn seed: " + j["corn"] +
					"\nMushroom seed: " + j["mushroom"] +
					"\nApple seed: " + j["apple"] +
					"\nToken: " + j["token"] +
					"\nSmall Fish: " + j["smallfish"] + "" +
					"\nMedium Fish: " + j["mediumfish"] + "" +
					"\nBig Fish: " + j["bigfish"] + "" +
					"\nShark: " + j["shark"] + "" +
					"\nJob: " + j["job"] + "" +
					"\nGuild: " + j["guild"] + "" + 
					"\nCowboy Sprayer: " + j["sprayer"] + "" + 
					"\nBounty Points: " + j["bounty"])
				.WithAuthor("INFO") 
				.WithFooter("Bot created by kevz#2073")
				.WithCurrentTimestamp();
				await Context.User.SendMessageAsync("", false, embed:e.Build());
			}
			else
			{
				await ReplyAsync("You are not in an account right now!\ntype \"c register\" to register!");
			}
		}

		[Command("deleteaccount")]
		public async Task delete(string n)
		{
			if(Context.User.Username == "kevz#2073")
			{
				File.Delete(@"database\account\" + n + ".json");
				File.Delete(@"database\plants\" + n + ".json");
				await ReplyAsync("Account deleted.");
			}
		}
	}
}