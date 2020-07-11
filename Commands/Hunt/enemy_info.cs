using System;

namespace CowboyBot
{
	public class enemy_info
	{
		public static int ehealth { get; set; }
		public static int eattack { get; set; }
		public static void eincreaseHealth()
		{
			Random r = new Random();
			ehealth += r.Next(ehealth / 3, ehealth / 2);
		}
	}
}
