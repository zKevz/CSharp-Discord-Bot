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
using static CowboyBot.Deck;

namespace CowboyBot.Card
{
	public class CardMG : InteractiveBase
	{
		[Command("pcard", RunMode = RunMode.Async)]
		public async Task pcard(IGuildUser user)
		{
			if (!isOnAccount(user.Id))
			{
				await ReplyAsync("That user doesn't even have a " +
					"cowboy account!");
				return;
			}

			await ReplyAsync("Sending request to " + user.Mention);

			await user.SendMessageAsync("user " + Context.User + " asked you to play card!" +
				"\nType \"accept\" if you accept. You have 30 seconds.");

			EnsureFromUserCriterion enemyCriterion = new EnsureFromUserCriterion(user);
			EnsureFromUserCriterion youCriterion = new EnsureFromUserCriterion(Context.User);

			List<string> yourMatchesCard = new List<string>();
			List<string> enemyMatchesCard = new List<string>();

			string targetuseranswer = ""; try { targetuseranswer = (await NextMessageAsync(enemyCriterion, TimeSpan.FromSeconds(30))).ToString(); }
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
				Random r = new Random();
				string yourResult = "";
				string enemyResult = "";

				try
				{
					createDeck();
					Deck.Shuffle(card);
				}
				catch { Console.WriteLine("error at making deck"); }

				int countforloops = 1;
				int round = 1;

				try
				{
					for (int i = 0; i < 5; i++)
					{
						int yourRandom = r.Next(0, card.Count);
						yourCard.Add(card[yourRandom]);
						yourResult += "[" + countforloops + "] " + card[yourRandom] + "\n";
						card.RemoveAt(yourRandom);

						int enemyRandom = r.Next(0, card.Count);
						enemyCard.Add(card[enemyRandom]);
						enemyResult += "[" + countforloops + "] " + card[enemyRandom] + "\n";
						card.RemoveAt(enemyRandom);
						countforloops++;
					}
				}
				catch
				{
					Console.WriteLine("error at for loops");
					await Context.User.SendMessageAsync("Card Game Error. Please try again later.");
					await user.SendMessageAsync("Card Game Error. Please try again later.");
					return;
				}

				await sendEmbedToYou("Your card: ", yourResult);
				await sendEmbedToEnemy("Your card:", enemyResult, user);

				turn t = enemy;

				while(enemyCard.Count > 0 && yourCard.Count > 0)
				{
					if(round == 1)
					{
						if(t == you)
						{
							await sendEmbedToEnemy("You are in the second turn!",
								"Waiting for your opponent...", user);
						}
						else
						{
							await sendEmbedToYou("You are in the first turn!",
								"What card would you like to hit?");
						}
					}

					if (t == you)
					{

					}
					else
					{

					}
				}
			}
		}

		public async Task sendEmbedToYou(string text, string text2)
		{
			Random r = new Random();

			EmbedBuilder enemyEmbed2 = new EmbedBuilder();

			enemyEmbed2.AddField(text, text2)
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithAuthor("Card Games")
				.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("",false,enemyEmbed2.Build());
		}

		public async Task sendEmbedToEnemy(string text1,string text2, IGuildUser user)
		{
			Random r = new Random();

			EmbedBuilder enemyEmbed2 = new EmbedBuilder();

			enemyEmbed2.AddField(text1, text2)
				.WithFooter("Bot made by kevz#2073")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithAuthor("Card Games")
				.WithCurrentTimestamp();
			await user.SendMessageAsync("", false, enemyEmbed2.Build());
		}

		public static string getSuit(string card)
		{
			string s = card.Substring(card.IndexOf(" ", card.IndexOf(" ") + 1) + 1);
			Console.WriteLine(s);
			return s;
		}
	}
}
