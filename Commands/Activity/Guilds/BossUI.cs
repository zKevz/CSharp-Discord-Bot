using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CowboyBot
{
	public class BossUI
	{
		public static void EditHP(string name, int hp)
		{
			dynamic j = JsonConvert.DeserializeObject(
				File.ReadAllText(@"database\guilds\" + name + ".json"));
			j["bosshealth"] += hp;
			string output = JsonConvert.SerializeObject(j,
				Formatting.Indented);
			File.WriteAllText(@"database\guilds\" +
				name + ".json", output);
		}

		public static void EditLVL(string name, int lvl)
		{
			dynamic j = JsonConvert.DeserializeObject(
				File.ReadAllText(@"database\guilds\" + name + ".json"));
			j["bosslevel"] += lvl;
			string output = JsonConvert.SerializeObject(j,
				Formatting.Indented);
			File.WriteAllText(@"database\guilds\" +
				name + ".json", output);
		}

		public static int GetHP(string name)
		{
			JObject j = JObject.Parse(
				File.ReadAllText(@"database\guilds\" +
				name + ".json"));
			return (int)j["bosshealth"];
		}

		public static bool isBattle(string name)
		{
			JObject j = JObject.Parse(
				File.ReadAllText(@"database\guilds\" +
				name + ".json"));
			return (bool)j["isbattle"];
		}

		public enum Turn
		{
			boss = 0,
			you
		}

		public static int bHealth { get; set; }
		public static int bAttack { get; set; }

	}
}
