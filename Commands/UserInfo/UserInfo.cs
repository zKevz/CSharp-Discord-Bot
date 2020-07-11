using Discord.Commands;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
	public static class UserInfo
	{
		public static int coins(string user)
		{
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));
			return (int) j["coins"];

			/*if (!File.Exists(@"database\" + user + @"\coins.txt")){ File.WriteAllText(@"database\" + user + @"\coins.txt", "1000"); }
			return Convert.ToInt32(File.ReadAllText(@"database\" + user + @"\coins.txt"));*/
		}
		public static string getClothesName(string type, int id)
		{
			if (type == "pb")
			{
				if (id == 1)
				{return "T - Shirt";}
				else if (id == 2)
				{return "Leather Shirt";}
				else if (id == 3) 
				{return "Bulletproof Shirt";}
			}
			else if (type == "ph")
			{
				if(id == 1) { return "Fist"; }
				else if (id == 2) { return "Knife"; }
				else if (id == 3) { return "Shotgun"; }
			}
			else if (type == "pr")
			{
				if (id == 1) { return "Normal Rod"; }
				else if (id == 2) { return "Golden Rod"; }
				else if (id == 3) { return "Marvelous Rainbow Rod"; }
			}
			else{
				Console.WriteLine("error type of : " + type);
				return null;
			}
			return "";
		}
		public static int player_hand(string user)
		{
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));
			return (int) j["player_hand"];

			/*if (!File.Exists(@"database\" + user + @"\player_hand.txt")) 
			{
				File.WriteAllText(@"database\" + user + @"\player_hand.txt", "1"); 
			
			}
			return Convert.ToInt32(File.ReadAllText(@"database\" + user + @"\player_hand.txt"));*/
		}			
		public static int player_body(string user)
		{
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));
			return (int)j["player_body"];
			/*if (!File.Exists(@"database\" + user + @"\player_body.txt")) File.WriteAllText(@"database\" + user + @"\player_body.txt", "1");
			return Convert.ToInt32(File.ReadAllText(@"database\" + user + "/player_body.txt"));*/
		}

		public static int player_rod(string user)
		{
			JObject j = JObject.Parse(File.ReadAllText(@"database\account\" + user + ".json"));
			return (int)j["player_rod"];
		}

		public static bool isOnAccount(ulong userID)
		{
			return File.Exists(@"database\isonaccount\" + userID + ".txt");
		}

		public static string accName(ulong user)
		{
			try
			{
				return File.ReadAllText(@"database\isonaccount\" + user + ".txt");
			}
			catch { return null; }
		}
	}
}
