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
using static CowboyBot.BossUI;
using static CowboyBot.BossUI.Turn;

namespace CowboyBot
{
	public class BossMain : InteractiveBase
	{
		[Command("fightboss",RunMode = RunMode.Async)]
		public async Task FightBoss()
		{
			string[] s = Directory.GetFiles(@"database\guilds");
			string username = accName(Context.User.Id);
			bool isOnGuild = false;

			string guildName = "";

			foreach(string g in s)
			{
				JObject j = JObject.Parse(
					File.ReadAllText(g));
				for(int i = 0; i < j["member"].ToList().Count; i++)
				{

					if(j["member"][i].ToString().Substring(0,
						j["member"][i].ToString().IndexOf(" ")) == username)
					{
						isOnGuild = true;
						guildName = (string)j["guildname"];
						bHealth = (int)j["bosshealth"] * (int)j["bosslevel"];
					}
				}
			}

			if (!isOnGuild)
			{
				await SendEmbed("WHOOPS", "You don't even in a guild!");
				return;
			}
			else if (isBattle(guildName))
			{
				await SendEmbed("WAIT","Someone is fighting the boss right now!\n" +
					"Please wait!");
				return;
			}
			else
			{
				dynamic wow = JsonConvert.DeserializeObject(
					File.ReadAllText(@"database\guilds\" + guildName + ".json"));
				wow["isbattle"] = true;
				string asd = JsonConvert.SerializeObject(
					wow, Formatting.Indented);
				File.WriteAllText(@"database\guilds\" +
					guildName + ".json", asd);

				JObject what = JObject.Parse(File.ReadAllText(
					@"database\guilds\" + guildName + ".json"));
				int firstHealth = bHealth;

				Random r = new Random();

				int yHealth = 1000;
				Turn t = new Turn();
				try
				{
					t = (Turn)r.Next(0, 2);
				}
				catch { Console.WriteLine("error"); }

				string logs = "";
				int enemyHeal = 0;

				while (yHealth > 0 && bHealth > 0)
				{
					int yAttack = r.Next(50, 101);
					bAttack = r.Next(500, 1001) * (int) what["bosslevel"];

					if (t == you) {

						await SendEmbed("BOSS FIGHT", logs + "\nYour health : " + yHealth + "\n" +
								"Your attack : " + yAttack + "\n" +
								"Enemy's health : " + bHealth + "\n" +
								"Enemy's attack : " + bAttack + "\n\n" +
								"**[1] Attack**\n" +
								"**[2] Heal**\n" +
								"**[3] Surrender**\n\n" +
								"What will u choose? [You have 30 seconds]");

						string answer = "";
						try
						{
							answer = (await NextMessageAsync(true,true,TimeSpan.FromSeconds(30))).ToString();
						}
						catch
						{
							await SendEmbed("WHOOPS", "You didn't answer at time! Skipping turn...");
							t = boss;
							continue;
						}

						if(answer == "1" || answer.ToLower().Contains("att"))
						{
							bHealth -= yAttack + r.Next(10,21);
						}
						else if (answer == "2" || answer.ToLower().Contains("heal"))
						{
							yHealth += r.Next(25, 86);
						}
						else if (answer == "3" || answer.ToLower().Contains("surrend"))
						{
							yHealth = 0;
							break;
						}
						else
						{
							await SendEmbed("WHOOPS", "You didn't answer the right value!" +
								" Skipping turn");
							t = boss;
							continue;
						}
						t = boss;
					}
					else
					{
						await SendEmbed("BOSS TURN", "Boss Turn");
						int a = r.Next(0, 3);
						if (a == 0 || a == 2)
						{
							yHealth -= bAttack;
							logs = "THE BOSS HITTED YOU WITH " + bAttack + 
								" DAMAGE!";
						}
						else if (a == 1)
						{
							int b = r.Next(100,201);
							bHealth += b;
							logs = "THE BOSS HEALED HIMSELF BY " + b;
							enemyHeal += b;
						}
						t = you;
					}
				}

				if (yHealth <= 0)
				{
					EditHP(guildName, bHealth);
					await SendEmbed("YOU LOSE!", "" +
						"Total Damage : " + ((firstHealth - bHealth) - enemyHeal) + "\n" +
						"Boss Health [NOW] : " + bHealth);

					dynamic cmon = JsonConvert.DeserializeObject(
						File.ReadAllText(@"database\guilds\" +
						guildName + ".json"));
					cmon["isbattle"] = false;
					cmon["bosshealth"] = bHealth;
					string q = JsonConvert.SerializeObject(cmon,
						Formatting.Indented);
					File.WriteAllText(@"database\guilds\" + guildName + ".json", q);
				}
				else if (bHealth <= 0)
				{
					int coinsGet = 500 * (int)what["bosslevel"];
					EditLVL(guildName, (int)what["bosslevel"] + 1);
					await SendEmbed("BOSS DEFEATED", "YOU HAVE DEFEATED BOSS LEVEL " +
						(int)what["bosslevel"] + "!\n" + coinsGet + " coins is " +
						"given to all the members.");

					JObject lol = JObject.Parse(File.ReadAllText(
						@"database\guilds\" + guildName + ".json"));

					for(int i = 0; i < lol["member"].ToList().Count; i++)
					{
						dynamic lolxd = JsonConvert.DeserializeObject(
							File.ReadAllText(@"database\account\" +
							lol["member"][i].ToString().Substring(0,
							lol["member"][i].ToString().IndexOf(" ")) + ".json"));
						lolxd["coins"] += coinsGet;
						string q = JsonConvert.SerializeObject(lolxd,
							Formatting.Indented);
						File.WriteAllText(@"database\account\" +
							lol["member"][i].ToString().Substring(0,
							lol["member"][i].ToString().IndexOf(" ")) + ".json", q);
					}

					dynamic cmon = JsonConvert.DeserializeObject(
						File.ReadAllText(@"database\guilds\" +
						guildName + ".json"));
					cmon["isbattle"] = false;
					string qq = JsonConvert.SerializeObject(cmon,
						Formatting.Indented);
					File.WriteAllText(@"database\guilds\" + guildName + ".json", qq);
				}
			}
		}

		public async Task SendEmbed(string title, string text)
		{
			Random r = new Random();

			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, text)
				.WithAuthor("Guild Boss")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithCurrentTimestamp();
			await ReplyAsync("",false,e.Build());
		}
	}
}