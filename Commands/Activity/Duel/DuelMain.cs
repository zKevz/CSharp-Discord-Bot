using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.DuelUI;
using static CowboyBot.DuelUI.DuelTurn;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class DuelMain : InteractiveBase
	{
		[Command("duel",RunMode = RunMode.Async)]
		public async Task Duel(IGuildUser user)
		{
			if(Context.User == user)
			{
				await sey("You can't duel yourself!");
				return;
			}

			else if (!isOnAccount(user.Id))
			{
				await ReplyAsync("That user doesn't even have cowboy account!");
			}
			else
			{
				if (!lastInviteDuel(accName(Context.User.Id), 2))
				{
					await sey("Please wait 2 minutes to invite someone again.\n" +
						"We don't want you use this feature to spam someone.");
					return;
				}

				await ReplyAsync("Sending request...");

				await see("User " + Context.User + " asked you to play Cowboy Duel!\n" +
					"Type \"accept\" if u accept it.", user);

				string AcceptOrNo = "";
				try
				{
					AcceptOrNo = (await NextMessageAsync(new EnsureFromUserCriterion(user), TimeSpan.FromSeconds(30))).ToString();
				}
				catch
				{
					await sey("User " + user + " didn't accept your request.");
					await see("You didn't accept the request", user);
					lastInviteDuelTimer.Add(DateTimeOffset.Now);
					lastInviteDuelTarget.Add(accName(Context.User.Id));
					return;
				}

				if (AcceptOrNo.ToLower() != "accept")
				{
					await sey("User " + user + " didn't accept your request.");
					await see("You didn't accept the request", user);
					lastInviteDuelTimer.Add(DateTimeOffset.Now);
					lastInviteDuelTarget.Add(accName(Context.User.Id));
					return;
				}

				Random r = new Random();

				DuelTurn turn = (DuelTurn)r.Next(0, 2);

				int round = 1;

				string yourLogs = "";
				string enemyLogs = "";

				string enemySurrenderLogs = "";
				string yourSurrenderLogs = "";

				ydefence = r.Next(10, 21);
				edefence = r.Next(10, 21);

				eattack = r.Next(50, 101);
				yattack = r.Next(50, 101);

				yhealth = 500;
				ehealth = 500;

				while (yhealth > 0 && ehealth > 0)
				{

					EnsureFromUserCriterion cuser = new EnsureFromUserCriterion(Context.User);
					EnsureFromUserCriterion euser = new EnsureFromUserCriterion(user);

					if (turn == enemy)
					{
						if (round == 1)
						{
							await see("You are in the first turn!", user);
							await sey("You are in the second turn!\nW" +
								"aiting for enemy..");

							await see("Your health : " + ehealth + "\n" +
							"Your attack : " + eattack + "\n" +
							"Your defence : " + edefence + "\n\n" +
							"Enemy's health : " + yhealth + "\n" +
							"Enemy's attack : " + yattack + "\n" +
							"Enemy's defence : " + ydefence + "\n\n" +
							"**[1] Attack**\n" +
							"**[2] Defence**\n" +
							"**[3] Heal [HEALTH MUST LESS THAN 80%]**\n" +
							"**[4] Surrender**\n\n" +
							"What will u choose? [You have 30 seconds]", user);

						}

						if (round != 1)
						{
							await see(enemyLogs + "\nYour health : " + ehealth + "\n" +
								"Your attack : " + eattack + "\n" +
								"Your defence : " + edefence + "\n\n" +
								"Enemy's health : " + yhealth + "\n" +
								"Enemy's attack : " + yattack + "\n" +
								"Enemy's defence : " + ydefence + "\n\n" +
								"**[1] Attack**\n" +
								"**[2] Defence**\n" +
								"**[3] Heal [HEALTH MUST LESS THAN 80%]**\n" +
								"**[4] Surrender**\n\n" +
								"What will u choose? [You have 30 seconds]", user);
						}

						string answer = "";
						try
						{
							answer = (await NextMessageAsync(euser, TimeSpan.FromSeconds(30))).ToString();
						}
						catch
						{
							await see("You didn't answer in time! Your turn is skipped..", user);
							yourLogs = "Enemy didn't do anything!";
							turn = you;
							continue;
						}

						if(answer == "1" || answer.ToLower().Contains("att"))
						{
							int b = r.Next(0, 2);
							if (b == 0)
							{
								int critical = 0;
								string criticalMessage = "";
								if (r.Next(0, 4) == 0)
								{
									critical = (eattack - ydefence);
									criticalMessage = "[CRITICAL] ";
								}
								yhealth -= (eattack - ydefence) + critical;
								int e = (eattack - ydefence) + critical;
								if (yhealth <= 0) yhealth = 0;
								yourLogs = criticalMessage + "Enemy used attack on you and caused " + e  + " damage!";
								await see(criticalMessage +"You attacked the enemy and caused " + e + " damage!", user);
							}
							else
							{
								await see("You failed to attack the enemy!", user);
								yourLogs = "Enemy failed to attack you.";
							}
						}
						else if (answer == "2" || answer.ToLower().Contains("def"))
						{
							int a = r.Next(1,3);
							int b = r.Next(0, 2);
							if (b == 0)
							{
								int critical = 0;
								string criticalMessage = "";
								if (r.Next(0, 4) == 0)
								{
									critical = a;
									criticalMessage = "[CRITICAL] ";
								}
								edefence += a + critical;
								int e = a + critical;
								yourLogs = criticalMessage + "Enemy improved the defence by " + e;
								await see(criticalMessage + "You improved your defence by " + e, user);
							}
							else
							{
								await see("You failed to improve your defence", user);
								yourLogs = "Enemy failed to improve his defence!";
							}
						}
						else if (answer == "3" || answer.ToLower().Contains("heal"))
						{
							if (ehealth >= 400)
							{
								await see("Your health must under `800` to heal! Skipping turn..", user);
								turn = you;
								continue;
							}
							else
							{
								int b = r.Next(0, 2);
								if (b == 0)
								{
									int a = r.Next(50, 81);
									int critical = 0;
									string criticalMessage = "";
									if (r.Next(0, 4) == 0)
									{
										critical = a;
										criticalMessage = "[CRITICAL] ";
									}
									ehealth += a + critical;
									int e = a + critical;
									yourLogs = criticalMessage + "Enemy healed itself by " + e;
									await see(criticalMessage + "You healed yourself by " + e, user);
								}
								else
								{
									yourLogs = "Enemy failed to heal himself!";
									await see("You failed to heal yourself!", user);
								}
							}
						}
						else if (answer == "4" || answer.ToLower().Contains("surrend"))
						{
							yourSurrenderLogs = "Enemy surrender-ed !";
							ehealth = 0;
							await see("You surrendered!", user);
							break;
						}
						else
						{
							await see("Invalid input! Your turn is skipped!", user);
							yourLogs = "Enemy did nothing!";
						}
						turn = you;
						await see("Waiting for your opponent...", user);
						yattack = r.Next(50, 101);
					}
					else
					{
						if (round == 1)
						{
							await sey("You are in the first turn!");
							await see("You are in the second turn!\nW" +
								"aiting for enemy..",user);

							await sey("Your health : " + yhealth + "\n" +
							"Your attack : " + yattack + "\n" +
							"Your defence : " + ydefence + "\n\n" +
							"Enemy's health : " + ehealth + "\n" +
							"Enemy's attack : " + eattack + "\n" +
							"Enemy's defence : " + edefence + "\n\n" +
							"**[1] Attack**\n" +
							"**[2] Defence**\n" +
							"**[3] Heal [HEALTH MUST LESS THAN 80%]**\n" +
							"**[4] Surrender**\n\n" +
							"What will u choose? [You have 30 seconds]");
						}

						if (round != 1)
						{
							await sey(yourLogs + "\nYour health : " + yhealth + "\n" +
								"Your attack : " + yattack + "\n" +
								"Your defence : " + ydefence + "\n\n" +
								"Enemy's health : " + ehealth + "\n" +
								"Enemy's attack : " + eattack + "\n" +
								"Enemy's defence : " + edefence + "\n\n" +
								"**[1] Attack**\n" +
								"**[2] Defence**\n" +
								"**[3] Heal [HEALTH MUST LESS THAN 80%]**\n" +
								"**[4] Surrender**\n\n" +
								"What will u choose? [You have 30 seconds]");
						}

						string answer = "";
						try
						{
							answer = (await NextMessageAsync(cuser, TimeSpan.FromSeconds(30))).ToString();
						}
						catch
						{
							await sey("You didn't answer in time! Your turn is skipped..");
							turn = enemy;
							enemyLogs = "Enemy didn't do anything!";
							continue;
						}

						if (answer == "1" || answer.ToLower().Contains("att"))
						{
							int b = r.Next(0, 2);
							if (b == 0)
							{
								int critical = 0;
								string criticalMessage = "";
								if (r.Next(0, 4) == 0)
								{
									critical = (yattack - edefence);
									criticalMessage = "[CRITICAL] ";
								}
								ehealth -= (yattack - edefence) + critical;
								int e = (yattack - edefence) + critical;
								if (ehealth <= 0) ehealth = 0;
								enemyLogs = criticalMessage + "Enemy used attack on you and caused " + e+ " damage!";
								await sey(criticalMessage + "You attacked the enemy and caused " + e+ " damage!");
							}
							else
							{
								await sey("You failed to attack the enemy!");
								enemyLogs = "Enemy failed to attack you!";
							}
						}
						else if (answer == "2" || answer.ToLower().Contains("def"))
						{
							int a = r.Next(1, 3);
							int b = r.Next(0, 2);
							if (b == 0)
							{
								int critical = 0;
								string criticalMessage = "";
								if (r.Next(0, 4) == 0)
								{
									critical = a;
									criticalMessage = "[CRITICAL] ";
								}
								ydefence += a + critical;
								int e = a + critical;

								enemyLogs = criticalMessage + "Enemy improved the defence by " + e;
								await sey(criticalMessage + "You improved your defence by " + e);
							}
							else
							{
								await sey("You failed to improve your defence");
								enemyLogs = "Enemy failed to improve his defence!";
							}
						}
						else if (answer == "3" || answer.ToLower().Contains("heal"))
						{
							if (yhealth >= 400)
							{
								await sey("Your health must be less than 800 to heal!\n" +
									"Skipping turn..");
								turn = enemy;
								continue;
							}
							else
							{
								int b = r.Next(0, 2);
								if (b == 0)
								{
									int a = r.Next(50, 81);
									int critical = 0;
									string criticalMessage = "";
									if (r.Next(0, 4) == 0)
									{
										critical = a;
										criticalMessage = "[CRITICAL] ";
									}
									int e = critical + a;
									yhealth += a + critical;
									enemyLogs = criticalMessage +"Enemy healed itself by " + e;
									await sey(criticalMessage +"You healed yourself by " + e);
								}
								else
								{
									enemyLogs = "Enemy failed to heal himself";
									await sey("You failed to heal yourself!");
								}
							}
						}
						else if (answer == "4" || answer.ToLower().Contains("surrend"))
						{
							enemySurrenderLogs = "Enemy surrender-ed!";
							yhealth = 0;
							await sey("You surrendered!");
							break;
						}
						else
						{
							await sey("Invalid input! Your turn is skipped!");
							enemyLogs = "Enemy didn't do anything!";
						}
						eattack = r.Next(50, 101);
						turn = enemy;
						await sey("Waiting for your opponent...");
					}
					round++;
				}

				if (yhealth <= 0)
				{
					await see(enemySurrenderLogs + "\nYou have won the duel!", user);
					await sey("You lost the duel.. It's okay..");

					int bounty = r.Next(1, 3);

					await see("You got " + bounty + " bounty points!", user);

					dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
						@"database\account\" + accName(user.Id) + ".json"));
					j["bounty"] += bounty;
					string output = JsonConvert.SerializeObject(j,
						Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(user.Id) + ".json", output);

					JObject haha = JObject.Parse(File.ReadAllText(
						@"database\account\" + accName(Context.User.Id) + ".json"));
					if ((int)haha["bounty"] == 0) bounty = 0;

					dynamic jj = JsonConvert.DeserializeObject(File.ReadAllText(
						@"database\account\" + accName(Context.User.Id) + ".json"));
					jj["bounty"] -= bounty;
					string outputj = JsonConvert.SerializeObject(jj,
						Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", outputj);

					if (bounty != 0)
					{
						await sey("You lost " + bounty + " bounty points!");
					}
				}
				else if(ehealth <= 0)
				{
					await see("You lost the duel.. It's okay..",user);
					await sey(yourSurrenderLogs + "\nYou have won the duel!");

					int bounty = r.Next(1, 3);

					await sey("You got " + bounty + " bounty points!");

					dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
						@"database\account\" + accName(Context.User.Id) + ".json"));
					j["bounty"] += bounty;
					string output = JsonConvert.SerializeObject(j,
						Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(Context.User.Id) + ".json", output);

					JObject haha = JObject.Parse(File.ReadAllText(
						@"database\account\" + accName(user.Id) + ".json"));
					if ((int)haha["bounty"] == 0) bounty = 0;

					dynamic jj = JsonConvert.DeserializeObject(File.ReadAllText(
						@"database\account\" + accName(user.Id) + ".json"));
					jj["bounty"] -= bounty;
					string outputj = JsonConvert.SerializeObject(jj,
						Formatting.Indented);
					File.WriteAllText(@"database\account\" + accName(user.Id) + ".json", outputj);

					if(bounty != 0)
					{
						await see("You lost " + bounty + " bounty points!",user);
					}
				}
			}
		}
		public async Task see(string text,IGuildUser user)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("DUEL", text)
				.WithAuthor("Cowboy's Duel")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await user.SendMessageAsync("",false,e.Build());
		}

		public async Task sey(string text)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("DUEL", text)
				.WithAuthor("Cowboy's Duel")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());
		}
	}
}