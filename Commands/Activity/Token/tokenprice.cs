using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using static CowboyBot.STP;
using System.Threading.Tasks;
using Discord.Addons.Interactive;

namespace CowboyBot
{
	public class tokenprices : InteractiveBase
	{
		public static int tokenPrice { get; set; }
		public static int nPrice { get; set; }
		public static bool isIncrease { get; set; } = false;
		public static int setToken()
		{
			if (File.Exists(@"database\token\token.txt")){
				tokenPrice = Convert.ToInt32(File.ReadAllText(@"database\token\token.txt"));
				return Convert.ToInt32(File.ReadAllText(@"database\token\token.txt"));
			}
			else
			{
				tokenPrice = 20;
				File.WriteAllText(@"database\token\token.txt","200");
				return 20;
			}
		}
	}
}
