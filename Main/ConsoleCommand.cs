using Discord;
using Discord.Addons.Interactive;
using SampleApp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CowboyBot
{
	public class ConsoleCommand :InteractiveBase
	{
		public static bool boolMessage = false;
		public async Task ConsoleAsync()
		{
			while (true)
			{
				try {
					var guild = Commands.ClientPublic.GetGuild(715825843816890368);
					
					color(ConsoleColor.Blue);
					Console.Write("Input >> ");
					string str = Console.ReadLine();
					if (str.Contains("sendmessage"))
					{
						string channel = str.Substring(12, Extensions.NIndexOf(str, " ", 1) - 12);
						string message = str.Substring(Extensions.NIndexOf(str, " ", 1) + 1);

						foreach (var a in guild.Channels)
						{
							if (a.Name.ToLower().Contains(channel.ToLower()))
							{
								await (a as ITextChannel).SendMessageAsync(message);
								Console.WriteLine("Message succesfully sent.");
								break;
							}
						}
					}
					else if (str == "enablemsg")
					{
						boolMessage = true;
						Console.WriteLine("Message enabled.");
					}
					else if (str.Contains("ban"))
					{
						string user = str.Substring(4);
						foreach(var a in guild.Users)
						{
							if (a.ToString().ToLower().Contains(user.ToLower()))
							{
								await a.BanAsync();
							}
						}
					}
					else if (str.Contains("kick"))
					{
						string user = str.Substring(5);
						foreach (var a in guild.Users)
						{
							if (a.ToString().ToLower().Contains(user.ToLower()))
							{
								await a.KickAsync();
							}
						}
					}
					else if (str == "disablemsg")
					{
						boolMessage = false;
						Console.WriteLine("Message disabled.");
					}
					else if (str.Contains("senddm"))
					{
						string user = str.Substring(7, Extensions.NIndexOf(str, " ", 1) - 7);
						string message = str.Substring(Extensions.NIndexOf(str, " ", 1) + 1);

						foreach(var a in guild.Users)
						{
							if(a.ToString().ToLower().Contains(user.ToLower()))
							{
								await a.SendMessageAsync(message);
								Console.WriteLine("Message successfully sent to " + user);
							}
						}
					}
				}
				catch
				{
					color(ConsoleColor.Red);
					Console.WriteLine("\n[LOGS] Console Error " +
						"catched.");
				}
			}
		}

		public static void color(ConsoleColor c)
		{
			Console.ForegroundColor = c;
		}
	}

	public static class Extensions
	{
		public static int NIndexOf(this string str, string value, int n = 0)
		{
			int offset = str.IndexOf(value);
			for(int i = 0; i < n; i++)
			{
				if (offset == -1) return -1;
				offset = str.IndexOf(value, offset + 1);
			}
			return offset;
		}
	}
}
