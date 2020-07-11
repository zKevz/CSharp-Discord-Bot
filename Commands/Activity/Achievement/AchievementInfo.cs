using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CowboyBot
{
	public class AchievementInfo
	{
		public static string GetMoneyExpert(string username)
		{
			JObject j = JObject.Parse(File.ReadAllText(
				@"database\achievement\" + username +
				".json"));
			string result = "";
			if (!(bool)j["moneyexpert"])
				result = "Money Expert => Not Achieved";
			else
				result = "Money Expert => ✅";
			return result;
		}

		public static string GetFishingExpert(string username)
		{
			JObject j = JObject.Parse(File.ReadAllText(
				@"database\achievement\" + username +
				".json"));
			string result = "";
			if (!(bool)j["fishingexpert"])
				result = "Fishing Expert => Not Achieved";
			else
				result = "Money Expert => ✅";
			return result;
		}

		public static string GetEquipmentSeeker(string username)
		{
			JObject j = JObject.Parse(File.ReadAllText(
				@"database\achievement\" + username +
				".json"));
			string result = "";
			if (!(bool)j["equipmentseeker"])
				result = "Equipment Seeker => Not Achieved";
			else
				result = "Equipment Seeker => ✅";
			return result;
		}
	}
}
