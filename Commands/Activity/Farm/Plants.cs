using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Discord.Addons.Interactive;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
	public class Plants
	{
		public static void createPlants(string user)
		{
			JObject j = new JObject(
				new JProperty("seed", new JProperty("wheat", 5))
				);
			File.WriteAllText(@"database\plants\" + user + ".json", j.ToString());
		}

		public static string getPlants(string user)
		{
			JObject j = JObject.Parse(File.ReadAllText(@"database\plants\" + user + ".json"));
			return (string) j["seed"];
		}
	}
}