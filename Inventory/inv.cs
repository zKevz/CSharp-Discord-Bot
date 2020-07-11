using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using Discord.Addons.Interactive;
using Discord;
using Discord.WebSocket;
using static CowboyBot.UserInfo;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CowboyBot
{
    public class Inventory : InteractiveBase
    {
        public static void addChick(string username,int number)
        {
            string value = File.ReadAllText(@"database\inventory\" + username + ".json");
            dynamic j = JsonConvert.DeserializeObject(value);
            j["chicken"] += number;
            string output = JsonConvert.SerializeObject(value, Formatting.Indented);
            File.WriteAllText(@"database\inventory\" + username + ".json",output);
        }

        public static void addCow(string username, int number)
        {
            string value = File.ReadAllText(@"database\inventory\" + username + ".json");
            dynamic j = JsonConvert.DeserializeObject(value);
            j["cow"] += number;
            string output = JsonConvert.SerializeObject(value, Formatting.Indented);
            File.WriteAllText(@"database\inventory\" + username + ".json", output);
        }

        public static void addEgg(string username, int number)
        {
            string value = File.ReadAllText(@"database\inventory\" + username + ".json");
            dynamic j = JsonConvert.DeserializeObject(value);
            j["egg"] += number;
            string output = JsonConvert.SerializeObject(value, Formatting.Indented);
            File.WriteAllText(@"database\inventory\" + username + ".json", output);
        }

        public static void addMeat(string username, int number)
        {
            string value = File.ReadAllText(@"database\inventory\" + username + ".json");
            dynamic j = JsonConvert.DeserializeObject(value);
            j["meat"] += number;
            string output = JsonConvert.SerializeObject(value, Formatting.Indented);
            File.WriteAllText(@"database\inventory\" + username + ".json", output);
        }

        public static int cow(string username)
		{
            JObject j = JObject.Parse(File.ReadAllText(@"database\inventory\" + username + ".json"));
            return (int)j["cow"];
		}

        public static int egg(string username)
        {
            JObject j = JObject.Parse(File.ReadAllText(@"database\inventory\" + username + ".json"));
            return (int)j["egg"];
        }

        public static int chicken(string username)
        {
            JObject j = JObject.Parse(File.ReadAllText(@"database\inventory\" + username + ".json"));
            return (int)j["chicken"];
        }

        public static int meat(string username)
        {
            JObject j = JObject.Parse(File.ReadAllText(@"database\inventory\" + username + ".json"));
            return (int)j["meat"];
        }
    }
}