using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class Giveaway : InteractiveBase
	{

		private System.Threading.Timer GTimer;
		private int GiveawayTimeFixed { get; set; }
		private DateTimeOffset GiveawayStartTimeFixed { get; set; }
		private ITextChannel GiveawayChannel { get; set; }
		private string GiveawayItems { get; set; }
		private bool isMadeYet { get; set; }
		private IUserMessage message { get; set; } = null;
		public int count = 0;
		public static List<ulong> ParticipedUser { get; set; } = new List<ulong>();

		[Command("creategiveaway",RunMode = RunMode.Async)]
		public async Task CreateGiveaway(SocketGuildChannel channel)
		{
			if ((Context.User as SocketGuildUser).GuildPermissions.Administrator)
			{
				bool channelFound = false;
				foreach (var a in Context.Guild.Channels)
					if (a.Name.ToLower() == channel.Name) { channelFound = true;
						channel = a;
						var s = a as ITextChannel;
						GiveawayChannel = s;
					}

				if (!channelFound)
				{
					await ReplyAsync("It's a invalid channel!");
					return;
				}

				await ReplyAsync("Item / things name ?");

				string name = (await NextMessageAsync()).ToString();

				await ReplyAsync("Time ?");

				string time = (await NextMessageAsync()).ToString();

				int timeDelay = 0;
				if (time.ToLower()[time.Length - 1] == 's')
					timeDelay = 1000;
				else if (time.ToLower()[time.Length - 1] == 'm')
					timeDelay = 1000 * 60;
				else if (time.ToLower()[time.Length - 1] == 'h')
					timeDelay = 1000 * 60 * 60;
				else if (time.ToLower()[time.Length - 1] == 'd')
					timeDelay = 1000 * 60 * 60 * 24;
				else
					await ReplyAsync("Invalid type of times.");

				int timeSetup = Convert.ToInt32(time.Substring(0, time.Length - 1));

				GiveawayStartTimeFixed = DateTimeOffset.Now;

				DateTimeOffset DateTimeFixed = DateTimeOffset.Now;

				GiveawayTimeFixed = (timeDelay * timeSetup);

				GiveawayItems = name;

				File.Create(@"database\giveaway\startgiveaway.txt");

				File.WriteAllText(@"database\giveaway\133769time.txt",(DateTimeOffset.Now).ToString());
				File.WriteAllText(@"database\giveaway\133769.txt", (GiveawayTimeFixed).ToString());

				isMadeYet = false;

				count++;

				LoadGiveaway();
			}
		}

		[Command("joingiveaway")]
		public async Task JoinGiveaway()
		{
			if (File.Exists(@"database\giveaway\startgiveaway.txt"))
			{
				if (ParticipedUser.Contains(Context.User.Id))
				{
					await ReplyAsync("You already joined the giveaway!");
					return;
				}
				await ReplyAsync("You joined the giveaway!");
				File.AppendAllText(@"database\giveaway\participants.txt", Context.User.Id + ",");
				ParticipedUser.Add(Context.User.Id);
			}
			else
			{
				await ReplyAsync("There is no giveaway now.");
			}
		}

		void LoadGiveaway()
		{
			GTimer = new System.Threading.Timer(SendGiveaway, null, 0, 5000);
		}

		public void SendGiveaway(object s)
		{
			g();
		}

		public async Task g()
		{
			DateTimeOffset dtos = Convert.ToDateTime(File.ReadAllText(@"database\giveaway\133769time.txt"));
			int time = Convert.ToInt32(File.ReadAllText(@"database\giveaway\133769.txt"));

			if (time >= (DateTimeOffset.Now - dtos).TotalMilliseconds)
			{
				int hoursLeft = 0;
				if (time > 1000 * 60 * 60)
				{
					hoursLeft = (int)(time / (1000 * 60 * 60) - (DateTimeOffset.Now - dtos).TotalHours);
				}
				int minutesLeft = (int)(time / (1000 * 60) - (DateTimeOffset.Now - dtos).TotalMinutes);
				int secondsLeft = (int)(time / 1000 - (DateTimeOffset.Now - dtos).TotalSeconds);

				secondsLeft -= (minutesLeft * 60);
				minutesLeft -= (hoursLeft * 60);

				string minutes = minutesLeft + " minutes, ";
				string hours = hoursLeft + " hours, ";

				if (minutesLeft == 0) minutes = "";
				if (hoursLeft == 0) hours = "";

				EmbedBuilder e = new EmbedBuilder();
				e.AddField("Giveaway!\nType \"c joingiveaway\" to join!", "Hosted by : " + Context.User)
					.AddField("Giveaway Items : " + GiveawayItems, "Time Left : " +
					hours + minutes + secondsLeft + " seconds")
					.WithAuthor("Giveaway Time!")
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();

				if (isMadeYet == false)
				{
					message = await GiveawayChannel.SendMessageAsync("", false, e.Build());
					isMadeYet = true;
					if (count == 1) return;
				}
				else
				{
					EmbedBuilder eEE = new EmbedBuilder();
					eEE.AddField("Giveaway!\nType \"c joingiveaway\" to join!", "Hosted by : " + Context.User)
						.AddField("Giveaway Items : " + GiveawayItems, "Time Left : " +
						hours + minutes + secondsLeft + " seconds")
						.WithAuthor("Giveaway Time!")
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();

					await message.ModifyAsync(x => x.Embed = eEE.Build());

					/*"Giveaway!\nType \"c joingiveaway\" to join!\n" +
					"Hosted by : " + Context.User + "\n" +
					"Giveaway Items : " + GiveawayItems + "\n" + "Time Left : " +
					hours + minutes + secondsLeft + " seconds"*/
				}
				count++;
			}
			else
			{
				if (File.Exists(@"database\giveaway\participants.txt") == false)
				{
					await ReplyAsync("Noone participated the giveaways!");
					File.Delete(@"database\giveaway\133769.txt");
					File.Delete(@"database\giveaway\133769time.txt");
					File.Delete(@"database\giveaway\participants.txt");
					File.Delete(@"database\giveaway\startgiveaway.txt");

					isMadeYet = false;

					EmbedBuilder eEEee = new EmbedBuilder();
					eEEee.AddField("Giveaway!\nType \"c joingiveaway\" to join!", "Hosted by : " + Context.User)
						.AddField("Giveaway Items : " + GiveawayItems, "Time Left : ENDED\n" +
						"Winners : `No participants joined`")
						.WithAuthor("Giveaway Time!")
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();

					await message.ModifyAsync(x => x.Embed = eEEee.Build());
					return;
				}

				string participants = File.ReadAllText(@"database\giveaway\participants.txt");

				string[] ChoooseWinner = participants.Split(',');
				
				Random r = new Random();
				int a = r.Next(0, ChoooseWinner.Length);
				ulong winner = Convert.ToUInt64(ChoooseWinner[a]);

				IGuildUser user = null;

				foreach(var ass in Context.Guild.Users)
				{
					if (ass.Id == winner)
					{
						await GiveawayChannel.SendMessageAsync("The winner is " + ass.Mention + "!\n" +
							"Congrats!!");
						user = ass;
						GTimer.Dispose();
					}
				}
				File.Delete(@"database\giveaway\133769.txt");
				File.Delete(@"database\giveaway\133769time.txt");
				File.Delete(@"database\giveaway\participants.txt");
				File.Delete(@"database\giveaway\startgiveaway.txt");

				isMadeYet = false;

				EmbedBuilder eEE = new EmbedBuilder();
				eEE.AddField("Giveaway!\nType \"c joingiveaway\" to join!", "Hosted by : " + Context.User)
					.AddField("Giveaway Items : " + GiveawayItems, "Time Left : ENDED\n" +
					"Winners : " + user)
					.WithAuthor("Giveaway Time!")
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();

				await message.ModifyAsync(x => x.Embed = eEE.Build());
			}
		}
	}
}
