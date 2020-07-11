using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;
using static CowboyBot.Timer;
using Discord;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
	public class DailyCoins : InteractiveBase
	{
		public static List<DateTimeOffset> dailyTimer = new List<DateTimeOffset>();
		public static List<string> dailyTarget = new List<string>();

        public static int minutesLeft { get; set; }
        public static int secondsLeft { get; set; }
        public static int hoursLeft { get; set; }

		[Command("daily")]
		public async Task Daily()
		{
            Random r = new Random();
            string username = accName(Context.User.Id);
            if(!lastDaily(username,60 * 24))
			{
                string minutes = minutesLeft + " minutes,";
                string hours = hoursLeft + " hours, ";

                if (minutesLeft == 0) minutes = "";
                if (hoursLeft == 0) hours = "";

                EmbedBuilder e = new EmbedBuilder();
                e.AddField("WHOOPS", "You need to wait " + hours +
                    minutes + secondsLeft + " seconds to claim daily coins " +
                    "again!")
                    .WithAuthor("COOLDOWN")
                    .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                    .WithFooter("Bot made by kevz#2073")
                    .WithCurrentTimestamp();
                await ReplyAsync("", false, e.Build());
			}
			else
			{
                EmbedBuilder e = new EmbedBuilder();
                e.AddField("Congrats!", "You claimed 100 Coins!")
                    .WithAuthor("Cowboy's Daily")
                    .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                    .WithFooter("Bot made by kevz#2073")
                    .WithCurrentTimestamp();
                await ReplyAsync("", false, e.Build());

                dynamic j = JsonConvert.DeserializeObject(File.ReadAllText(
                    @"database\account\" + username + ".json"));
                j["coins"] += 100;
                string output = JsonConvert.SerializeObject(j, Formatting.Indented);

                File.WriteAllText(@"database\account\" + username + ".json", output);
            }
		}

        public static bool lastDaily(string context, int time)
        {
            if (dailyTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (dailyTimer[dailyTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + context + ".json"));

                    minutesLeft = (int)(time - (DateTimeOffset.Now - (DateTimeOffset)j["lastdaily"]).TotalMinutes);
                    hoursLeft = (int)(time / 60 - (DateTimeOffset.Now - (DateTimeOffset)j["lastdaily"]).TotalHours);
                    secondsLeft = (int)(time * 60- (DateTimeOffset.Now - (DateTimeOffset)j["lastdaily"]).TotalSeconds);

                    secondsLeft -= (minutesLeft * 60);
                    minutesLeft -= (hoursLeft * 60);

                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    dailyTimer[dailyTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                dailyTarget.Add(context);
                dailyTimer.Add(DateTimeOffset.Now);
                return true;
            }
        }
    }
}
