using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class ChickenACT : InteractiveBase
	{
        [Command("collectegg")]
        public async Task collectegg()
        {
            await OnChickenTrigger();
        }
        public async Task OnChickenTrigger()
        {
            string[] a = Directory.GetFiles(@"database\inventory");
            
            string context = accName(Context.User.Id);

            string text = File.ReadAllText(@"database\inventory\" + context + ".json");

            JObject j = JObject.Parse(text);

            if (chickenTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (chickenTimer[chickenTarget.IndexOf(context)].AddMinutes(30) >= DateTimeOffset.Now && Context.User.ToString() != "kevz#2073")
                {
                    int minutesLeft = (int)(30 - (DateTimeOffset.Now - (DateTimeOffset)j["chickentime"]).TotalMinutes);

                    int secondsLeft = Convert.ToInt32(-1 * (DateTimeOffset.Now - ((DateTimeOffset)j["chickentime"]).AddMinutes(30)).TotalSeconds);

                    string minutes = minutesLeft + " minutes, ";
                    secondsLeft -= (minutesLeft * 60);

                    await ReplyAsync("You need to wait " + minutes + secondsLeft + " seconds to collect your egg!");
                }
                else
                {
                    Random r = new Random();

                    chickenTimer[chickenTarget.IndexOf(context)] = DateTimeOffset.Now;
                    dynamic j1 = JsonConvert.DeserializeObject(text);
                    j1["chickentime"] = DateTimeOffset.Now;
                    string output = JsonConvert.SerializeObject(j1, Formatting.Indented);
                    File.WriteAllText(@"database\inventory\" + context + ".json", output);

                    int s = r.Next(0, 3);

                    int eggCollected = s * (int)j["cow"];

                    if (s == 0 || r.Next(0, 4) == 1)
                    {
                        List<string> funnyQuotes = new List<string>()
                        {
                            "LOL You accidentally dropped the egg! You got nothing!",
                            "Your chicken hates you and gave you nothing!",
                            "You got " + r.Next(30,101) + " eggs but your neighbour stole it... sucks to be you",
                            "Well, well, well the egg became a baby chicken! But shit his mother literally ate" +
                            " him..",
                            "Another cowboy borrowed the egg and suddenly ran away! LMAO",
                            "Your chicken yelled, \"You ain't got nothing deeply ass shit!\" lol sucks to be you",
                            "Do you know what is good ? Be patient! You got 0 egg LMAO",
                            "What did you do? You accidentally kicked the egg.."
                        };

                        EmbedBuilder e = new EmbedBuilder();
                        e.AddField("LOOOOL", funnyQuotes[r.Next(0, 8)])
                            .WithAuthor("Sucks to be you")
                            .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                            .WithCurrentTimestamp()
                            .WithFooter("Better luck next time!");
                        await ReplyAsync("", false, e.Build());
					}
					else
					{
                        EmbedBuilder e = new EmbedBuilder();
                        e.AddField("Nice!", "You collected " + eggCollected + " eggs!")
                            .WithAuthor("Eggs Harvested")
                            .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                            .WithCurrentTimestamp()
                            .WithFooter("Bot made by kevz#2073");
                        await ReplyAsync("", false, e.Build());

                        dynamic jj = JsonConvert.DeserializeObject(text);
                        jj["egg"] += eggCollected;
                        jj["chickentime"] = DateTimeOffset.Now;
                        string ee = JsonConvert.SerializeObject(jj, Formatting.Indented);
                        File.WriteAllText(@"database\inventory\" + context + ".json", ee);
                    }
                }
            }
            else
            {
                await ReplyAsync("You don't even have an egg buddy! LOOOOOOL");
            }
        }
    }
}
