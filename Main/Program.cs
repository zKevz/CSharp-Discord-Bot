using CowboyBot;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static CowboyBot.Timer;
using static CowboyBot.tokenprices;
using static CowboyBot.UserInfo;
using static CowboyBot.LotteryUI;

namespace SampleApp
{
	class Commands : InteractiveBase
    {
        static void Main(string[] args)
            => new Commands().MainAsync().GetAwaiter().GetResult();
        private DiscordSocketClient client;
        private CommandService commands;
        public static DiscordSocketClient ClientPublic;
        private IServiceProvider services;
        private System.Threading.Timer timer;
        private System.Threading.Timer TimerEvent;
        private System.Threading.Timer TimerForLottery;
        private System.Threading.Timer TimerForAchievement;
        public static int FirstNum { get; set; }
        public static int SecondNum { get; set; }
        public static int Prize { get; set; }
        public static bool isEvent { get; set; }
        public bool boolMessage = false; 

        public static int CountForLogin { get; set; } = 0;
        private static int LOL
        {
            get; set;
        }

        public async Task MainAsync()
        {
            
            //LoginForBot();
            var token = "your-token-here";

            client = new DiscordSocketClient();

            client.Log += log =>
            {
                Console.WriteLine("\n[LOGS] " +log.ToString());
                return Task.CompletedTask;
            };

            Console.ForegroundColor = ConsoleColor.Green;

            //await client.SetGameAsync("c help");
            await client.SetStatusAsync(UserStatus.DoNotDisturb);


            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();

            DateTimeOffset a = DateTimeOffset.Now;

            addChickenTimerDB();

            commands = new CommandService();
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            client.UserJoined += AnnounceJoinedUser;

            client.UserLeft += userLeftMain;

            client.MessageReceived += HandleCommandAsync;

            ClientPublic = client;

            //LoadLottery();

            LoadDailyDatabase();

			client.Connected += Client_Connected;

			client.LoggedOut += Client_LoggedOut;

            Thread.Sleep(5000);

			load();
			LoadEvent();

            await new ConsoleCommand().ConsoleAsync();

            await Task.Delay(-1);
        }

		public async Task Client_LoggedOut()
		{
            var tokenxd = "NzE1ODIxMjc0MzM2ODU0MDI2.XtC8DQ._6RTcXD4avNGT8SZj4cwT9emH84";

            await client.LoginAsync(TokenType.Bot, tokenxd);
            await client.StartAsync();
        }

		public async Task Client_Connected()
		{
			Console.WriteLine("[LOGS] " +DateTime.Now.ToLongTimeString() + "           Bot connected.");
		}

		public void LoginForBot()
		{
            if(CountForLogin == 2)
			{
				Console.WriteLine("You entered the wrong username / password " +
                    "2 times! Shutting down the application");
                Environment.Exit(1);
                return;
			}

            Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Username : ");
            string s = Console.ReadLine();
			Console.Write("Password : ");
            string p = Console.ReadLine();

            if(s == "kevz" && p == "yeeharetardfuck69420")
			{
                Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("\nACCESS GRANTED!\n" +
                    "Please wait..\n");
			}
			else{
                Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("\nWrong Login!\n");
                CountForLogin++;
                LoginForBot();
			}
		}

        public async Task HandleCommandAsync(SocketMessage m)
        {
            if (m.Author.ToString() != "Cowboy#2612")
            {
                if (ConsoleCommand.boolMessage)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n[LOGS]   Channel name : " + m.Channel + ",   Author name : " +
                        m.Author + ",   Message Content : " + m.Content);
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }

            if (!(m is SocketUserMessage msg)) return;
            if (msg.Author.IsBot) return;
            int argPos = 0;

            var channelll = client.GetChannel(717265650929893376) as SocketGuildChannel;

            if (!(msg.HasStringPrefix("c ", ref argPos)) && !(msg.HasStringPrefix("C ", ref argPos))) return;

            if (!File.Exists(@"database\account\" + accName(msg.Author.Id) + ".json") && msg.Content.ToLower().Substring(0, 2) == "c " && !msg.Content.ToLower().Contains("c register") && !msg.Content.ToLower().Contains("c help") && !msg.Content.ToLower().Contains("c login") && 
                !msg.Content.ToLower().Contains("c enableaap") && !msg.Content.ToLower().Contains("c disableaap"))
            {
                await noacc(msg.Author);
                return;
            }

            Steal s = new Steal();

            if (File.Exists(@"database\jailed\" + accName(msg.Author.Id) + ".json"))
            {
                await s.jailTime(accName(msg.Author.Id), msg.Author as IGuildUser);
            }

            if (s.isOnJail(accName(m.Author.Id)) == true) return;

            else if (msg.Channel.ToString() == ("@" + msg.Author) && !msg.Content.ToLower()
                .Contains("c enableaap") && !msg.Content.ToLower().Contains("c disableaap")) return;

            var context = new SocketCommandContext(client, msg);
            await commands.ExecuteAsync(context, argPos, services);
        }

        public async Task userLeftMain(SocketGuildUser user)
        {
            var channel = client.GetChannel(718686602095886336) as SocketTextChannel;
            EmbedBuilder e = new EmbedBuilder();
            e.AddField("User " + user + " left the server!", "RIP")
                .WithAuthor("Bot made by kevz#2073")
                .WithColor(255, 255, 255)
                .WithThumbnailUrl(user.GetAvatarUrl())
                .WithCurrentTimestamp();
            await channel.SendMessageAsync("", false, e.Build());
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user)
        {
            var channel = client.GetChannel(718684657461493810) as SocketTextChannel;
            EmbedBuilder e = new EmbedBuilder();
            e.AddField("Welcome to Cowboy Bot's server, " + user.ToString().Substring(0, user.ToString().IndexOf("#")) + "!\n",
                "Check #announcement to see what is this server " +
                "about!")
                .WithAuthor("Bot made by kevz#2073")
                .WithColor(255, 255, 255)
                .WithThumbnailUrl(user.GetAvatarUrl())
                .WithCurrentTimestamp();
            await channel.SendMessageAsync("", false, e.Build());
        }
        
        void LoadAchievement()
		{
            TimerForAchievement = new System.Threading.Timer(AchievementChecker, null, 0, 1000 * 60 * 30);
		}

        void AchievementChecker(object s)
		{
            string[] a = Directory.GetFiles(@"database\account");
            string[] sxd = Directory.GetFiles(@"database\achievement");

            foreach (var c in sxd) 
            {
                // c achievement b account
                string usernamexd = c.Substring(21, c.IndexOf(".") - 21);
                if (usernamexd == "null") continue;
                JObject jjxd = JObject.Parse(File.ReadAllText(c));
                foreach (var b in a)
                {
                    string username = b.Substring(17, b.IndexOf(".") - 17);
                    JObject j = JObject.Parse(File.ReadAllText(b));

                    if (!(bool)jjxd["moneyexpert"])
                    {

                        if ((int)j["coins"] >= 100000)
                        {
                            var guild = ClientPublic.GetGuild(715825843816890368);
                            foreach (var w in guild.Channels)
                            {
                                if (w.Id == 720269935833776239)
                                {
                                    foreach (var q in guild.Users)
                                    {
                                        if ((ulong)j["currentuser"] == q.Id)
                                        {
                                            (w as ITextChannel).SendMessageAsync("User " + q.Mention + " with account name " + username + " has achieved " +
                                                "achievement `Money Expert!`");

                                            dynamic xd = JsonConvert.DeserializeObject(File.ReadAllText(c));
                                            xd["moneyexpert"] = true;
                                            string output = JsonConvert.SerializeObject(xd, Formatting.Indented);
                                            File.WriteAllText(c, output);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!(bool)jjxd["fishingexpert"])
                    {
                        if ((int)jjxd["fishingcount"] >= 300)
                        {
                            var guild = ClientPublic.GetGuild(715825843816890368);
                            foreach (var w in guild.Channels)
                            {
                                if (w.Id == 720269935833776239)
                                {
                                    foreach (var q in guild.Users)
                                    {
                                        if ((ulong)j["currentuser"] == q.Id)
                                        {
                                            (w as ITextChannel).SendMessageAsync("User " + q.Mention + " with account name " + username + " has achieved " +
                                                "achievement `Fishing Expert!`");

                                            dynamic xd = JsonConvert.DeserializeObject(File.ReadAllText(c));
                                            xd["fishingexpert"] = true;
                                            string output = JsonConvert.SerializeObject(xd, Formatting.Indented);
                                            File.WriteAllText(c, output);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!(bool)jjxd["equipmentseeker"])
                    {
                        if ((int)j["player_hand"] == 3 && (int)j["player_body"] == 3 && (int)j["player_rod"] == 3)
                        {
                            var guild = ClientPublic.GetGuild(715825843816890368);
                            foreach (var w in guild.Channels)
                            {
                                if (w.Id == 720269935833776239)
                                {
                                    foreach (var q in guild.Users)
                                    {
                                        if ((ulong)j["currentuser"] == q.Id)
                                        {
                                            (w as ITextChannel).SendMessageAsync("User " + q.Mention + " with account name " + username + " has achieved " +
                                        "achievement `Equipment Expert!`");

                                            dynamic xd = JsonConvert.DeserializeObject(File.ReadAllText(c));
                                            xd["equipmentseeker"] = true;
                                            string output = JsonConvert.SerializeObject(xd, Formatting.Indented);
                                            File.WriteAllText(c, output);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
		}

        void LoadLottery()
		{
            TimerForLottery = new System.Threading.Timer(new LotteryUI().SendLottery, null, 0, 1000 * 60 * 60);
		}

        void addChickenTimerDB()
		{
            string[] s = Directory.GetFiles(@"database\inventory");

            if (s.Length <= 0) return;

            foreach(string a in s)
			{
                string sss = File.ReadAllText(a);
                if (!sss.Contains("chickentime") && !sss.Contains("cowtime")) return;

                JObject j = JObject.Parse(sss);
                if((int)j["chicken"] > 0)
				{
                    if (j["chickentime"] == null)
                    {
                        chickenTarget.Add(a.Substring(19, a.IndexOf(".") - 19));

                        chickenTimer.Add(DateTimeOffset.Now);

                        dynamic jj = JsonConvert.DeserializeObject(a);
                        jj["chickentime"] = DateTimeOffset.Now;
                        string ee = JsonConvert.SerializeObject(jj, Formatting.Indented);
                        File.WriteAllText(a, ee);
                    }
                    else
                    {
                        chickenTarget.Add(a.Substring(19, a.IndexOf(".") - 19));

                        chickenTimer.Add((DateTimeOffset)j["chickentime"]);
                    }
				}
				if ((int)j["cow"] > 0)
				{
                    if (j["cowtime"] == null)
                    {
                        cowTarget.Add(a.Substring(19, a.IndexOf(".") - 19));

                        cowTimer.Add(DateTimeOffset.Now);

                        dynamic jj = JsonConvert.DeserializeObject(a);
                        jj["cowtime"] = DateTimeOffset.Now;
                        string ee = JsonConvert.SerializeObject(jj, Formatting.Indented);
                        File.WriteAllText(a, ee);
                    }
                    else
                    {
                        cowTarget.Add(a.Substring(19, a.IndexOf(".") - 19));

                        cowTimer.Add((DateTimeOffset)j["cowtime"]);
                    }
                }
			}
		}

        
        void LoadEvent()
		{
            TimerEvent = new System.Threading.Timer(EventPer10Mins, null, 0, 1000 * 60 * 10);
        }

        public static void EventPer10Mins(Object s)
		{
            var guild = ClientPublic.GetGuild(715825843816890368);

            foreach (var a in guild.Channels)
			{
                if(a.Id == 719825195648548944)
				{
                    Random r = new Random();
                    int firstNum = r.Next(30, 151);
                    int secondNum = r.Next(30, 151);
                    int prize = r.Next(1,5);

                    EmbedBuilder e = new EmbedBuilder();
                    e.AddField("EVENT", "What is " + firstNum + " + " + secondNum + "?\nTo answer" +
                        " do \"c answer [answer]\"\nWinner gets " + prize + " coins!")
                        .WithAuthor("Token Price")
                        .WithFooter("Bot made by kevz#2073")
                        .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                        .WithCurrentTimestamp();
                    (a as ITextChannel).SendMessageAsync("", false, e.Build());

                    FirstNum = firstNum;
                    SecondNum = secondNum;
                    Prize = prize;

                    isEvent = true;
                }
			}
		}
        void load()
        {
            tokenprices s = new tokenprices();

            timer = new System.Threading.Timer(priceChanged, null, 0, 1000 * 60 * 30);
        }

        public async Task noacc(SocketUser user)
		{
            Random r = new Random();

            EmbedBuilder e = new EmbedBuilder();
            e.AddField("Make An Account!", "You don't have a cowboy account!\n" +
                "Type \"c register\" to register one!")
                .WithAuthor("ACCOUNT NEEDED")
                .WithFooter("Bot made by kevz#2073")
                .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                .WithCurrentTimestamp();

            await user.SendMessageAsync("", false, e.Build());
            return;
		}
        public void OnChickenTrigger(Object OOOOOOOOOOOOOOOOOOOOOOO)
        {
            string[] a = Directory.GetFiles(@"database\inventory");


            foreach (string s in a)
            {
                string context = s.Substring(s.IndexOf(@"\", s.IndexOf(@"\") + 1) + 1);

                JObject j = JObject.Parse(s);
                if ((int)j["chicken"] > 0)
                {
                    if (chickenTarget.Contains(context))
                    {
                        //If they have used this command before, take the time the user last did something, add 5 seconds, and see if it's greater than this very moment.
                        if (timerxd[targetxd.IndexOf(context)].AddMinutes(30) >= DateTimeOffset.Now)
                        {
                            int minutesLeft = (int)(30 - (DateTimeOffset.Now - (DateTimeOffset)j["chickentime"]).TotalMinutes);

                            int secondsLeft = Convert.ToInt32(-1 * (DateTimeOffset.Now - ((DateTimeOffset)j["chickentime"]).AddMinutes(30)).TotalSeconds);

                            ReplyAsync("");
                        }
                        else
                        {
                            //If enough time has passed, set the time for the user to right now.
                            timerxd[targetxd.IndexOf(context)] = DateTimeOffset.Now;
                        }
                    }
                    else
                    {
                        //If they've never used this command before, add their username and when they just used this command.
                        targetxd.Add(context);
                        timerxd.Add(DateTimeOffset.Now);
                    }
                }
            }
        }

        void LoadDailyDatabase()
		{
            string[] a = Directory.GetFiles(@"database\account");

            if (a.Length <= 0) return;

            foreach(string s in a)
			{
                JObject j = JObject.Parse(File.ReadAllText(s));
                DailyCoins.dailyTarget.Add((string)j["username"]);
                DailyCoins.dailyTimer.Add((DateTimeOffset)j["lastdaily"]);
			}
		}

        public static void priceChanged(Object s)
        {
            if (File.Exists(@"database\token\token.txt"))
            {
                tokenPrice = Convert.ToInt32(File.ReadAllText(@"database\token\token.txt"));
            }
            else
            {
                tokenPrice = 20;
                File.WriteAllText(@"database\token\token.txt", "20");
            }

            Random r = new Random();
            int fiftypercent = r.Next(0, 3);
            int newPrice = 0;
            if (tokenPrice >= 50)
            {
                newPrice = r.Next(tokenPrice / 50, tokenPrice / 25);
            }
            else
            {
                newPrice = r.Next(1, 8);
            }
			if (fiftypercent == 0 || fiftypercent == 1)
			{
				tokenPrice += newPrice;
				nPrice = newPrice;
				isIncrease = true;
				File.WriteAllText(@"database\token\token.txt", tokenPrice.ToString());
				Console.WriteLine("\ntoken price increased by " + newPrice + ". The price is now " + tokenPrice +"\n");

                var guild = ClientPublic.GetGuild(715825843816890368);

				foreach (var a in guild.Channels)
				{
					if (a.Id == 718690683753594932)
					{
                        EmbedBuilder e = new EmbedBuilder();
                        e.AddField("TOKEN INCREASED", "token price increased by " + newPrice + ". The price is now " + tokenPrice + " coins per token")
                            .WithAuthor("Token Price")
                            .WithFooter("Bot made by kevz#2073")
                            .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                            .WithCurrentTimestamp();
                        (a as ITextChannel).SendMessageAsync("",false,e.Build());
					}
				}
            }
			else
			{
                newPrice *= r.Next(2, 4);
                tokenPrice -= newPrice;

				isIncrease = false;
				File.WriteAllText(@"database\token\token.txt", tokenPrice.ToString());
				Console.WriteLine("\ntoken price decreased by " + newPrice + ". The price is now " + tokenPrice + "\n");

                var guild = ClientPublic.GetGuild(715825843816890368);

                foreach (var a in guild.Channels)
                {
                    if (a.Id == 718690683753594932)
                    {
                        if (tokenPrice <= 0)
                        {
                            EmbedBuilder e = new EmbedBuilder();
                            e.AddField("TOKEN INCREASED", "Token price is now 20 coins per token")
                                .WithAuthor("Token Price")
                                .WithFooter("Bot made by kevz#2073")
                                .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                                .WithCurrentTimestamp();
                            (a as ITextChannel).SendMessageAsync("", false, e.Build());

                            File.WriteAllText(@"database\token\token.txt", "20");
                        }
                        else
                        {
                            EmbedBuilder e = new EmbedBuilder();
                            e.AddField("TOKEN DECREASED", "token price decreased by " + newPrice + ". The price is now " + tokenPrice + " coins per token")
                                .WithAuthor("Token Price")
                                .WithFooter("Bot made by kevz#2073")
                                .WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
                                .WithCurrentTimestamp();
                            (a as ITextChannel).SendMessageAsync("", false, e.Build());
                        }
                    }
                }
            }
        }
    }
}