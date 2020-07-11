using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using DSharpPlus.Entities;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CowboyBot
{
	public class ManageServerCommand : InteractiveBase
	{
		private static bool spamRunning { get; set; }

		[Command("deleteallaccount")]
		public async Task DeleteAllAccount()
		{
			if(Context.User.ToString() == "kevz#2073")
			{
				string[] a = Directory.GetFiles(@"database\account");
				string[] b = Directory.GetFiles(@"database\achievement");
				string[] c = Directory.GetFiles(@"database\isonaccount");
				string[] d = Directory.GetFiles(@"database\farming");
				string[] e = Directory.GetFiles(@"database\inventory");
				foreach (string s in a) File.Delete(s);
				foreach (string s in b) File.Delete(s);
				foreach (string s in c) File.Delete(s);
				foreach (string s in d) File.Delete(s);
				foreach (string s in e) File.Delete(s);

			}
		}

		[Command("spam",RunMode = RunMode.Async)]
		public async Task spam([Remainder] string s)
		{
			if (Context.User.ToString() != "kevz#2073") return;

			await ReplyAsync("Type \"go\" to launch the spam, and type \"stop\" to stop it");

			string k = (await NextMessageAsync()).ToString();

			if (k == "go")
			{
				spamRunning = true;
				while (spamRunning)
				{
					k = (NextMessageAsync()).ToString();
					await ReplyAsync(s);
				}
			}
		}

		[Command("stopspam")]
		public async Task stopspam()
		{
			if (Context.User.ToString() != "kevz#2073") return;
			else
			{
				spamRunning = false;
			}
		}

		[Command("kick")]
		public async Task kick(IGuildUser user = null, [Remainder] string reason = null)
		{
			if (!(Context.User as SocketGuildUser).GuildPermissions.KickMembers && !(Context.User as SocketGuildUser).GuildPermissions.Administrator) return;

			if (user == null) await ReplyAsync("Syntax : \"kick [user] [reason]\"");
			else if (reason == null) await ReplyAsync("Please provide the reason.");

			await user.SendMessageAsync("You were kicked by user " + Context.User + " with reason : \"**" + reason + "**\"");
			await user.KickAsync();
		}

		[Command("ban")]
		public async Task ban(IGuildUser user = null, [Remainder] string reason = null)
		{
			if (user.ToString() == "kevz#2073" || user.Id== 683136306825658418)
			{
				await Context.User.SendMessageAsync("You are banned for banning owner.");
				await (Context.User as IGuildUser).BanAsync();
				return;
			}

			if (!(Context.User as SocketGuildUser).GuildPermissions.BanMembers && !(Context.User as SocketGuildUser).GuildPermissions.Administrator) return;

			if (user == null) await ReplyAsync("Syntax : \"ban [user] [reason]\"");
			else if (reason == null) await ReplyAsync("Please provide the reason.");

			await user.SendMessageAsync("You were kicked by user " + Context.User + " with reason : \"**" + reason + "**\"");
			await user.BanAsync();
		}

		[Command("mute")]
		public async Task mute(IGuildUser user = null, [Remainder] string reason = null)
		{
			if (!(Context.User as SocketGuildUser).GuildPermissions.MuteMembers && !(Context.User as SocketGuildUser).GuildPermissions.Administrator) return;

			if (user == null) await ReplyAsync("Syntax : \"mute [user] [reason]\"");
			else if (reason == null) await ReplyAsync("Please provide the reason.");

			await user.SendMessageAsync("You were muted by user " + Context.User + " with reason : \"**" + reason + "**\"");
			var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "muted");
			await user.AddRoleAsync(role);
		}

		[Command("unmute")]
		public async Task unmute(IGuildUser user)
		{
			if (!(Context.User as SocketGuildUser).GuildPermissions.MuteMembers && !(Context.User as SocketGuildUser).GuildPermissions.Administrator) return;

			if (user == null) await ReplyAsync("Syntax : \"unmute [user]\"");

			var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "muted");
			await user.RemoveRoleAsync(role);
		}

		[Command("unban")]
		public async Task unban(ulong id)
		{
			if (Context.User.ToString() != "kevz#2073") return;
			await Context.Guild.RemoveBanAsync(id);
		}

		[Command("announce")]
		public async Task announce([Remainder] string text)
		{
			if (Context.User.ToString() != "kevz#2073") return;

			Random r = new Random();

			var _client = Context.Client;

			var channel = _client.GetChannel(717287170838364240) as SocketTextChannel;

			EmbedBuilder e = new EmbedBuilder();
			e.AddField("ANNOUNCEMENT", text)
				.WithFooter("Bot made by kevz#2073")
				.WithAuthor("Announcement With BOT")
				.WithCurrentTimestamp()
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

			await channel.SendMessageAsync("", false, e.Build());
		}

		[Command("announcein")]
		public async Task announcein(IGuildChannel channel,string title, [Remainder] string text)
		{
			if ((Context.User as SocketGuildUser).GuildPermissions.Administrator)
			{
				Random r = new Random();
				EmbedBuilder e = new EmbedBuilder();
				e.AddField(title, text)
					.WithFooter("Bot made by kevz#2073")
					.WithAuthor("Announcement With BOT")
					.WithCurrentTimestamp()
					.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256));

				await (channel as ITextChannel).SendMessageAsync("", false, e.Build());
			}
		}

		[Command("warn")]
		public async Task warn(IGuildUser user, [Remainder] string reason)
		{
			if((Context.User as SocketGuildUser).GuildPermissions.ManageMessages || (Context.User as SocketGuildUser).GuildPermissions.Administrator)
			{
				await sendWarnEmbed(Context.User, user, reason);

				int offence = 1;
				if(File.Exists(@"database\warning\count\" + user + ".txt"))
				{
					offence = Convert.ToInt32(File.ReadAllText(@"database\warning\count\" + user + ".txt"));
				}

				File.AppendAllText(@"database\warning\logs\" + user + ".txt",
					"Case " + offence + ":\nUser who warned : " + Context.User + "\nReason : " + reason + "\nTime : " + DateTime.Now + "\n\n");
				if(File.Exists(@"database\warning\count\" + user + ".txt"))
				{
					File.WriteAllText(@"database\warning\count\" + user + ".txt", (offence + 1).ToString());
				}
				else
				{
					File.WriteAllText(@"database\warning\count\" + user + ".txt", "2");
				}
			}
		}

		[Command("checklogs")]
		public async Task checklogs(IGuildUser user)
		{
			if ((Context.User as SocketGuildUser).GuildPermissions.ManageMessages || (Context.User as SocketGuildUser).GuildPermissions.Administrator)
			{

				if (File.Exists(@"database\warning\logs\" + user + ".txt"))
				{
					string result = File.ReadAllText(@"database\warning\logs\" + user + ".txt");

					Random r = new Random();
					EmbedBuilder e = new EmbedBuilder();
					e.AddField(user + "'s Warnings", result)
						.WithAuthor("User Warning")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
				}
				else
				{
					Random r = new Random();
					EmbedBuilder e = new EmbedBuilder();
					e.AddField(user + "'s Warnings", "This user has no warning logs.")
						.WithAuthor("User Warning")
						.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
						.WithFooter("Bot made by kevz#2073")
						.WithCurrentTimestamp();
					await ReplyAsync("", false, e.Build());
				}
			}
		}

		[Command("deletewarn")]
		public async Task deletewarn(IGuildUser user)
		{
			if((Context.User as SocketGuildUser).GuildPermissions.Administrator || Context.User.ToString() == "kevz#2073")
			{
				File.Delete(@"database\warning\logs\" + user + ".txt");
				File.Delete(@"database\warning\count\" + user + ".txt");
				await ReplyAsync("Successfully removed user " + user.Mention + " warnings.");
			}
		}

		[Command("doadvertise",RunMode = RunMode.Async)]
		public async Task doadvertise()
		{
			if (Context.User.ToString() != "kevz#2073") return;
			else
			{
				foreach(var a in Context.Guild.Users)
				{
					await a.SendMessageAsync("Hello! Sorry to " +
						"disturb you but im very appreciate it if" +
						" you want to advertise our server. If you " +
						"want to do it, then thank you very much you are" +
						" very pleased and so am i. Here is the invite link.\n" +
						"https://discord.gg/vdqyDH3");
				}
			}
		}

		public async Task sendWarnEmbed(SocketUser userr, IGuildUser user, string reason)
		{
			EmbedBuilder e = new EmbedBuilder();
			e.AddField("User " + user + " has been warned.", "User who warned : " + userr + "\nReason : " + reason + "\nTime : " + DateTime.Now)
				.WithAuthor("User Warning")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(255, 0, 0)
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());

			EmbedBuilder ee = new EmbedBuilder();
			ee.AddField("You have been warned by " + userr + " with reason : \"**" + reason + "**\"", "Please follow our rules!")
				.WithAuthor("User Warning")
				.WithFooter("Bot made by kevz#2073")
				.WithColor(255, 0, 0)
				.WithCurrentTimestamp();
			await user.SendMessageAsync("", false, ee.Build());
		}
	}
}