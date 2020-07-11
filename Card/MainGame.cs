using System;
using Discord.Addons.Interactive;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using static CowboyBot.UserInfo;
using Newtonsoft.Json.Linq;
using static CowboyBot.Deck.turn;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static CowboyBot.Deck;

namespace CowboyBot
{
    public class CowboyBot : InteractiveBase
    {
        [Command("playcard", RunMode = RunMode.Async)]
        public async Task playcard(IGuildUser user)
		{
			if (Context.User.ToString() != "kevz#2073") return;

			if (!isOnAccount(user.Id))
			{
				await ReplyAsync("That user doesn't even have a " +
					"cowboy account!");
				return;
			}

			await ReplyAsync("Sending request to " + user.Mention);

			await user.SendMessageAsync("user " + Context.User + " asked you to play card!" +
				"\nType \"accept\" if you accept. You have 30 seconds.");

			EnsureFromUserCriterion eidk = new EnsureFromUserCriterion(user);

			string targetuseranswer = ""; try { targetuseranswer = (await NextMessageAsync(eidk, TimeSpan.FromSeconds(30))).ToString(); }
			catch
			{
				await Context.User.SendMessageAsync("The user didn't accept your request.");
				return;
			}

			if (targetuseranswer.ToLower() != "accept")
			{
				await Context.User.SendMessageAsync("The user didn't accept your request.");
				return;
			}
			else
			{
				List<string> yourCard = new List<string>();
				List<string> enemyCard = new List<string>();
				Random r = new Random();
				try
				{
					Deck.createDeck();
					Deck.Shuffle(Deck.card);
				}
				catch { Console.WriteLine("error at making deck"); }

				string enemyresult = "";
				string yourresult = "";
				try
				{
					int countforloops = 1;
					for (int i = 1; i <= 5; i++)
					{
						Random randforIndex = new Random();
						int randomIndex = randforIndex.Next(0, Deck.card.Count);
						int randomIndex2 = randforIndex.Next(0, Deck.card.Count);

						if (i <= 4)
						{
							yourresult += "[" + countforloops + "] " + Deck.card[randomIndex] + "\n";
							Deck.yourCard.Add(Deck.card[randomIndex]);
							Deck.card.RemoveAt(randomIndex);

							enemyresult += "[" + countforloops + "] " + Deck.card[randomIndex2] + "\n";
							Deck.enemyCard.Add(Deck.card[randomIndex2]);
							Deck.card.RemoveAt(randomIndex2);
						}
						else
						{
							yourresult += "[" + countforloops + "] " + Deck.card[randomIndex];
							Deck.yourCard.Add(Deck.card[randomIndex]);
							Deck.card.RemoveAt(randomIndex);

							enemyresult += "[" + countforloops + "] " + Deck.card[randomIndex2];
							Deck.enemyCard.Add(Deck.card[randomIndex2]);
							Deck.card.RemoveAt(randomIndex2);
						}
						countforloops++;
					}
				}
				catch { Console.WriteLine("error at for loops");
					await Context.User.SendMessageAsync("Card Game Error. Please try again.");
					await user.SendMessageAsync("Card Game Error. Please try again.");
					return;
				}

				await Context.User.SendMessageAsync("Your card : \n" + yourresult);
				await user.SendMessageAsync("Your card : \n" + enemyresult);

				int rand = r.Next(0, Deck.card.Count);

				string firstCard = Deck.card[rand];

				List < string > enemyMatchesCards = Deck.enemyCard;
				List < string > yourMatchesCards = Deck.yourCard;

				EnsureFromUserCriterion enemyCriterion = new EnsureFromUserCriterion(user);
				EnsureFromUserCriterion youCriterion = new EnsureFromUserCriterion(Context.User);

				Deck.turn t = new Deck.turn();
				t = (Deck.turn) r.Next(0, 2);

				await Context.User.SendMessageAsync("First Card is : " + firstCard);
				await user.SendMessageAsync("First Card is : " + firstCard);

				if (t == enemy)
				{
					string str2 = firstCard.Substring(firstCard.IndexOf(" ") + 1);

					bool enemyFound = false;
					try
					{
						foreach (string s in Deck.enemyCard)
						{
							string str1 = s.Substring(s.IndexOf(" ") + 1);

							if (s.Substring(s.IndexOf(" ", s.IndexOf(" ") + 1) + 1) ==
								firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								enemyFound = true;
							}
						}
					}
					catch { Console.WriteLine("error at foreachloops"); }

					if (!enemyFound)
					{
						await user.SendMessageAsync("You took " + firstCard + " because you don't have that type of card!");
						await Context.User.SendMessageAsync("Your enemy took the " + firstCard + "!");
						t = you;
					}

					else
					{
						string result = "";
						int count = 1;
						
						for(int i = 0; i < 5; i++)
						{
							if(Deck.enemyCard[i].Substring(Deck.enemyCard[i].IndexOf(" ", Deck.enemyCard[i].IndexOf(" ") + 1) + 1)
								== firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								result += "[" + (Deck.enemyCard.IndexOf(Deck.enemyCard[i]) + 1) + "] " + Deck.enemyCard[i] + "\n";
								enemyMatchesCards.Add(Deck.enemyCard[i]);
								count++;
							}
						}

						await user.SendMessageAsync("Which card would you like to use?" +
							"\n**__PLEASE CHOOSE BY NUMBER!__**\n Matches" +
							" type of cards : \n" + result);

						int removeCards = 0;try
						{
							removeCards = Convert.ToInt32((await NextMessageAsync(enemyCriterion, TimeSpan.FromSeconds(30))).Content);
						}
						catch
						{
							await user.SendMessageAsync("You didn't put number! Your card is removed by default.");

							Random idk = new Random();

							removeCards = 69420;

							int h = idk.Next(0, enemyMatchesCards.Count);

							string randomCard = enemyMatchesCards[h];

							await Context.User.SendMessageAsync("Your enemy removed " + randomCard + "!");
							await user.SendMessageAsync("You removed " + randomCard + " as default.");

							Deck.enemyCard.Remove(enemyMatchesCards[h]);
						}

						if (removeCards != 69420)
						{

							if (Deck.enemyCard[removeCards - 1].Contains(str2.Substring(str2.IndexOf("f") + 2)))
							{
								await user.SendMessageAsync("You removed " + Deck.enemyCard[removeCards - 1] + "\nWaiting for opponent...");
								await Context.User.SendMessageAsync("Your enemy removed " + Deck.enemyCard[removeCards - 1] + "!");

								Deck.enemyCard.Remove(Deck.enemyCard[removeCards - 1]);
							}
							else
							{
								await user.SendMessageAsync("The type of that card is different!");

								Random idk = new Random();

								int h = idk.Next(0, enemyMatchesCards.Count);

								string randomCard = enemyMatchesCards[h];

								await Context.User.SendMessageAsync("Your enemy removed " + randomCard + "!");
								await user.SendMessageAsync("You removed " + randomCard + " as default.");

								Deck.enemyCard.Remove(enemyMatchesCards[h]);

							}
						}
					}
					t = you;
				}
				else
				{
					string str2 = firstCard.Substring(firstCard.IndexOf(" ") + 1);

					bool enemyFound = false;
					try
					{
						foreach (string s in Deck.yourCard)
						{
							string str1 = s.Substring(s.IndexOf(" ") + 1);

							if (s.Substring(s.IndexOf(" ", s.IndexOf(" ") + 1) + 1) ==
								firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								enemyFound = true;
							}
						}
					}
					catch { Console.WriteLine("error at foreachloops"); }

					if (!enemyFound)
					{
						await Context.User.SendMessageAsync("You took " + firstCard + " because you don't have that type of card!");
						await user.SendMessageAsync("Your enemy took the " + firstCard + "!");
						t = you;
					}

					else
					{
						string result = "";
						int count = 1;

						for (int i = 0; i < 5; i++)
						{
							if (Deck.yourCard[i].Substring(Deck.yourCard[i].IndexOf(" ", Deck.yourCard[i].IndexOf(" ") + 1) + 1)
								== firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								result += "[" + (Deck.yourCard.IndexOf(Deck.yourCard[i]) + 1) + "] " + Deck.yourCard[i] + "\n";
								yourMatchesCards.Add(Deck.yourCard[i]);
								count++;
							}
						}

						await Context.User.SendMessageAsync("Which card would you like to hit?" +
							"\n**__PLEASE CHOOSE BY NUMBER!__**\n Matches" +
							" type of cards : \n" + result);

						int removeCards = 0;try
						{
							removeCards = Convert.ToInt32((await NextMessageAsync(youCriterion, TimeSpan.FromSeconds(30))).Content);
						}
						catch
						{
							await Context.User.SendMessageAsync("You didn't put number! Your card is removed by default.");

							Random idk = new Random();

							removeCards = 69420;

							int h = idk.Next(0, yourMatchesCards.Count);

							string randomCard = yourMatchesCards[h];

							await user.SendMessageAsync("Your enemy removed " + randomCard + "!");
							await Context.User.SendMessageAsync("You removed " + randomCard + " as default.");

							Deck.yourCard.Remove(yourMatchesCards[h]);
						}

						if (removeCards != 69420)
						{

							if (Deck.yourCard[removeCards - 1].Contains(str2.Substring(str2.IndexOf("f") + 2)))
							{
								await Context.User.SendMessageAsync("You removed card " + Deck.yourCard[removeCards - 1]);
								await user.SendMessageAsync("Your enemy removed " + Deck.yourCard[removeCards - 1] + "!");

								yourCard.Remove(yourCard[removeCards - 1]);
							}
							else
							{
								await Context.User.SendMessageAsync("The type of that card is different!");

								Random idk = new Random();

								int h = idk.Next(0, yourMatchesCards.Count);

								string randomCard = yourMatchesCards[h];

								await user.SendMessageAsync("Your enemy removed " + randomCard + "!");
								await Context.User.SendMessageAsync("You removed " + randomCard + " as default.");

								Deck.yourCard.Remove(yourMatchesCards[h]);

							}
						}
					}
					t = enemy;
				}

				if (t == enemy)
				{
					string str2 = firstCard.Substring(firstCard.IndexOf(" ") + 1);

					bool enemyFound = false;
					try
					{
						foreach (string s in Deck.enemyCard)
						{
							string str1 = s.Substring(s.IndexOf(" ") + 1);

							if (s.Substring(s.IndexOf(" ", s.IndexOf(" ") + 1) + 1) ==
								firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								enemyFound = true;
							}
						}
					}
					catch { Console.WriteLine("error at foreachloops"); }

					if (!enemyFound)
					{
						await user.SendMessageAsync("You took " + firstCard + " because you don't have that type of card!");
						await Context.User.SendMessageAsync("Your enemy took the " + firstCard + "!");
						t = you;
					}
					else
					{

						string result = "";

						for (int i = 0; i < 5; i++)
						{
							if (Deck.enemyCard[i].Substring(Deck.enemyCard[i].IndexOf(" ", Deck.enemyCard[i].IndexOf(" ") + 1) + 1)
								== firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								result += "[" + (Deck.enemyCard.IndexOf(Deck.enemyCard[i]) + 1) + "] " + Deck.enemyCard[i] + "\n";
								enemyMatchesCards.Add(Deck.enemyCard[i]);
							}
						}

						await user.SendMessageAsync("It's your turn now!\n" +
							"Which card would you like to hit?" +
								"\n**__PLEASE CHOOSE BY NUMBER!__**\n Matches" +
								" type of cards : \n" + result);

						int removeCards = 0;try
						{
							removeCards = Convert.ToInt32((await NextMessageAsync(enemyCriterion, TimeSpan.FromSeconds(30))).Content);
						}
						catch
						{
							await user.SendMessageAsync("You didn't put number! Your card is removed by default.");

							Random idk = new Random();

							removeCards = 69420;

							int h = idk.Next(0, enemyMatchesCards.Count);

							string randomCard = enemyMatchesCards[h];

							await Context.User.SendMessageAsync("Your enemy removed " + randomCard + "!");
							await user.SendMessageAsync("You removed " + randomCard + " as default.");

							Deck.enemyCard.Remove(enemyMatchesCards[h]);
						}
						if (removeCards != 69420)
						{
							if (Deck.enemyCard[removeCards - 1].Contains(str2.Substring(str2.IndexOf("f") + 2)))
							{
								await user.SendMessageAsync("You removed " + Deck.enemyCard[removeCards - 1]);
								await Context.User.SendMessageAsync("Your enemy removed " + Deck.enemyCard[removeCards - 1] + "!");

								enemyCard.Remove(enemyCard[removeCards - 1]);
							}
							else
							{
								await user.SendMessageAsync("The type of that card is different!");

								Random idk = new Random();

								int h = idk.Next(0, enemyMatchesCards.Count);

								string randomCard = enemyMatchesCards[h];

								await Context.User.SendMessageAsync("Your enemy removed " + randomCard + "!");
								await user.SendMessageAsync("You removed " + randomCard + " as default.");

								Deck.enemyCard.Remove(enemyMatchesCards[h]);

							}
						}
					}
				}
				else
				{
					string str2 = firstCard.Substring(firstCard.IndexOf(" ") + 1);

					bool enemyFound = false;
					try
					{
						foreach (string s in Deck.yourCard)
						{
							string str1 = s.Substring(s.IndexOf(" ") + 1);

							if (s.Substring(s.IndexOf(" ", s.IndexOf(" ") + 1) + 1) ==
								firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								enemyFound = true;
							}
						}
					}
					catch { Console.WriteLine("error at foreachloops"); }

					if (!enemyFound)
					{
						await Context.User.SendMessageAsync("You took " + firstCard + " because you don't have that type of card!");
						await user.SendMessageAsync("Your enemy took the " + firstCard + "!");
						t = you;
					}
					else
					{

						string result = "";

						for (int i = 0; i < 5; i++)
						{
							if (Deck.yourCard[i].Substring(Deck.yourCard[i].IndexOf(" ", Deck.yourCard[i].IndexOf(" ") + 1) + 1)
								== firstCard.Substring(firstCard.IndexOf(" ", firstCard.IndexOf(" ") + 1) + 1))
							{
								result += "[" + (Deck.yourCard.IndexOf(Deck.yourCard[i]) + 1) + "] " + Deck.yourCard[i] + "\n";
								yourMatchesCards.Add(Deck.yourCard[i]);
							}
						}

						await Context.User.SendMessageAsync("It's your turn now!\n" +
							"Which card would you like to hit?" +
								"\n**__PLEASE CHOOSE BY NUMBER!__**\n Matches" +
								" type of cards : \n" + result);
							
						int removeCards = 0;try { removeCards = Convert.ToInt32((await NextMessageAsync(youCriterion, TimeSpan.FromSeconds(30))).Content); }
						catch {
							await Context.User.SendMessageAsync("You didn't put number! Your card is removed by default.");

							removeCards = 69420;

							Random idk = new Random();

							int h = idk.Next(0, yourMatchesCards.Count);

							string randomCard = yourMatchesCards[h];

							await user.SendMessageAsync("Your enemy removed " + randomCard + "!");
							await Context.User.SendMessageAsync("You removed " + randomCard + " as default.");

							Deck.yourCard.Remove(yourMatchesCards[h]);
						}
						if (removeCards != 69420)
						{
							if (Deck.yourCard[removeCards - 1].Contains(str2.Substring(str2.IndexOf("f") + 2)))
							{
								await Context.User.SendMessageAsync("You removed " + Deck.yourCard[removeCards - 1]);
								await user.SendMessageAsync("Your enemy removed " + Deck.yourCard[removeCards - 1] + "!");

								yourCard.Remove(yourCard[removeCards - 1]);
							}
							else
							{
								await user.SendMessageAsync("The type of that card is different!");

								Random idk = new Random();

								int h = idk.Next(0, yourMatchesCards.Count);

								string randomCard = yourMatchesCards[h];

								await user.SendMessageAsync("Your enemy removed " + randomCard + "!");
								await Context.User.SendMessageAsync("You removed " + randomCard + " as default.");

								Deck.yourCard.Remove(yourMatchesCards[h]);

							}
						}
					}
				}

				while (Deck.yourCard.Count > 0 && Deck.enemyCard.Count > 0)
				{

					string enemyresult2 = "";
					string yourresult2 = "";

					bool higherY = false;
					bool higherE = false;

					int enemycount = 1;
					int yourcount = 1;

					yourMatchesCards.Clear();
					enemyMatchesCards.Clear();

					foreach (string s in Deck.enemyCard)
					{
						enemyresult2 += "[" + enemycount + "]" + s + "\n";
						enemycount++;
					}

					foreach(string s in Deck.yourCard)
					{
						yourresult2 += "[" + yourcount + "]" + s + "\n";
						yourcount++;
					}

					if(t == enemy)
					{
						await user.SendMessageAsync("Which card you want to throw to enemy? [WITH NUMBER]\n" +
							"Your card : \n" + enemyresult2);
						int removedCard = 0;
						try
						{
							removedCard = Convert.ToInt32((await NextMessageAsync(enemyCriterion, TimeSpan.FromSeconds(30))).Content);
						}
						catch
						{
							removedCard = 69420;
							await Context.User.SendMessageAsync("Your enemy is skipped for either " +
								"not answering or wrong input.");
							await user.SendMessageAsync("Your turn is skipped now because you either" +
								" not answering or you typed the wrong input.");
							t = you;
						}

						if(removedCard != 69420)
						{
							lastEnemyCard = Deck.enemyCard[removedCard - 1];
							Deck.enemyCard.Remove(lastEnemyCard);
							await Context.User.SendMessageAsync("Your enemy throw " + lastEnemyCard + "!");
							await user.SendMessageAsync("You threw " + lastEnemyCard + "!\nWaiting for opponent turn...");

							if (Deck.enemyCard.Contains(lastEnemyCard.Substring(
								lastEnemyCard.IndexOf(" ", lastEnemyCard.IndexOf(" "))) + 1))
							{
								string result = "";

								for (int i = 0; i < 5; i++)
								{
									if (Deck.enemyCard[i].Substring(Deck.enemyCard[i].IndexOf(" ", Deck.enemyCard[i].IndexOf(" ") + 1) + 1)
										== lastEnemyCard.Substring(lastEnemyCard.IndexOf(" ", lastEnemyCard.IndexOf(" ") + 1) + 1))
									{
										result += "[" + (Deck.enemyCard.IndexOf(Deck.enemyCard[i]) + 1) + "] " + Deck.enemyCard[i] + "\n";
										yourMatchesCards.Add(Deck.enemyCard[i]);
									}
								}

								await Context.User.SendMessageAsync("It's your turn now!\n" +
									"Which card would you like to hit?" +
										"\n**__PLEASE CHOOSE BY NUMBER!__**\n Matches" +
										" type of cards : \n" + result);

								int yourCardN = 0;
								try
								{
									yourCardN = Convert.ToInt32((await NextMessageAsync(youCriterion, TimeSpan.FromSeconds(30))).Content);
								}
								catch
								{
									await Context.User.SendMessageAsync("You didn't put number or you didn't answer! Your card is removed by default.");

									yourCardN = 69420;

									Random idk = new Random();

									int h = idk.Next(0, yourMatchesCards.Count);

									string randomCard = yourMatchesCards[h];

									await Context.User.SendMessageAsync("Your enemy removed " + randomCard + "!");
									await user.SendMessageAsync("You removed " + randomCard + " as default.");

									Deck.yourCard.Remove(yourMatchesCards[h]);

									lastYourCard = yourMatchesCards[h];
								}

								if(yourCardN != 69420)
								{
									string str2 = lastEnemyCard.Substring(lastEnemyCard.IndexOf(" ") + 1);

									if (Deck.yourCard[yourCardN - 1].Contains(str2.Substring(str2.IndexOf("f") + 2)))
									{
										await Context.User.SendMessageAsync("You removed " + Deck.yourCard[yourCardN - 1]);
										await user.SendMessageAsync("Your enemy removed " + Deck.yourCard[yourCardN - 1] + "!");

										lastYourCard = Deck.yourCard[yourCardN - 1];

										Deck.yourCard.Remove(Deck.yourCard[yourCardN - 1]);
									}
									else
									{
										await Context.User.SendMessageAsync("The type of that card is different!");

										Random idk = new Random();

										int h = idk.Next(0, yourMatchesCards.Count);

										string randomCard = yourMatchesCards[h];

										await user.SendMessageAsync("Your enemy removed " + randomCard + "!");
										await Context.User.SendMessageAsync("You removed " + randomCard + " as default.");

										Deck.yourCard.Remove(yourMatchesCards[h]);

										lastYourCard = yourMatchesCards[h];
									}
								}
							}
							else
							{
								await Context.User.SendMessageAsync("You took the " + lastEnemyCard + " because you don't have!");
								await user.SendMessageAsync("Your enemy took the " + lastEnemyCard + "!");

								Deck.yourCard.Add(lastEnemyCard);
								t = enemy;
								higherY = true;
							}

							if (rarity(lastYourCard.Substring(0, lastYourCard.IndexOf(" "))) > rarity(lastEnemyCard.Substring(0, lastEnemyCard.IndexOf(" "))) && !higherY)
							{
								t = you;
							}
							else
							{
								t = enemy;
							}
						}
					}
					else
					{
						await Context.User.SendMessageAsync("Which card you want to throw to enemy? [WITH NUMBER]\n" +
							"Your card : \n" + yourresult2);
						int removedCard = 0;
						try
						{
							removedCard = Convert.ToInt32((await NextMessageAsync(youCriterion, TimeSpan.FromSeconds(30))).Content);
						}
						catch
						{
							removedCard = 69420;
							await user.SendMessageAsync("Your enemy is skipped for either " +
								"not answering or wrong input.");
							await Context.User.SendMessageAsync("Your turn is skipped now because you either" +
								" not answering or you typed the wrong input.");
							t = enemy;
						}

						if (removedCard != 69420)
						{
							lastYourCard = Deck.yourCard[removedCard - 1];
							await user.SendMessageAsync("Your enemy throw " + lastYourCard + "!");
							await Context.User.SendMessageAsync("You threw " + lastYourCard + "!\nWaiting for opponent turn...");

							if (Deck.enemyCard.Contains(lastYourCard.Substring(
								lastYourCard.IndexOf(" ", lastYourCard.IndexOf(" "))) + 1))
							{
								string result = "";

								for (int i = 0; i < 5; i++)
								{
									if (Deck.enemyCard[i].Substring(Deck.enemyCard[i].IndexOf(" ", Deck.enemyCard[i].IndexOf(" ") + 1) + 1)
										== lastYourCard.Substring(lastYourCard.IndexOf(" ", lastYourCard.IndexOf(" ") + 1) + 1))
									{
										result += "[" + (Deck.enemyCard.IndexOf(Deck.enemyCard[i]) + 1) + "] " + Deck.enemyCard[i] + "\n";
										enemyMatchesCards.Add(Deck.yourCard[i]);
									}
								}

								await user.SendMessageAsync("It's your turn now!\n" +
									"Which card would you like to hit?" +
										"\n**__PLEASE CHOOSE BY NUMBER!__**\n Matches" +
										" type of cards : \n" + result);

								int yourCardN = 0;
								try
								{
									yourCardN = Convert.ToInt32((await NextMessageAsync(enemyCriterion, TimeSpan.FromSeconds(30))).Content);
								}
								catch
								{
									await user.SendMessageAsync("You didn't put number or you didn't answer! Your card is removed by default.");

									yourCardN = 69420;

									Random idk = new Random();

									int h = idk.Next(0, enemyMatchesCards.Count);

									string randomCard = enemyMatchesCards[h];

									await Context.User.SendMessageAsync("Your enemy removed " + randomCard + "!");
									await user.SendMessageAsync("You removed " + randomCard + " as default.");

									Deck.enemyCard.Remove(enemyMatchesCards[h]);
								}

								if (yourCardN != 69420)
								{
									string str2 = lastYourCard.Substring(lastYourCard.IndexOf(" ") + 1);

									if (Deck.enemyCard[yourCardN - 1].Contains(str2.Substring(str2.IndexOf("f") + 2)))
									{
										await user.SendMessageAsync("You removed " + Deck.enemyCard[yourCardN - 1]);
										await Context.User.SendMessageAsync("Your enemy removed " + Deck.enemyCard[yourCardN - 1] + "!");

										lastEnemyCard = enemyCard[yourCardN - 1];

										enemyCard.Remove(enemyCard[yourCardN - 1]);

									}
									else
									{
										await user.SendMessageAsync("The type of that card is different!");

										Random idk = new Random();

										int h = idk.Next(0, enemyMatchesCards.Count);

										string randomCard = enemyMatchesCards[h];

										await Context.User.SendMessageAsync("Your enemy removed " + randomCard + "!");
										await user.SendMessageAsync("You removed " + randomCard + " as default.");

										Deck.enemyCard.Remove(enemyMatchesCards[h]);

										lastEnemyCard = enemyMatchesCards[h];
									}
								}
							}
							else
							{
								await user.SendMessageAsync("You took the " + lastEnemyCard + " because you don't have!");
								await Context.User.SendMessageAsync("Your enemy took the " + lastEnemyCard + "!");
								higherE = true;
								t = you;
							}

							if (rarity(lastYourCard.Substring(0, lastYourCard.IndexOf(" "))) > rarity(lastEnemyCard.Substring(0, lastEnemyCard.IndexOf(" "))) && !higherE)
							{
								t = you;
							}
							else
							{
								t = enemy;
							}
						}
					}
				}

				if(Deck.enemyCard.Count > 0)
				{
					await Context.User.SendMessageAsync("You lose!");
					await user.SendMessageAsync("You win!");
				}
				else if (Deck.yourCard.Count > 0)
				{
					await user.SendMessageAsync("You lose!");
					await Context.User.SendMessageAsync("You win!");
				}
			}
		}
    }
}