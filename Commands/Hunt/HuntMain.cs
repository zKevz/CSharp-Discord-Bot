using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static CowboyBot.CT;
using static CowboyBot.UserInfo;
using static CowboyBot.client_info;
using static CowboyBot.enemy_info;
using static CowboyBot.Timer;
using System.IO;

namespace CowboyBot
{
	public class CardMain : InteractiveBase
	{
		private Random r = new Random();
		private static ColorList c = new ColorList();
		private int rr = c.r;
		private int g = c.g;
		private int b = c.b;

		[Command("hunt", RunMode = RunMode.Async)]
		public async Task hunt()
		{
			if (lastHunt(accName(Context.User.Id), 3) == true)
			{
				List<string> animal = new List<string>()
				{
				"Deer","Deer","Deer","Deer","Deer","Gorilla","Anaconda", "Tapir","Jaguar","Lion","Tiger",
				"Monkey", "Elephant"
				};

				var user = accName(Context.User.Id);

				int attackAbility = 0;
				int healthincrease = 0;

				if (player_body(user) == 2) healthincrease = 100;
				if (player_hand(user) == 2) attackAbility = 30;
				if (player_body(user) == 3) healthincrease = 250;
				if (player_hand(user) == 3) attackAbility = 70;

				// variable
				turn t = new turn();
				int rand = r.Next(0, 2);

				// Program Setup
				int round = 1;

				// user info
				yhealth = r.Next(700, 1001) + healthincrease;
				yattack = 50;

				// enemy info
				ehealth = r.Next(600, 901);
				eattack = 50;

				switch (rand)
				{
					case 0:
						t = turn.enemy;
						break;
					case 1:
						t = turn.you;
						break;
				}
				string enemyLogs = "";

				while (ehealth > 0 && yhealth > 0)
				{
					eattack += round + r.Next(11, 21) + r.Next(1,5);
					yattack += round + r.Next(11, 21) + attackAbility;

					if (t == turn.you)
					{
						if (round == 1)
						{
							EmbedBuilder e = new EmbedBuilder();
							e.AddField("GOGOGO", "Player Body : " + getClothesName("pb", player_body(user)) + "\n" +
								"Player Hand : " + getClothesName("ph", player_hand(user)) + "\n" +
								"Your health : " + yhealth + "\n" +
								"Enemy health : " + ehealth + "\n" +
								"\nWhat will you do?\n[1] Attack\n[2] Heal\n[3] Surrender")
								.WithAuthor("You are in the first turn, " + Context.User + "!\n")
								.WithColor(rr, g, b)
								.WithFooter("Bot made by kevz#2073")
								.WithCurrentTimestamp();
							await ReplyAsync("", false, e.Build());

							/*await ReplyAsync(enemyLogs + "You are in the first turn!\n" +
								"Player Body : " + getClothesName("pb", player_body(user)) + "\n" +
								"Player Hand : " + getClothesName("ph", player_hand(user)) + "\n" +
								"Your health : " + yhealth + "\n" +
								"Enemy health : " + ehealth + "\n" +
								"\nWhat will you do?\n[1] Attack\n[2] Heal\n[3] Surrender");*/
						}
						else
						{
							EmbedBuilder e = new EmbedBuilder();
							e.AddField(enemyLogs, "Player Body : " + getClothesName("pb", player_body(user)) + "\n" +
								"Player Hand : " + getClothesName("ph", player_hand(user)) + "\n" +
								"Your health : " + yhealth + "\n" +
								"Enemy health : " + ehealth + "\n" +
								"\nWhat will you do?\n[1] Attack\n[2] Heal\n[3] Surrender")
								.WithAuthor("It Is " + Context.User + " Turn!\n")
								.WithColor(rr, g, b)
								.WithFooter("Bot made by kevz#2073")
								.WithCurrentTimestamp();
							await ReplyAsync("", false, e.Build());

							/*await ReplyAsync(enemyLogs + "\nIt is your turn!\n" +
								"Player Body : " + getClothesName("pb", player_body(user)) + "\n" +
								"Player Hand : " + getClothesName("ph", player_hand(user)) + "\n" +
								"Your health : " + yhealth + "\n" +
								"Enemy health : " + ehealth + "\n" +
								"\nWhat will you do?\n[1] Attack\n[2] Heal\n[3] Surrender");*/
						}
						t = turn.enemy;
						string answer = (await NextMessageAsync(true,true,TimeSpan.FromSeconds(60))).ToString().ToLower();
						if(answer == null)
						{
							answer = "1";
						}
						if (answer == "1" || answer.Contains("att"))
						{
							ehealth -= yattack + r.Next(1, 51);
							if (ehealth <= 0) ehealth = 0;
						}
						else if (answer == "2" || answer.Contains("heal"))
						{
							if (yhealth > 800)
							{
								await ReplyAsync("Your health is more than 80 % !\n" +
									"Your choice is become attack automatically.");
								ehealth -= yattack + r.Next(1, 51);
							}
							else
							{
								yincreaseHealth();
							}
						}
						else if (answer == "3" || answer.Contains("surre"))
						{
							yhealth = 0;
							break;
						}
						round++;
					}
					else
					{
						if (round == 1)
						{
							await ReplyAsync("The enemy got the first place!");
							if (ehealth > (ehealth * 2) / 5)
							{
								int asd = eattack + r.Next(1, 51);
								yhealth -= asd;
								enemyLogs = "The enemy used attack on you with power **" + asd + "**!\n";
								if (yhealth <= 0) yhealth = 0;
							}
							else
							{
								enemyLogs = "The enemy increased his health by ";
								int nowhealth = ehealth;
								eincreaseHealth();
								enemyLogs += (ehealth - nowhealth) + "!";
							}
						}
						else
						{
							await ReplyAsync("It's enemy turn now!");
							if (ehealth > (ehealth * 2) / 5)
							{
								int asd = eattack + r.Next(1, 51);
								yhealth -= asd;
								enemyLogs = "The enemy used attack on you with power **" + asd + "**!\n";
								if (yhealth <= 0) yhealth = 0;
							}
							else
							{
								enemyLogs = "The enemy increased his health by ";
								int nowhealth = ehealth;
								eincreaseHealth();
								enemyLogs += (ehealth - nowhealth) + "!";
							}
						}
						t = turn.you;
						round++;
						
					}
				}
				if (ehealth == 0)
				{
					string animals = animal[r.Next(0, 13)];
					int coinsPrize = 10 * r.Next(1, 6);
					EmbedBuilder e = new EmbedBuilder();
					e.AddField("You Win!", "Hello " + Context.User + "! You got " + animals + " and you sold it for " + coinsPrize + " coins!")
						.WithAuthor("Hooray!\n")
						.WithColor(rr, g, b)
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
					//await ReplyAsync("**YOU WIN!**\n" + "You got " + animals + " and you sold it for " + coinsPrize + " coins!");

					string path = File.ReadAllText(@"database\account\" + user + ".json");

					dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(path);
					jsonObj["coins"] += coinsPrize;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
					File.WriteAllText(@"database\account\" + user + ".json", output);

					EnsureFromUserCriterion ensure = new EnsureFromUserCriterion(Context.User);
					var a = await NextMessageAsync();
				}
				else
				{
					string animals = animal[r.Next(0, 13)];

					EmbedBuilder e = new EmbedBuilder();
					e.AddField("You Lose!", "Hello " + Context.User + "! You weren't able to fight " + animals + "! You decided to surrender.")
						.WithAuthor("R.I.P\n")
						.WithColor(rr, g, b)
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());

					/*await ReplyAsync("**RIP**\n" +
						"You weren't able to fight " + animals + "! You decided to surrender.");*/
				}
			}
			else
			{

			}
		}
	}
}