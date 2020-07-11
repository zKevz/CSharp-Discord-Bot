using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.Addons.Interactive;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;

namespace CowboyBot
{
	public class messagecommands : InteractiveBase
	{
		[Command("del",RunMode = RunMode.Async)]
		public async Task del(int amount)
		{
			if (amount <= 0) return;

			var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount).FlattenAsync();
			var filteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14);

			var count = filteredMessages.Count();

			if (count == 0) await ReplyAndDeleteAsync("There is nothing messages to be deleted!", timeout:TimeSpan.FromSeconds(3));
			else
			{
				try
				{
					await (Context.Channel as ITextChannel).DeleteMessagesAsync(filteredMessages);
				}
				catch { }
				await ReplyAndDeleteAsync($"Done! Removed {count} {(count > 1? "messages" : "message")}", timeout:TimeSpan.FromSeconds(1));
				await Context.Message.DeleteAsync();
				await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);
			}
		}

		[Command("color")]
		public async Task colorRole(SocketRole role)
		{
			if (role.Permissions.Administrator || role.Name == "gangsta") return;
			else if (role.Position >= 10) return;
			else
			{
				try
				{
					var r = Context.Guild.Roles.FirstOrDefault(x => x.Name == role.ToString());
					if ((Context.User as SocketGuildUser).Roles.Contains(r))
					{
						try
						{
							await (Context.User as SocketGuildUser).RemoveRoleAsync(r);
							await ReplyAsync("You remove your color!");
						}
						catch
						{
							Console.WriteLine("not found?");
							await ReplyAsync("Remove color role failed. Role not found.");
						}
					}
					else
					{
						try
						{
							await (Context.User as SocketGuildUser).AddRoleAsync(r);
							await ReplyAsync("Your color is now " + role.Mention);
						}
						catch
						{
							Console.WriteLine("not found?");
							await ReplyAsync("Adding color role failed. Role not found.");
						}
					}
				}
				catch { Console.WriteLine("error"); }
			}
		}
	}
}
