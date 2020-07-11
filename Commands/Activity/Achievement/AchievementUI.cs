using System;
using System.Collections.Generic;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CowboyBot.Timer;
using static CowboyBot.AchievementInfo;

namespace CowboyBot
{
	public class AchievementUI : InteractiveBase
	{
		[Command("achievement")]
		public async Task Achievement(IGuildUser user = null) 
		{
			Random r = new Random();

			string UserTarget = "";
			if (user == null)
				UserTarget = accName(Context.User.Id);
			else
				UserTarget = accName(user.Id);

			EmbedBuilder e = new EmbedBuilder();
			e.AddField(UserTarget + "'s Achievement", "\n\n" +
				GetMoneyExpert(UserTarget) + "\nGet by having 100.000 Coins\n\n" +
				GetFishingExpert(UserTarget) + "\nGet by fishing 300 times\n\n" + 
				GetEquipmentSeeker(UserTarget) + "\nGet by buying all the rarest equ" +
				"ipment")
				.WithAuthor("Cowboy's Achievement")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await ReplyAsync("", false, e.Build());
		}
	}
}
