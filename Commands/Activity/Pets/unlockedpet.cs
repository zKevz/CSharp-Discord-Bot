using Newtonsoft.Json.Linq;
using System.IO;

namespace CowboyBot
{
    public static class unlockedPets
    {
        public static string petList(int id, string username)
        {
            string result = "";

            switch(id)
            {
                case 1:
                    if(havePet(id,username))
                    {
                        result = "Your default pet : **Cat**\n";
                    }
                    break;
                case 2:
                    if(havePet(id,username))
                    {
                        result = "Second pet : **Dog**\n";
                    }
                    else
                    {
                        result = "LOCKED\n";
                    }
                    break;
                case 3:
                    if(havePet(id,username))
                    {
                        result = "Third Pet : **Puppy**\n";
                    }
                    else{
                        result = "LOCKED\n";
                    }
                    break;
                case 4:
                    if(havePet(id,username))
                    {
                        result = "Fourth Pet : **Monkey**\n";
                    }
                    else{
                        result = "LOCKED\n";
                    }
                    break;
                case 5:
                    if(havePet(id,username))
                    {
                        result = "Fifth Pet : **Megalodon**";
                    }
                    else
                    {
                        result = "LOCKED\n";
                    }
                    break;
            }

            return result;
        }

        public static bool havePet(int id,string username)
        {
            JObject j = JObject.Parse(File.ReadAllText(@"database\pets\" + username + ".json"));
            return (bool) j[id.ToString()];
        }

        public static string petPrice(int id,string username)
        {
            if(havePet(id,username))
            {
                return "Price : " + id * 800 + " Coins";
            }
            else
            {
                return "Get " + 1200 * (id- 1) + " Coins to unlock this pet.";
            }
        }
    }
}