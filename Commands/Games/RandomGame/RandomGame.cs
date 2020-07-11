using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class RandomGame : InteractiveBase
	{
		[Command("creategame",RunMode = RunMode.Async)]
		public async Task CreateGame()
		{
			await ReplyAsync("" +
				"User " + Context.User.Mention
				 + " hosting a random game!\n" +
				 "Type \"c join\" to join.");

			int count = 0;
			List<string> userJoined = new List<string>();

			while(count < 5)
			{
				try
				{
					var joinMessage = (
						await NextMessageAsync(false, true,
						TimeSpan.FromSeconds(10)));
					if (joinMessage.ToString().ToLower() == "c join")
					{
						if (!userJoined.Contains(accName(joinMessage.Author.Id)))
						{
							count++;
							userJoined.Add(accName(
								joinMessage.Author.Id));
							await ReplyAsync("" +
								"User " + joinMessage.Author
								.Mention + " joined the game.[" +
								(5 - count) + "] left");
						}
					}
				}
				catch { continue; }
			}

			foreach(string s in userJoined)
				Console.WriteLine(s);

		}
	}
}