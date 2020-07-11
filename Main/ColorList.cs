using System;

namespace CowboyBot
{
	public class ColorList
	{
		public int[] color = 
		{ 255, 255, 178, 102, 102, 102, 178, 255, 255, 192, 255, 0,
		  51, 255, 255, 255, 178, 102, 102, 102, 102, 192, 255, 0,
		  51, 102, 102, 255, 255, 255, 255, 255, 178, 192, 255, 0
		};

		private static int[] colorr =
		{ 255, 255, 178, 102, 102, 102, 178, 255, 255, 192, 255, 0,
		  51, 255, 255, 255, 178, 102, 102, 102, 102, 192, 255, 0,
		  51, 102, 102, 255, 255, 255, 255, 255, 178, 192, 255, 0
		};
		private static Random rand = new Random();
		private static int index = getRandomNum();
		public int r = colorr[index];
		public int g = colorr[index + 12];
		public int b = colorr[index + 24];

		private static int getRandomNum()
		{
			return rand.Next(0, 12);
		}
	}
}