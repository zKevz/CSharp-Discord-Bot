using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class help : InteractiveBase
	{
		[Command("help")]
		[Alias("command","commands","helps","commandinfo","infocommand")]
		public async Task Help()
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("Hello! The help command is split into 5 sections.\n" +
				"Please choose by using \"c help[category]\"\n" +
				"Example : \"c helpgames\"", "\u200b")
				.AddField("[1] Games", "Usage : \"c helpgames\"\n" +
				"Display all information about games commands.")
				.AddField("[2] Account", "Usage : \"c helpaccount\"\n" +
				"Display all information about account commands.")
				.AddField("[3] Token", "Usage : \"c helptoken\"\n" +
				"Display all information about token commands.")
				.AddField("[4] Guild", "Usage : \"c helpguild\"\n" +
				"Display all information about guild/server commands.")
				.AddField("[5] Activity", "Usage : \"c helpactivity\"\n" +
				"Display all information about activity commands")
				.WithAuthor("Cowboy's Help")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());
		}
		/*[Command("help")]
		[Alias("command", "commands", "help", "commandinfo", "infocommand")]
		public async Task HelpCommand()
		{
			Random r = new Random();
			try
			{
				EmbedBuilder e = new EmbedBuilder();
				e.AddField("-------------------------------------------------------\n" +
					"Bot Info", "\n\n- Prefix : c [WITH SPACE]\n- Bot made by kevz#2073\n" +
					"- Example usage : \"c farm\"\n" +
					"-------------------------------------------------------")
					.AddField("Account Command: \n[1] Login", "To login to your cowboy's account.")
					.AddField("[2] Register", "To register a cowboy account.\n-------------------------------------------------------")


					.AddField("Activity Command: \n[1] Farm", "To plant a seed or to harvest a seed.")
					.AddField("[2] Sell [seed name]", "To sell your seeds with coins.")
					.AddField("[3] Shop", "To see all items that is being sell in shop.")
					.AddField("[4] Buy [type] [amount]", "To buy items in the shop.")
					.AddField("[5] Steal [user]", "To steal the user coins. [NOTE: You can" +
					" go to jail (50 %) ]")
					.AddField("[6] collectegg", "To collect egg [If you have chicken]")
					.AddField("[7] collectmeat", "To collect meat [If you have cow]")
					.AddField("[8] fishing", "To fishing")
					.AddField("[9] robbank", "To rob a bank. Chance is 1% to succeed [prize 5.000 - 10.000 coins]")
					.AddField("[10] joblist", "To see all the job lists")
					.AddField("[11] work", "To work [if you have a job]")
					.AddField("[12] createguild", "To create a guild. Costs 5.000 coins")
					.AddField("[13] invite [user]", "To invite a user to ur guild")
					.AddField("[14] leaderboard", "To see the leaderboards by coins")
					.AddField("[15] bountyleaderboard", "To see the leaderboards by bounty points")
					.AddField("[16] daily", "To claim your daily coins")
					.AddField("[17] boostharvest", "To boost your planted seeds by 30 minutes. [If you have more than one " +
					"Cowboy's Sprayer]")
					.AddField("[18] mining", "To goes mining! Can get rock, coal, metals, gold and diamond." +
					"\n-------------------------------------------------------")


					.AddField("Token Command: \n[1] Buytoken [amount]", "To buy cowboy's token")
					.AddField("[2] tokeninfo", "To see the current price of cowboy's token")
					.AddField("[3] trade [user] [amount] [price]", "To sell your [amount] token for [price] coins per token\n-------------------------------------------------------")


					.AddField("Games Command: \n[1] qq [bet]", "To bet your coins with QQ Games.")
					.AddField("[2] bj [bet]", "To bet your coins with BJ Games.")
					.AddField("[3] casino [bet]", "To bet your coins with CSN Games.")
					.AddField("[4] playrps [user]", "To play Rock Paper Scissor with a user.")
					.AddField("[5] hunt", "To play cowboy's hunting. Can get 100 - 500 coins.")
					.AddField("[6] buylotteryticket", "To buy a lottery ticket, prize is 200.000 coins and price is 10 coins each")
					.AddField("[7] duel [user]", "To duel with the user [Mostly like \"c hunt\"]")
					.AddField("[8] slotmachine", "To play slot machine, cost 50 coins.")
					.AddField("[9] mathgames", "To play a infinite math games. Each round gives u 5 coins.")
					.AddField("[10] playtictactoe [user]", "To play tic tac toe with a user")
					.AddField("[11] playai", "To play tic tac toe with AI")
					.AddField("[12] playcoin [bet]", "To play a coin games (HEAD / TAIL)")
					.AddField("[13] mathbattle [user]", "To play a math battle with a user" +
					"\n-------------------------------------------------------")


					.AddField("Guild Command: \n[1] kick [user] [reason]", "To kick a user. [Need permission]")
					.AddField("[2] ban [user] [reason]", "To ban a user. [Need permission]")
					.AddField("[3] mute [user] [reason]", "To mute a user. [Need permission]")
					.AddField("[4] announce [text]", "To announce something in #announcement [Need permission]")
					.AddField("[5] warn [user] [reason]", "To warn a user [Need permission]")
					.AddField("[6] checklogs [user]", "To check the warn logs of the user [Need permission]")
					.AddField("[7] color [role]", "To get yourself the color you like.")
					.AddField("[8] creategiveaway", "To create a giveaway. [Need permission]")
					.AddField("[9] joingiveaway", "To join a giveaway." +
					"\n-------------------------------------------------------")
					.WithAuthor("Command List")
					.WithFooter("Bot made by kevz#2073")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithCurrentTimestamp();
				await Context.User.SendMessageAsync("", false, e.Build());
			}
			catch (Exception e) { Console.WriteLine(e.Message); }
		}*/

		[Command("helptoken")]
		[Alias("help token")]
		public async Task HelpToken()
		{
			EmbedBuilder e = new EmbedBuilder();
			Random r = new Random();
			e.AddField("[1] Buytoken [amount]", "To buy cowboy's token")
					.AddField("[2] tokeninfo", "To see the current price of cowboy's token")
					.AddField("[3] trade [user] [amount] [price]", "To sell your [amount] token for [price] coins per token\n-------------------------------------------------------")
					.WithAuthor("Token Command\u200b")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());
		}

		[Command("helpaccount")]
		[Alias("help account")]
		public async Task HelpAccount()
		{
			EmbedBuilder e = new EmbedBuilder();
			Random r = new Random();
			e.AddField("[1] Login", "To login to your cowboy's account.")
				.AddField("[2] Register", "To register a cowboy account.")
				.AddField("[3] enableaap", "To enable Account Advanced Protection")
				.AddField("[4] disableaap","To disable Account Advanced Protection\n-------------------------------------------------------")
				.WithAuthor("Account Command\u200b")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());

		}

		[Command("helpgames")]
		[Alias("help games","helpgame","help game")]
		public async Task HelpGames()
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("[1] qq [bet]", "To bet your coins with QQ Games.")
					.AddField("[2] bj [bet]", "To bet your coins with BJ Games.")
					.AddField("[3] casino [bet]", "To bet your coins with CSN Games.")
					.AddField("[4] playrps [user]", "To play Rock Paper Scissor with a user.")
					.AddField("[5] hunt", "To play cowboy's hunting. Can get 100 - 500 coins.")
					.AddField("[6] buylotteryticket", "To buy a lottery ticket, prize is 200.000 coins and price is 10 coins each")
					.AddField("[7] duel [user]", "To duel with the user [Mostly like \"c hunt\"]")
					.AddField("[8] slotmachine", "To play slot machine, cost 50 coins.")
					.AddField("[9] mathgames", "To play a infinite math games. Each round gives u 5 coins.")
					.AddField("[10] playtictactoe [user]", "To play tic tac toe with a user")
					.AddField("[11] playai", "To play tic tac toe with AI")
					.AddField("[12] playcoin [bet]", "To play a coin games (HEAD / TAIL)")
					.AddField("[13] mathbattle [user]", "To play a math battle with a user" +
					"\n-------------------------------------------------------")
					.WithAuthor("Games Command\u200b")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());
		}

		[Command("helpguild")]
		[Alias("help guild")]
		public async Task HelpGuild()
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("[1] kick [user] [reason]", "To kick a user. [Need permission]")
					.AddField("[2] ban [user] [reason]", "To ban a user. [Need permission]")
					.AddField("[3] mute [user] [reason]", "To mute a user. [Need permission]")
					.AddField("[4] announce [text]", "To announce something in #announcement [Need permission]")
					.AddField("[5] warn [user] [reason]", "To warn a user [Need permission]")
					.AddField("[6] checklogs [user]", "To check the warn logs of the user [Need permission]")
					.AddField("[7] color [role]", "To get yourself the color you like.")
					.AddField("[8] creategiveaway", "To create a giveaway. [Need permission]")
					.AddField("[9] joingiveaway", "To join a giveaway." +
					"\n-------------------------------------------------------")
					.WithAuthor("Guild Command\u200b")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());
		}

		[Command("helpactivity")]
		[Alias("help activity")]
		public async Task HelpActivity()
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("[1] Farm", "To plant a seed or to harvest a seed.")
					.AddField("[2] Sell [seed name]", "To sell your seeds with coins.")
					.AddField("[3] Shop", "To see all items that is being sell in shop.")
					.AddField("[4] Buy [type] [amount]", "To buy items in the shop.")
					.AddField("[5] Steal [user]", "To steal the user coins. [NOTE: You can" +
					" go to jail (50 %) ]")
					.AddField("[6] collectegg", "To collect egg [If you have chicken]")
					.AddField("[7] collectmeat", "To collect meat [If you have cow]")
					.AddField("[8] fishing", "To fishing")
					.AddField("[9] robbank", "To rob a bank. Chance is 1% to succeed [prize 5.000 - 10.000 coins]")
					.AddField("[10] joblist", "To see all the job lists")
					.AddField("[11] work", "To work [if you have a job]")
					.AddField("[12] createguild", "To create a guild. Costs 5.000 coins")
					.AddField("[13] invite [user]", "To invite a user to ur guild")
					.AddField("[14] leaderboard", "To see the leaderboards by coins")
					.AddField("[15] bountyleaderboard", "To see the leaderboards by bounty points")
					.AddField("[16] daily", "To claim your daily coins")
					.AddField("[17] boostharvest", "To boost your planted seeds by 30 minutes. [If you have more than one " +
					"Cowboy's Sprayer]")
					.AddField("[18] mining", "To goes mining! Can get rock, coal, metals, gold and diamond.")
					.AddField("[19] sellmaterial [type] [amount]","To sell your mining materials.")
					.AddField("[20] givecoin [user] [amount]","To give a user coins" +
					"\n-------------------------------------------------------")
					.WithAuthor("Activity Command\u200b")
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
					.WithFooter("Bot made by kevz#2073")
					.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());
		}
	}
}
