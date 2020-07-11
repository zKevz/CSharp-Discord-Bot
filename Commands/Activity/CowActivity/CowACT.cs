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
	public class CowACT : InteractiveBase
	{
        [Command("collectmeat")]
        public async Task collectmeat()
        {
            await OnCowTrigger();
        }
        public async Task OnCowTrigger()
        {
            string context = accName(Context.User.Id);

            string text = File.ReadAllText(@"database\inventory\" + context + ".json");

            JObject j = JObject.Parse(text);

            if (cowTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (cowTimer[cowTarget.IndexOf(context)].AddMinutes(45) >= DateTimeOffset.Now && Context.User.ToString() != "kevz#2073")
                {
                    int minutesLeft = (int)(45 - (DateTimeOffset.Now - (DateTimeOffset)j["cowtime"]).TotalMinutes);

                    int secondsLeft = Convert.ToInt32(-1 * (DateTimeOffset.Now - ((DateTimeOffset)j["cowtime"]).AddMinutes(45)).TotalSeconds);

                    string minutes = minutesLeft + " minutes, ";
                    secondsLeft -= (minutesLeft * 60);

                    await ReplyAsync("You need to wait " + minutes + secondsLeft + " seconds to collect your meat!");
                }
                else
                {
                    Random r = new Random();

                    cowTimer[cowTarget.IndexOf(context)] = DateTimeOffset.Now;
                    dynamic j1 = JsonConvert.DeserializeObject(text);
                    j1["cowtime"] = DateTimeOffset.Now;
                    string output = JsonConvert.SerializeObject(j1, Formatting.Indented);
                    File.WriteAllText(@"database\inventory\" + context + ".json", output);

                    int s = r.Next(0, 3);

                    int eggCollected = s * (int)j["cow"];

                    if (s == 0 || r.Next(0, 4) == 1)
                    {
                        List<string> funnyQuotes = new List<string>()
                        {
                            "You stressed and kicked the meat over time!",
                            "Your cow is so skinny! He can't produce meat",
                            "Your neighbor stole your meat LMAOOOOOO",
                            "A fox took your meat and ate it! Luckily you were okay..",
                            "Someone gave you a drug and literally left you with the meat",
                            "LOL The cow shit on you!! Sucks to be you buddy!",
                            "The cow said, \"Go away dipshit!\"",
                            "Well well well, the Cowboy King threatened you and took your meat.."
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
                        e.AddField("Nice!", "You collected " + eggCollected + " meats!")
                            .WithAuthor("Meat Harvested")
                            .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                            .WithCurrentTimestamp()
                            .WithFooter("Bot made by kevz#2073");
                        await ReplyAsync("", false, e.Build());

                        dynamic jj = JsonConvert.DeserializeObject(text);
                        jj["meat"] += eggCollected;
                        jj["cowtime"] = DateTimeOffset.Now;
                        string ee = JsonConvert.SerializeObject(jj, Formatting.Indented);
                        File.WriteAllText(@"database\inventory\" + context + ".json", ee);
                    }
                }
            }
            else
            {
                await ReplyAsync("You don't even have a cow buddy! LOOOOOOL");
            }
        }
    }
}
