using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static CowboyBot.tokenprices;

namespace CowboyBot
{
	public class STP : ModuleBase<SocketCommandContext>
	{
		public async Task sendTokenPrice()
		{
			Console.WriteLine(Context.Channel.Id);

			var a = Context.Guild.Channels.FirstOrDefault(x => x.Id == 718442891600461846) as IMessageChannel;

			if (isIncrease)
			{

				await a.SendMessageAsync("token price increased by " + nPrice + ". The price is now " + tokenPrice);
			}
			else
			{
				await a.SendMessageAsync("Token price decreased by " + nPrice + "." +
					" The price is now " + tokenPrice);
			}
		}
	}
}
