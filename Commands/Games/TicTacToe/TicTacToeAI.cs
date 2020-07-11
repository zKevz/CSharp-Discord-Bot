using Discord;
using Discord.Addons.Interactive;
using System.Collections.Generic;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class TicTacToeAI : InteractiveBase
	{
		[Command("playai",RunMode = RunMode.Async)]
		[Alias("playtictactoeai", "playtttai", "tictactoeai")]
		public async Task PlayAI()
		{
			try
			{
				char[,] board = CreateBoard();
				Turn t = Turn.you;
				await SendEmbed("BOARD","```fix\n" + PrintBoard(board) + "```");
				while (!isTie(board) && CheckWinnerV2(board,'X') != 10 && CheckWinnerV2(board,'O') != -10)
				{
					if (t == Turn.you)
					{
						Loop("ah");
						await SendEmbed("Hello " + Context.User.Username
							+ "!", "What do you want to choose? (1 - 9)");
						while (true)
						{
							try
							{
								string b = ((await NextMessageAsync(true, false,
									TimeSpan.FromSeconds(30))).Content);
								int a;
								if (int.TryParse(b, out a))
								{
									if (a > 0 && a < 10)
									{
										int spot1 = 0;
										int spot2 = 0;

										if (a >= 1 && a <= 3)
										{
											spot1 = 0;
											spot2 = a - 1;
										}
										else if (a >= 4 && a <= 6)
										{
											spot1 = 1;
											spot2 = a - 4;
										}
										else if (a >= 7 && a <= 9)
										{
											spot1 = 2;
											spot2 = a - 7;
										}

										if (board[spot1, spot2] == ' ')
										{
											board[spot1, spot2] = 'X';
											break;
										}
										else
										{
											await SendEmbed("WHOOPS", "That " +
												"position is occupied!");
										}
									}
									else
									{
										await SendEmbed("WHOOPS", "Only input " +
											"number from 1 to 9!");
									}
								}
								else
								{
									await SendEmbed("WHOOPS", "Please input number!");
								}
							}
							catch
							{
								await SendEmbed("WHOOPS", "You lose because you didn't answer.");
								return;
							}
						}

						if (checkWinnerAgain(board) == 10) break;

						t = Turn.enemy;
						await SendEmbed("BOARD","```fix\n" + PrintBoard(board) + "```");
					}
					else
					{
						board = AlgorithmV2(board);
						if (checkWinnerAgain(board) == -10) break;
						await SendEmbed("BOARD", "```fix\n" + PrintBoard(board) + "```");
						t = Turn.you;
					}
				}

				if (isTie(board))
				{
					await SendEmbed("WHOOPS", "TIE!!");
					return;
				}
				else
				{
					if (CheckWinnerV2(board,'X') == 10)
						await SendEmbed("YOU WIN AGAINST AI!", "WOWOOWOWOW");
					else
						await SendEmbed("LOL YOU LOSE!", "NOOOBBBBB");
					return;
				}
			}
			catch(Exception e) { Console.WriteLine(e.Message); }
		}

		//ALL THE RESOURCES
		public char you = 'X', computer = 'O';

		public int[] ReversedFormat(int a)
		{
			int[] i = new int[2];
			if(a >= 1 && a <= 3)
			{
				i[0] = 0;
				i[1] = a - 1;
			}
			else if (a >=4 && a <= 6)
			{
				i[0] = 1;
				i[1] = a - 4;
			}
			else
			{
				i[0] = 2;
				i[1] = 1 - 7;
			}
			return i;
		}

		public static void Loop<T>(T a)
		{
			for(int i = 0; i < 10; i++)
			{
				Console.WriteLine(a);
				for(int j = 10; j > 0; j--)
				{
					Console.WriteLine(a);
					for(int b = i + 1; b < j; b++)
					{
						Console.WriteLine(a);
					}
				}
			}
		}

		public enum Turn
		{
			enemy,
			you
		}

		public char[,] AlgorithmV2(char[,] lol)
		{
			char[,] board = CloneGrid(lol);
			int count = 0;

			bool firstStep = false;

			int ah = 0;
			foreach (char c in board) if (c == ' ') ah++;

			if (ah == 8 && (board[1, 1] != ' ' || board[0,0] != ' ' || board[0,2] != ' '
				|| board[2,0] != ' ' || board[2,2] != ' '))
			{
				if (board[1, 1] == ' ')
				{
					board[1, 1] = 'O';
				}
				else
				{
					int[,] randomint = { { 0, 0 }, { 0, 2 }, { 2, 0 }, { 2, 2 } };
					int x = new Random().Next(0, 4);
					board[randomint[x, 0], randomint[x, 1]] = 'O';
					SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(randomint[x, 0], randomint[x, 1]));
				}
			}
			else if (ah == 5 && (board[0, 0] != ' ' || board[0, 2] != ' ' || board[2, 0] != ' ' || board[2, 2] != ' '))
			{
				while (true)
				{
					int[,] randomint = { { 0, 0 }, { 0, 2 }, { 2, 0 }, { 2, 2 } };
					int x = new Random().Next(0, 4);
					if (board[randomint[x, 0], randomint[x, 1]] == ' ')
					{
						board[randomint[x, 0], randomint[x, 1]] = 'O';
						SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(randomint[x, 0], randomint[x, 1]));
						break;
					}
				}
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					if (firstStep) break;
					for (int j = 0; j < 3; j++)
					{
						if (board[i, j] == ' ')
						{
							board[i, j] = 'O';
							if (checkWinner(board) == -10)
							{
								board[i, j] = 'O';
								firstStep = true;
								SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(i,j));
								break;
							}
							else board[i, j] = ' ';
						}
					}
				}

				if (!firstStep)
				{
					bool secondStep = false;
					for (int i = 0; i < 3; i++)
					{
						if (secondStep) break;
						for (int j = 0; j < 3; j++)
						{
							if (board[i, j] == ' ')
							{
								board[i, j] = 'X';
								if (checkWinner(board) == 10)
								{
									board[i, j] = 'O';
									secondStep = true;
									SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(i, j));
									break;
								}
								else board[i, j] = ' ';
							}
						}
					}

					if (!secondStep)
					{
						bool thirdStep = false;
						for (int i = 0; i < 3; i++)
						{
							if (thirdStep) break;
							for (int j = 0; j < 3; j++)
							{
								if (thirdStep) break;
								if (board[i, j] == ' ')
								{
									board[i, j] = 'O';
									if (checkWinner(board) == -10)
									{
										board[i, j] = 'O';
										SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(i, j));
									}
									else
									{
										for (int k = 0; k < 3; k++)
										{
											if (thirdStep) break;
											for (int g = 0; g < 3; g++)
											{
												if (board[k, g] == ' ')
												{
													board[k, g] = 'O';
													if (checkWinner(board) == -10)
													{
														board[k, g] = 'O';
														SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(k,g));
														thirdStep = true;
														break;
													}
													else board[k, g] = ' ';
												}
											}
										}
									}

									board[i, j] = ' ';
								}
							}
						}

						if (!thirdStep)
						{
							bool fourthStep = false;
							bool shit = false;
							for (int i = 0; i < 3; i++)
							{
								if (fourthStep || shit) break;
								for (int j = 0; j < 3; j++)
								{
									if (fourthStep || shit) break;
									if (board[i, j] == ' ')
									{
										board[i, j] = 'X';
										for (int k = 0; k < 3; k++)
										{
											if (fourthStep || shit) break;
											for (int g = 0; g < 3; g++)
											{
												if (fourthStep || shit) break;
												if (board[k, g] == ' ')
												{
													board[k, g] = 'X';
													if (checkWinner(board) == 10)
													{
														if (count == 0)
														{
															board[k, g] = 'O';
															SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(k,g));
															fourthStep = true;
														}
														break;
													}
													else
													{
														if (shit || fourthStep) break;
														for (int q = 0; q < 3; q++)
														{
															if (shit || fourthStep) break;
															for (int w = 0; w < 3; w++)
															{
																if (shit || fourthStep) break;
																if (board[w, g] == ' ')
																{
																	board[w, g] = 'X';
																	if (checkWinner(board) == 10)
																	{
																		board[w, g] = 'O';
																		SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(w,g));
																		shit = true;
																		fourthStep = true;
																		break;

																	}
																	else board[w, g] = ' ';
																}

															}
														}
													}
													board[k, g] = ' ';
												}
											}
										}

										board[i, j] = ' ';
									}
								}
							}

							if (!fourthStep)
							{
								while (true)
								{
									Random r = new Random();
									int first = r.Next(0, 3);
									int second = r.Next(0, 3);
									if (board[first, second] == ' ')
									{
										board[first, second] = 'O';
										SendEmbed("Tic Tac Toe", "Enemy placed an 'O' in " + Format(first,second));
										break;
									}
								}
							}
						}
					}
				}
			}

			return board;
		}

		public async Task SendEmbed(string title,string description)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, description)
				.WithAuthor(Context.User.Username + "'s Tic Tac Toe")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithFooter("Bot made by kevz#2073")
				.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("",false,e.Build());
		}

		public static int Format(int a, int b)
		{
			int result;
			if (a == 0) result = b++;
			else if (a == 1) result = b + 4;
			else result = b + 7;
			return result;
		}

		public static string PrintBoard(char[,] board)
		{
			string result = "-----------\n ";
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (j == 2)
					{
						result += board[i, j] + "\n-----------\n ";
					}
					//else if (i == 2)
					//{
					//	result += board[i,j] + "\n-----------\n ";
					//}
					//else if (i == 8)
					//{
					//	result += board[i,j];
					//}
					else
						result += board[i, j] + " | ";
				}
			}
			result += "\n";
			return result;
		}

		public static char[,] CreateBoard()
		{
			char[,] c = new char[3, 3];
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					c[i, j] = ' ';
				}
			}
			return c;
		}

		public int maxNumber(int a, int b)
		{
			return a > b ? a : b;
		}

		public int minNumber(int a, int b) { return a < b ? a : b; }

		public static bool isTie(char[,] grid)
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (grid[i, j] == ' ') return false;
				}
			}
			return true;
		}

		public static int CheckWinnerV2(char[,] board, char s)
		{
			if((board[0,1] == s && board[0,0] == s && board[0,2] == s) ||
			   (board[1,0] == s && board[1,1] == s && board[1,2] == s) ||
			   (board[2,0] == s && board[2,1] == s && board[2,2] == s) ||
			   (board[1,0] == s && board[2,0] == s && board[0,0] == s) ||
			   (board[1,1] == s && board[0,1] == s && board[2,1] == s) ||
			   (board[0,2] == s && board[1,2] == s && board[2,2] == s) ||
			   (board[0,0] == s && board[1,1] == s && board[2,2] == s) ||
			   (board[0,2] == s && board[1,1] == s && board[2,0] == s))
			{
				if (s == 'X') return 10;
				else return -10;
			}
			return 0;
		}

		public int checkWinnerAgain(char[,] board)
		{
			if		(board[0, 0] == 'X' && board[1, 0] == 'X' && board[2, 0] == 'X')
				return 1;
			else if (board[0, 1] == 'X' && board[1, 1] == 'X' && board[2, 1] == 'X')
				return 1;
			else if (board[0, 2] == 'X' && board[1, 2] == 'X' && board[2, 2] == 'X')
				return 1;
			else if (board[0, 0] == 'X' && board[0, 1] == 'X' && board[0, 2] == 'X')
				return 1;
			else if (board[1, 0] == 'X' && board[1, 1] == 'X' && board[1, 2] == 'X')
				return 1;
			else if (board[2, 0] == 'X' && board[2, 1] == 'X' && board[2, 2] == 'X')
				return 1;
			else if (board[0, 0] == 'X' && board[1, 1] == 'X' && board[2, 2] == 'X')
				return 1;
			else if (board[0, 2] == 'X' && board[1, 1] == 'X' && board[2, 0] == 'X')
				return 1;


			else if (board[0, 0] == 'O' && board[1, 0] == 'O' && board[2, 0] == 'O')
				return 2;
			else if (board[0, 1] == 'O' && board[1, 1] == 'O' && board[2, 1] == 'O')
				return 2;
			else if (board[0, 2] == 'O' && board[1, 2] == 'O' && board[2, 2] == 'O')
				return 2;
			else if (board[0, 0] == 'O' && board[0, 1] == 'O' && board[0, 2] == 'O')
				return 2;
			else if (board[1, 0] == 'O' && board[1, 1] == 'O' && board[1, 2] == 'O')
				return 2;
			else if (board[2, 0] == 'O' && board[2, 1] == 'O' && board[2, 2] == 'O')
				return 2;
			else if (board[0, 0] == 'O' && board[1, 1] == 'O' && board[2, 2] == 'O')
				return 2;
			else if (board[0, 2] == 'O' && board[1, 1] == 'O' && board[2, 0] == 'O')
				return 2;
			return 0;
		}

		public int checkWinner(char[,] table)
		{
			for (int i = 0; i < 3; i++)
			{
				if (table[i, 0] == table[i, 1] &&
					table[i, 1] == table[i, 2])
				{
					if (table[i, 0] == you)
						return 10;
					else if (table[i, 0] == computer)
						return -10;
				}
			}

			for (int j = 0; j < 3; j++)
			{
				if (table[0, j] == table[1, j] &&
					table[1, j] == table[2, j])
				{
					if (table[0, j] == you)
						return 10;
					else if (table[0, j] == computer)
						return -10;
				}
			}

			if (table[0, 0] == table[1, 1] && table[1, 1] == table[2, 2])
			{
				if (table[0, 0] == you)
				{
					return 10;
				}
				else if (table[0, 0] == computer)
					return -10;
			}

			if (table[0, 2] == table[1, 1] && table[1, 1] == table[2, 0])
			{
				if (table[0, 2] == you)
				{
					return 10;
				}
				else if (table[0, 2] == computer)
					return -10;
			}

			return 0;
		}

		public static char[,] CloneGrid(char[,] grid)
		{
			char[,] s = new char[3, 3];
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					s[i, j] = grid[i, j];
				}
			}
			return s;
		}
	}
}
