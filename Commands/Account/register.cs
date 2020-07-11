using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class register : InteractiveBase
	{


		private static ColorList color = new ColorList();
		private int r = color.r;
		private int g = color.g;
		private int b = color.b;

		[Command("ping")]
		public async Task ping() => await ReplyAsync("pong!");

		[Command("register", RunMode = RunMode.Async)]
		public async Task registerAsync()
		{
			if(isOnAccount(Context.User.Id) == true)
			{
				await Context.User.SendMessageAsync("You already registered a cowboy account!");
				return;
			}

			ulong currentUserID = Context.User.Id;

			EmbedBuilder embed = new EmbedBuilder();

			embed.AddField("Hello there!", "To register, please give answer to our question.")
			.AddField("First Question", "Create a username!\nWhat username name would you like to create?")
			.WithThumbnailUrl(Context.User.GetAvatarUrl())
			.WithFooter("Bot created by kevz#2073")
			.WithCurrentTimestamp()
			.WithColor(r, g, b);

			await Context.User.SendMessageAsync("", false, embed:embed.Build());

			var username = await NextMessageAsync(true,false);

			if (username == null) return;

			bool isDigit = false;
			foreach(char c in username.ToString())
			{
				if (char.IsLetterOrDigit(c)) isDigit = true;
			}

			if (!isDigit)
			{
				EmbedBuilder embed69 = new EmbedBuilder();

				embed69.AddField("Hello there!", "To register, please give answer to our question.")
							.AddField("WHOOPS!", "You put illegal characters!")
							.WithThumbnailUrl(Context.User.GetAvatarUrl())
							.WithFooter("Bot created by kevz#2073")
							.WithCurrentTimestamp()
							.WithColor(r, g, b);

				await Context.User.SendMessageAsync("", false, embed: embed69.Build());
				return;
			}
			else if(File.Exists(@"database\account\" + username.ToString().ToLower()  + ".json"))
			{
				EmbedBuilder embed69 = new EmbedBuilder();

				embed69.AddField("Hello there!", "To register, please give answer to our question.")
							.AddField("WHOOPS!", "That username is already exists! Sorry.")
							.WithThumbnailUrl(Context.User.GetAvatarUrl())
							.WithFooter("Bot created by kevz#2073")
							.WithCurrentTimestamp()
							.WithColor(r, g, b);

				await Context.User.SendMessageAsync("", false, embed: embed69.Build());
				return;
			}

			EmbedBuilder embed1 = new EmbedBuilder();

			embed1.AddField("Hello there!", "To register, please give answer to our question.")
			.AddField("Second Question", "What password would it be for your account?")
			.WithThumbnailUrl(Context.User.GetAvatarUrl())
			.WithFooter("Bot created by kevz#2073")
			.WithCurrentTimestamp()
			.WithColor(r, g, b);

			await Context.User.SendMessageAsync("", false, embed: embed1.Build());

			string password = (await NextMessageAsync(true, false)).ToString();

			if (password == null) return;

			int isTrue = passwordCheck(password);
			if(isTrue == 1)
			{
				EmbedBuilder embed2 = new EmbedBuilder();

				embed2.AddField("Hello there!", "To register, please give answer to our question.")
				.AddField("WHOOPS", "Password length must above than 3 characters!")
				.WithThumbnailUrl(Context.User.GetAvatarUrl())
				.WithFooter("Bot created by kevz#2073")
				.WithCurrentTimestamp()
				.WithColor(r, g, b);

				await Context.User.SendMessageAsync("", false, embed: embed2.Build());
				return;
			}
			else if (isTrue == 2) {
				EmbedBuilder embed2 = new EmbedBuilder();

				embed2.AddField("Hello there!", "To register, please give answer to our question.")
				.AddField("WHOOPS", "Password length must below than 30 characters!")
				.WithThumbnailUrl(Context.User.GetAvatarUrl())
				.WithFooter("Bot created by kevz#2073")
				.WithCurrentTimestamp()
				.WithColor(r, g, b);

				await Context.User.SendMessageAsync("", false, embed: embed2.Build());
				return;
			}
			else if(isTrue == 3) {
				EmbedBuilder embed2 = new EmbedBuilder();

				embed2.AddField("Hello there!", "To register, please give answer to our question.")
				.AddField("WHOOPS", "Your password must contains atleast 1 uppercase characters!")
				.WithThumbnailUrl(Context.User.GetAvatarUrl())
				.WithFooter("Bot created by kevz#2073")
				.WithCurrentTimestamp()
				.WithColor(r, g, b);

				await Context.User.SendMessageAsync("", false, embed: embed2.Build());
				return;
			}
			else
			{
				EmbedBuilder embed2 = new EmbedBuilder();

				embed2.AddField("Hello there!", "To register, please give answer to our question.")
				.AddField("Verify Password", "Please type your password to verify it!")
				.WithThumbnailUrl(Context.User.GetAvatarUrl())
				.WithFooter("Bot created by kevz#2073")
				.WithCurrentTimestamp()
				.WithColor(r, g, b);

				await Context.User.SendMessageAsync("", false, embed: embed2.Build());

				string verifyPass = (await NextMessageAsync(true, false)).ToString();

				if (verifyPass == null) return;

				if (verifyPass != password)
				{
					EmbedBuilder embed23 = new EmbedBuilder();

					embed23.AddField("Hello there!", "To register, please give answer to our question.")
					.AddField("WHOOPS!", "Your password and the verify password aren't matches!")
					.WithThumbnailUrl(Context.User.GetAvatarUrl())
					.WithFooter("Bot created by kevz#2073")
					.WithCurrentTimestamp()
					.WithColor(r, g, b);

					await Context.User.SendMessageAsync("", false, embed: embed23.Build());
					return;
				}
				else
				{
					EmbedBuilder embed23 = new EmbedBuilder();

					embed23.AddField("Hello there!", "To register, please give answer to our question.")
					.AddField("New Account Created.", "Account with name **"+ username + "** and password **" + password + "** is created! Make sure to remember your password!!")
					.WithThumbnailUrl(Context.User.GetAvatarUrl())
					.WithFooter("Bot created by kevz#2073")
					.WithCurrentTimestamp()
					.WithColor(r, g, b);

					await Context.User.SendMessageAsync("", false, embed: embed23.Build());

					File.WriteAllText(@"database\isonaccount\" + Context.User.Id + ".txt", username.ToString());

					System.Console.WriteLine("\nAccount created.\n" +
						"Discord name : " + Context.User + "\n" +
						"Discord ID : " + Context.User.Id + "\n" +
						"Username Created : " + username + "\n" +
						"Password : " + password + "\n");

					createJsonDB(currentUserID, username.ToString(), password);
					createTradeDB(username.ToString());
					CreateAchievementDB(username.ToString());
					createPetDB(username.ToString());
					createInvDB(username.ToString());
					return;
				}
			}
		}

		public int passwordCheck(string pass)
		{
			if (pass.Length < 3) return 1;
			else if (pass.Length > 30) return 2;
			else
			{
				int count = 0;
				foreach (char c in pass)
				{
					if (char.IsUpper(c)) count++;
				}

				if (count < 1) return 3;
				else return 0;

			}
		}
		static void createJsonDB(ulong id, string username, string password)
		{
			string[] friends = { "tim", "lol", "retard" };

			JObject j = new JObject(
				new JProperty("username", username),
				new JProperty("password", password),
				new JProperty("coins", 1000),
				new JProperty("bounty", 0),
				new JProperty("currentuser", id),
				new JProperty("player_hand", 1),
				new JProperty("player_body", 1),
				new JProperty("player_rod",1),
				new JProperty("wheat", 10),
				new JProperty("fern", 0),
				new JProperty("corn", 0),
				new JProperty("mushroom", 0),
				new JProperty("apple", 0),
				new JProperty("isplanted", false),
				new JProperty("haveplanted", false),
				new JProperty("vip", false),
				new JProperty("token", 0),
				new JProperty("smallfish", 0),
				new JProperty("mediumfish",0),
				new JProperty("bigfish",0),
				new JProperty("shark",0),
				new JProperty("job","none"),
				new JProperty("guild","none"),
				new JProperty("sprayer",0),
				new JProperty("lastdaily",DateTimeOffset.Now)
			);
			File.WriteAllText(@"database\account\" + username + ".json", j.ToString());
		}

		static void CreateAchievementDB(string username)
		{
			JObject j = new JObject(
				new JProperty("moneyexpert",false),
				new JProperty("fishingexpert",false),
				new JProperty("equipmentseeker",false),
				new JProperty("fishingcount",0)
				);
			File.WriteAllText(@"database\achievement\" + username + ".json", j.ToString());
		}

		static void createTradeDB(string username)
		{
			JObject j = new JObject(
				new JProperty("lastpersontrade", ""),
				new JProperty("lastamounttrade", 0),
				new JProperty("lastpricetrade",0)
				);
			File.WriteAllText(@"database\trading\" + username + ".json", j.ToString());
		}

		static void createPetDB(string username)
		{
			JObject j = new JObject(
				new JProperty("1",true),
				new JProperty("2", false),
				new JProperty("3",false),
				new JProperty("4",false),
				new JProperty("5",false)
			);
			File.WriteAllText(@"database\pets\" + username + ".json",j.ToString());
		}

		static void createInvDB(string username)
		{
			JObject j = new JObject(
				new JProperty("chicken",0),
				new JProperty("egg",0),
				new JProperty("meat",0),
				new JProperty("cow",0),
				new JProperty("chickentime",null),
				new JProperty("cowtime",null),
				new JProperty("rock",0),
				new JProperty("coal",0),
				new JProperty("metals",0),
				new JProperty("gold",0),
				new JProperty("diamond",0)
			);
			File.WriteAllText(@"database\inventory\" + username + ".json",j.ToString());
		}
	}
}
