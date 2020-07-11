using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.Addons.Interactive;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using static CowboyBot.farm;

namespace CowboyBot
{
	public class Timer : InteractiveBase
	{
        public static int minutesLeft { get; set; }
        public static int secondsLeft { get; set; }

        public static List<DateTimeOffset> timerxd = new List<DateTimeOffset>();
        public static List<string> targetxd = new List<string>();

        public static List<DateTimeOffset> plantTimer = new List<DateTimeOffset>();
        public static List<DateTime> plantTimer2 = new List<DateTime>();
        public static List<string> plant = new List<string>();
        public static List<string> planter = new List<string>();
        public static List<int> plantAmount = new List<int>();

        public static List<DateTimeOffset> stealtimer = new List<DateTimeOffset>();
        public static List<string> stealtarget = new List<string>();

        public static List<DateTimeOffset> jailtimer = new List<DateTimeOffset>();
        public static List<string> jailtarget = new List<string>();

        public static List<DateTimeOffset> fishingTimer = new List<DateTimeOffset>();
        public static List<string> fishingTarget = new List<string>();

        public static List<DateTimeOffset> chickenTimer = new List<DateTimeOffset>();
        public static List<string> chickenTarget = new List<string>();

        public static List<DateTimeOffset> cowTimer = new List<DateTimeOffset>();
        public static List<string> cowTarget = new List<string>();

        public static List<DateTimeOffset> betTimer = new List<DateTimeOffset>();
		public static List<string> betTarget = new List<string>();

        public static List<DateTimeOffset> miningTimer = new List<DateTimeOffset>();
        public static List<string> miningTarget = new List<string>();

        public static List<DateTimeOffset> jobTimer = new List<DateTimeOffset>();
        public static List<string> jobTarget = new List<string>();

        public static List<DateTimeOffset> jobTimerXD = new List<DateTimeOffset>();
        public static List<string> jobTargetXD = new List<string>();

        public static List<DateTimeOffset> lastInviteDuelTimer = new List<DateTimeOffset>();
        public static List<string> lastInviteDuelTarget = new List<string>();

        public static List<DateTimeOffset> mathTimer = new List<DateTimeOffset>();
        public static List<string> mathTarget = new List<string>();

        public static List<DateTimeOffset> sprayTimer = new List<DateTimeOffset>();
        public static List<string> sprayTarget = new List<string>();

        public static List<DateTimeOffset> hunttimer = new List<DateTimeOffset>();
        public static List<string> hunttarget = new List<string>(); public static bool lastFarm(string context, int time)
		{
            if (targetxd.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (timerxd[targetxd.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    timerxd[targetxd.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                targetxd.Add(context);
                timerxd.Add(DateTimeOffset.Now);
                return true;
            }
        }

        public static bool lastSteal(string context, int time)
        {
            if (stealtarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (stealtimer[stealtarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    stealtimer[stealtarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                stealtarget.Add(context);
                stealtimer.Add(DateTimeOffset.Now);
                return true;
            }
        }
        public static bool lastMining(string context, int time)
        {
            if (miningTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (miningTimer[miningTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    miningTimer[miningTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                miningTarget.Add(context);
                miningTimer.Add(DateTimeOffset.Now);
                return true;
            }
        }
        public static bool lastSpray(string context, int time)
        {
            if (sprayTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (sprayTimer[sprayTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    sprayTimer[sprayTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                
                return true;
            }
        }
        public static bool lastMath(string context, int time)
        {
            if (mathTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (mathTimer[mathTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    mathTimer[mathTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                mathTarget.Add(context);
                mathTimer.Add(DateTimeOffset.Now);
                return true;
            }
        }
        public static bool lastInviteDuel(string context, int time)
        {
            if (lastInviteDuelTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (lastInviteDuelTimer[lastInviteDuelTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    lastInviteDuelTimer[lastInviteDuelTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                /*lastInviteDuelTarget.Add(context);
                lastInviteDuelTimer.Add(DateTimeOffset.Now);*/
                return true;
            }
        }

        public static bool lastFishing(string context, int time)
        {
            if (fishingTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (fishingTimer[fishingTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    fishingTimer[fishingTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                fishingTarget.Add(context);
                fishingTimer.Add(DateTimeOffset.Now);
                return true;
            }
        }

        public static bool lastRegisterJob(string context, int time)
        {
            if (jobTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (jobTimer[jobTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    jobTimer[jobTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                jobTarget.Add(context);
                jobTimer.Add(DateTimeOffset.Now);
                return true;
            }
        }

        public static bool lastJob(string context, int time)
        {
            if (jobTargetXD.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (jobTimerXD[jobTargetXD.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    jobTimerXD[jobTargetXD.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                jobTargetXD.Add(context);
                jobTimerXD.Add(DateTimeOffset.Now);
                return true;
            }
        }
        public static bool lastHunt(string context, int time)
        {
            if (hunttarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (hunttimer[hunttarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    hunttimer[hunttarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                hunttarget.Add(context);
                hunttimer.Add(DateTimeOffset.Now);
                return true;
            }
        }

        public static bool lastBet(string context, int time)
        {
            if (betTarget.Contains(context))
            {
                //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                if (betTimer[betTarget.IndexOf(context)].AddMinutes(time) >= DateTimeOffset.Now)
                {
                    int minutesLeftt = (int)(betTimer[betTarget.IndexOf(context)].AddMinutes(time) - DateTimeOffset.Now).TotalMinutes;
                    //If enough time hasn't passed, reply letting them know how much longer they need to wait, and end the code.
                    int secondsLeftt = (int)(betTimer[betTarget.IndexOf(context)].AddMinutes(time) - DateTimeOffset.Now).TotalSeconds;

                    minutesLeft = minutesLeftt;
                    secondsLeft = secondsLeftt;

                    return false;
                }
                else
                {
                    //If enough time has passed, set the time for the user to right now.
                    betTimer[betTarget.IndexOf(context)] = DateTimeOffset.Now;
                    return true;
                }
            }
            else
            {
                //If they've never used this command before, add their username and when they just used this command.
                betTarget.Add(context);
                betTimer.Add(DateTimeOffset.Now);
                return true;
            }
        }
    }
}
