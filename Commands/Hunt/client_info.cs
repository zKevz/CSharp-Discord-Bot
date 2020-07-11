using System;

namespace CowboyBot
{
	public static class client_info
	{
		public static int yhealth { get; set; }
		public static int yattack { get; set; }
		public static void yincreaseHealth()
		{
			Random r = new Random();

			int remainder = yhealth / 100;
			int remainder2 = remainder / 2;

			yhealth += r.Next(yhealth / remainder, yhealth / remainder2);
		}
	}
}
