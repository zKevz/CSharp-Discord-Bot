using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CowboyBot
{
	public static class Deck
	{
		public static List<string> card = new List<string>();
		public static List<string> yourCard = new List<string>();
		public static List<string> enemyCard = new List<string>();

		public static string lastEnemyCard { get; set; }
		public static string lastYourCard { get; set; }

		public static int rarity(string num)
		{
			switch (num)
			{
				case "Ace":
					return 40;
				case "Two":
					return 2;
				case "Three":
					return 3;
				case "Four":
					return 4;
				case "Five":
					return 5;
				case "Six":
					return 6;
				case "Seven":
					return 7;
				case "Eight":
					return 8;
				case "Nine":
					return 9;
				case "Ten":
					return 10;
				case "Jack":
					return 11;
				case "Queen":
					return 12;
				case "King":
					return 13;
			}

			return 0;
		}

		public enum Suit
		{
			clubs = 0,
			spades = 1,
			hearts = 2,
			diamonds = 3
		}

		public enum Number
		{
			Ace = 1,
			Two = 2,
			Three = 3,
			Four = 4,
			Five = 5,
			Six = 6,
			Seven = 7,
			Eight = 8,
			Nine = 9,
			Ten = 10,
			Jack = 11,
			Queen = 12,
			King = 13
		}

		public enum turn
		{
			you, enemy
		}

		public static turn randomTurn()
		{
			Random r = new Random();
			int a = r.Next(0, 2);
			return (turn)a;
		}

		public static void createDeck()
		{
			for (int i = 1; i <= 4; i++)
			{
				switch (i)
				{
					case 1:
						for (int a = 1; a <= 13; a++)
						{
							if (a >= 2 && a <= 10)
							{
								card.Add((Number)a + " Of " + (Suit)0);
							}
							switch (a)
							{
								case 1:
									card.Add((Number)1 + " Of " + (Suit)0);
									break;
								case 11:
									card.Add((Number)11 + " Of " + (Suit)0);
									break;
								case 12:
									card.Add((Number)12 + " Of " + (Suit)0);
									break;
								case 13:
									card.Add((Number)13 + " Of " + (Suit)0);
									break;
							}
						}
						break;
					case 2:
						for (int a = 1; a <= 13; a++)
						{
							if (a >= 2 && a <= 10)
							{
								card.Add((Number)a + " Of " + (Suit)1);
							}
							switch (a)
							{
								case 1:
									card.Add((Number)1 + " Of " + (Suit)1);
									break;
								case 11:
									card.Add((Number)11 + " Of " + (Suit)1);
									break;
								case 12:
									card.Add((Number)12 + " Of " + (Suit)1);
									break;
								case 13:
									card.Add((Number)13 + " Of " + (Suit)1);
									break;
							}
						}
						break;
					case 3:
						for (int a = 1; a <= 13; a++)
						{
							if (a >= 2 && a <= 10)
							{
								card.Add((Number)a + " Of " + (Suit)2);
							}
							switch (a)
							{
								case 1:
									card.Add((Number)1 + " Of " + (Suit)2);
									break;
								case 11:
									card.Add((Number)11 + " Of " + (Suit)2);
									break;
								case 12:
									card.Add((Number)12 + " Of " + (Suit)2);
									break;
								case 13:
									card.Add((Number)13 + " Of " + (Suit)2);
									break;
							}
						}
						break;
					case 4:
						for (int a = 1; a <= 13; a++)
						{
							if (a >= 2 && a <= 10)
							{
								card.Add((Number)a + " Of " + (Suit)3);
							}
							switch (a)
							{
								case 1:
									card.Add((Number)1 + " Of " + (Suit)3);
									break;
								case 11:
									card.Add((Number)11 + " Of " + (Suit)3);
									break;
								case 12:
									card.Add((Number)12 + " Of " + (Suit)3);
									break;
								case 13:
									card.Add((Number)13 + " Of " + (Suit)3);
									break;
							}
						}
						break;
				}
			}
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}