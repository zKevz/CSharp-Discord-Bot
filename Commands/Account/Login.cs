using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class Login : InteractiveBase
	{
		private static ColorList color = new ColorList();
		private int r = color.r;
		private int g = color.g;
		private int b = color.b;

		[Command("login", RunMode = RunMode.Async)]
		public async Task login()
		{
			EmbedBuilder usernamee = new EmbedBuilder();
			usernamee.WithTitle("Username : ").WithColor(r,g,b);

			await Context.User.SendMessageAsync("", false, embed: usernamee.Build());
			string username = (await NextMessageAsync(true,false)).ToString().ToLower();

			string[] userList = Directory.GetFiles(@"database\account");

			bool isOnList = false;
			foreach(string oklol in userList)
			{
				if (@"database\account\" + username + ".json".ToLower() == oklol.ToLower()) isOnList = true;
			}

			if (!isOnList) { await Context.User.SendMessageAsync("That username isn't even exist!"); }
			else
			{
				JObject jlmaoxd = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));

				if ((ulong)jlmaoxd["currentuser"] == Context.User.Id)
				{
					await Context.User.SendMessageAsync("You already logged in to this account!");
					return;
				}
				EmbedBuilder passwordd = new EmbedBuilder();
				passwordd.WithTitle("Password : ").WithColor(r, g, b);

				await Context.User.SendMessageAsync("",false,embed:passwordd.Build());
				string password = (await NextMessageAsync(true, false)).ToString();

				JObject jj = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));
				if((string) jj["password"] != password)
				{
					await Context.User.SendMessageAsync("Wrong password!");
				}
				else
				{
					if(File.Exists(@"database\aap\" + username + ".txt"))
					{
						EmbedBuilder e = new EmbedBuilder();
						e.AddField("ACCOUNT ADVANCED PROTECTION", "This account has " +
							"AAP! Please type your pin to proceed.")
							.WithTitle("AAP")
							.WithColor(r, g, b);
						await Context.User.SendMessageAsync("", false, e.Build());
						try
						{
							string aap = (await NextMessageAsync(
								true, false, TimeSpan.FromSeconds(30))).ToString();
							string ah = File.ReadAllText(@"database\aap\" + username + ".txt");
							if(ah != aap)
							{
								EmbedBuilder eee = new EmbedBuilder();
								eee.AddField("ACCOUNT ADVANCED PROTECTION", "WRONG AAP! You have been disconnected" +
									" from this account.")
									.WithTitle("AAP")
									.WithColor(r, g, b);
								await Context.User.SendMessageAsync("", false, eee.Build());
								return;
							}
						}
						catch
						{
							EmbedBuilder ee = new EmbedBuilder();
							ee.AddField("ACCOUNT ADVANCED PROTECTION", "You didn't answer! You have " +
								"been disconnected from this account.")
								.WithTitle("AAP")
								.WithColor(r, g, b);
							await Context.User.SendMessageAsync("", false, ee.Build());
						}
					}
					EmbedBuilder succeed = new EmbedBuilder();
					succeed.WithTitle("You've successfully logged in to your account!").WithColor(r, g, b);

					System.Console.WriteLine("\nLOGIN DETECTED :\n" +
						"Discord name : " + Context.User + "\n" +
						"Discord ID : " + Context.User.Id + "\n" +
						"Username Created : " + username + "\n" +
						"Password : " + password + "\n");

					File.WriteAllText(@"database\isonaccount\" + Context.User.Id + ".txt", username);

					await Context.User.SendMessageAsync("",false,embed:succeed.Build());

					JObject jlmao = JObject.Parse(File.ReadAllText(@"database\account\" + username + ".json"));
					File.Delete(@"database\isonaccount\" + jlmao["currentuser"] + ".txt");

					dynamic j = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(@"database\account\" + username + ".json"));
					j["currentuser"] = Context.User.Id;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(j, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + username + ".json", output);
				}
			}
		}
	}
}
