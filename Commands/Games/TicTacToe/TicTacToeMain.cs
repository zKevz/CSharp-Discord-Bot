using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using static CowboyBot.UserInfo;

namespace CowboyBot
{
	public class TicTacToeMain : InteractiveBase
	{
		private static int Round = 0;

		[Command("playtictactoe",RunMode = RunMode.Async)]
		[Alias("tictactoe","playttt","fightttt")]
		public async Task TicTacToe(IGuildUser user)
		{
			string[] MainBoard = new string[9];
			EnsureFromUserCriterion enemy = new EnsureFromUserCriterion(user);
			if (!isOnAccount(user.Id))
			{
				await ReplyAsync("That user doesn't even have a " +
					"cowboy account!");
				return;
			}
			else
			{
				await ReplyAsync("Sending request...");
				await SendEmbed(user, "REQUEST", "User " + user + " asked " +
					"you to play `Tic Tac Toe!` type \"accept\" if you want" +
					" to play!");
				try
				{
					string answer = (await NextMessageAsync(enemy, TimeSpan.FromSeconds(30))).ToString();
					if(answer.ToLower() == "accept" || answer.ToLower() == "yes")
					{
						int count = 0;
						MainBoard = CreateBoard();
						await SendBoard(Context.User as IGuildUser, user, MainBoard);
						Turn t = Turn.enemy;
						while (!IsFull(MainBoard) && !CheckWinner(MainBoard, "X") && !CheckWinner(MainBoard, "O"))
						{
							if (t == Turn.enemy)
							{
								await SendEmbed(Context.User as IGuildUser, "WAITING", "Waiting for opponent...");

								await EnemyTurn(user,MainBoard);
								t = Turn.you;

								await SendBoard(Context.User as IGuildUser, user, MainBoard);
							}
							else
							{
								await SendEmbed(user, "WAITING", "Waiting for opponent...");
								while (true)
								{
									EnsureFromUserCriterion e = new EnsureFromUserCriterion(Context.User);
									await SendEmbed(Context.User as IGuildUser, "YOUR TURN", "Where do you want to place an 'O' ? (1 - 9)\nYou ha" +
										"ve 30 seconds. If you don't answer, you'll automatically lose.");
									try
									{
										string answer1 = (await NextMessageAsync(e, TimeSpan.FromSeconds(30))).ToString();
										try
										{
											int a = Convert.ToInt32(answer1);
											if (a > 0 && a < 10)
											{
												if (MainBoard[a - 1] == " ")
												{
													MainBoard[a - 1] = "O";
													t = Turn.enemy;
													break;
												}
												else
												{
													if (count != 1)
													{
														await SendEmbed(user, "WHOOPS", "That grid is occupied! If you didn't" +
															" reply with the correct format once more you are automatically lose.");
														count++;
													}
													else
													{
														await SendEmbed(user, "RIP", "YOU LOSE Because of wrong format");
														await SendEmbed(Context.User as IGuildUser, "YAAY", "You win! your " +
															"opponent didn't type the correct format");
														return;
													}
												}
											}
											else
											{
												if (count != 1)
												{
													await SendEmbed(user, "WHOOPS", "Please type numbers from 1 to 10! If you didn't" +
														" reply with the correct format once more you are automatically lose.");
													count++;
												}
												else
												{
													await SendEmbed(user, "RIP", "YOU LOSE Because of wrong format");
													await SendEmbed(Context.User as IGuildUser, "YAAY", "You win! your " +
														"opponent didn't type the correct format");
													return;
												}
											}
										}
										catch
										{
											if (count != 1)
											{
												await SendEmbed(user, "WHOOPS", "Please type only numbers! If you didn't" +
													" reply with the correct format once more you are automatically lose.");
												count++;
											}
											else
											{
												await SendEmbed(user, "RIP", "YOU LOSE Because of wrong format");
												await SendEmbed(Context.User as IGuildUser, "YAAY", "You win! your " +
													"opponent didn't type the correct format");
												return;
											}
										}
									}
									catch
									{
										await SendEmbed(user, "RIP", "You didn't answer! You are automatically" +
											" lose.");
										await SendEmbed(Context.User as IGuildUser, "YEY", "You win! Your enemy didn't reply!");
										return;
									}
								}
								await SendBoard(Context.User as IGuildUser, user, MainBoard);
							}
						}

						if (IsFull(MainBoard))
						{
							await SendEmbed(user,"AWW", "TIE!");
							await SendEmbed(Context.User as IGuildUser,"AWW", "TIE");
							return;
						}
						else
						{
							if(CheckWinner(MainBoard, "X"))
							{
								await SendEmbed(user, "YAYY", "YOU WIN!!");
								await SendEmbed(Context.User as IGuildUser, "NOOB", "YOU LOSE!");
							}
							else
							{
								await SendEmbed(Context.User as IGuildUser, "YAYY", "YOU WIN!!");
								await SendEmbed(user, "NOOB", "YOU LOSE!");
							}
						}
					}
					else
					{
						await SendEmbed(user, "Cancelled", "You " +
							"declined the request.");
						return;
					}
				}
				catch
				{
					await SendEmbed(user, "WHOOPS", "You didn't answer! Game " +
						"cancelled..");
					return;
				}
			}
		}

		/*[Command("tictactoeai",RunMode = RunMode.Async)]
		[Alias("playtictactoeai","playai")]
		public async Task TicTacToeAI()
		{
			Round = 0;
			string[] MainBoard = CreateBoard();
			await SendBoardAI(MainBoard);
			Turn t = Turn.you;
			while (!IsFull(MainBoard) && !CheckWinner(MainBoard,"X") && !CheckWinner(MainBoard,"O"))
			{
				if (t == Turn.you)
				{
					while (true)
					{
						await SendEmbedAI("YOUR TURN", "Where do you want to place an 'X' ? (1 - 9)\nYou ha" +
							"ve 30 seconds..");
						try
						{
							string answer1 = (await NextMessageAsync(true,false, TimeSpan.FromSeconds(30))).ToString();
							try
							{
								int a = Convert.ToInt32(answer1);
								if (a > 0 && a < 10)
								{
									if (MainBoard[a - 1] == " ")
									{
										MainBoard[a - 1] = "X";
										t = Turn.enemy;
										break;
									}
									else
									{
										await SendEmbedAI("WHOOPS", "That grid is occupied! If you didn't" +
											" reply with the correct format once more you are automatically lose.");
									}
								}
								else
								{
									await SendEmbedAI("WHOOPS", "Please type numbers from 1 to 10! If you didn't" +
										" reply with the correct format once more you are automatically lose.");
								}
							}
							catch
							{
								await SendEmbedAI("WHOOPS", "Please type only numbers! If you didn't" +
									" reply with the correct format once more you are automatically lose.");								
							}
						}
						catch
						{
							await SendEmbedAI("RIP", "You didn't answer! You lose!");
							return;
						}
					}
					await SendBoardAI(MainBoard);
					Round++;
				}
				else
				{
					try
					{
						
					}
					catch (Exception e) { Console.WriteLine(e.Message); }
					t = Turn.you;
					await SendBoardAI(MainBoard);
					Round++;
				}
			}

			if (IsFull(MainBoard))
			{
				await SendEmbedAI("WHOOPS", "TIE!!");
				return;
			}
			else
			{
				if(CheckWinner(MainBoard, "X"))
				{
					await SendEmbedAI("WOWOWOOW", "YOU WIN AGAINTS AI!! CONGRATS!");
				}
				else
				{
					await SendEmbedAI("NOOB","YOU LOSE!");
				}
			}
		}*/

		public async Task EnemyTurn(IGuildUser user, string[] MainBoard)
		{
			int count = 0;
			while (true)
			{
				EnsureFromUserCriterion e = new EnsureFromUserCriterion(user);
				await SendEmbed(user, "YOUR TURN", "Where do you want to place an 'X' ? (1 - 9)\nYou ha" +
					"ve 30 seconds. If you don't answer, you'll automatically lose.");
				try
				{
					string answer1 = (await NextMessageAsync(e, TimeSpan.FromSeconds(30))).ToString();
					try
					{
						int a = Convert.ToInt32(answer1);
						if (a > 0 && a < 10)
						{
							if (MainBoard[a - 1] == " ")
							{
								MainBoard[a - 1] = "X";
								break;
							}
							else
							{
								if (count != 1)
								{
									await SendEmbed(user, "WHOOPS", "That grid is occupied! If you didn't" +
										" reply with the correct format once more you are automatically lose.");
									count++;
								}
								else
								{
									await SendEmbed(user, "RIP", "YOU LOSE Because of wrong format");
									await SendEmbed(Context.User as IGuildUser, "YAAY", "You win! your " +
										"opponent didn't type the correct format");
									return;
								}
							}
						}
						else
						{
							if (count != 1)
							{
								await SendEmbed(user, "WHOOPS", "Please type numbers from 1 to 10! If you didn't" +
									" reply with the correct format once more you are automatically lose.");
								count++;
							}
							else
							{
								await SendEmbed(user, "RIP", "YOU LOSE Because of wrong format");
								await SendEmbed(Context.User as IGuildUser, "YAAY", "You win! your " +
									"opponent didn't type the correct format");
								return;
							}
						}
					}
					catch//no lag = true;
					{
						if (count != 1)
						{
							await SendEmbed(user, "WHOOPS", "Please type only numbers! If you didn't" +
								" reply with the correct format once more you are automatically lose.");
							count++;
						}
						else
						{
							await SendEmbed(user, "RIP", "YOU LOSE Because of wrong format");
							await SendEmbed(Context.User as IGuildUser, "YAAY", "You win! your " +
								"opponent didn't type the correct format");
							return;
						}
					}
				}
				catch
				{
					await SendEmbed(user, "RIP", "You didn't answer! You are automatically" +
						" lose.");
					await SendEmbed(Context.User as IGuildUser, "YEY", "You win! Your enemy didn't reply!");
					return;
				}
			}
		}

		public static bool CheckWinner(string[] board, string s)
		{
			return (board[0] == s && board[0] == board[1] && board[1] == board[2]) ||
				   (board[3] == s && board[3] == board[4] && board[4] == board[5]) ||
				   (board[6] == s && board[6] == board[7] && board[7] == board[8]) ||
				   (board[0] == s && board[0] == board[3] && board[3] == board[6]) ||
				   (board[1] == s && board[1] == board[4] && board[4] == board[7]) ||
				   (board[2] == s && board[2] == board[5] && board[5] == board[8]) ||
				   (board[0] == s && board[0] == board[4] && board[4] == board[8]) ||
				   (board[2] == s && board[2] == board[4] && board[4] == board[6]);
		}

		public enum Turn
		{
			you,
			enemy
		}

		public static string[] CreateBoard()
		{
			string[] result = new string[9];
			for (int i = 0; i < 9; i++) result[i] = " ";
			return result;
		}

		public async Task SendBoard(IGuildUser user1, IGuildUser user2, string[] board)
		{
			string result = " --- --- ---\n" +
						   "| " + board[0] + " | " + board[1] + " | " + board[2] + " |\n " +
							"--- --- ---\n" +
						   "| " + board[3] + " | " + board[4] + " | " + board[5] + " |\n" +
						   " --- --- ---\n" +
						   "| " + board[6] + " | " + board[7] + " | " + board[8] + " |\n" +
						   " --- --- ---";
			await SendEmbed(user1, "BOARD", "```fix\n" + result + "```");
			await SendEmbed(user2, "BOARD", "```fix\n" + result + "```");
		}

		public async Task SendBoardAI(string[] board)
		{
			string result = " --- --- ---\n" +
						   "| " + board[0] + " | " + board[1] + " | " + board[2] + " |\n "+
							"--- --- ---\n" +
						   "| " + board[3] + " | " + board[4] + " | " + board[5] + " |\n" +
						   " --- --- ---\n" +
						   "| " + board[6] + " | " + board[7] + " | " + board[8] + " |\n" +
						   " --- --- ---";
			await SendEmbedAI("BOARD","```fix\n" + result + "```");
		}

		public static bool IsFull(string[] board)
		{
			int count = 0;
			foreach(string s in board)
			{
				if (s == " ") count++;
			}
			return count == 0;
		}

		public async Task Move(IGuildUser user,IGuildUser lol, string[] board, string shape)
		{
			int count = 0;
			while (true)
			{
				EnsureFromUserCriterion e = new EnsureFromUserCriterion(user);
				await SendEmbed(user, "YOUR TURN", "Where do you want to place an '" + shape + "' ? (1 - 9)\nYou ha" +
					"ve 30 seconds. If you don't answer, you'll automatically lose.");
				try
				{
					string answer = (await NextMessageAsync(e, TimeSpan.FromSeconds(30))).ToString();
					try
					{
						int a = Convert.ToInt32(answer);
					}
					catch
					{
						if (count != 1)
						{
							await SendEmbed(user, "WHOOPS", "Please type only numbers! If you didn't" +
								" reply with the correct format once more you are automatically lose.");
							count++;
						}
						else
						{
							return;
						}
					}
				}
				catch
				{
					await SendEmbed(user, "RIP", "You didn't answer! You are automatically" +
						" lose.");
					await SendEmbed(lol, "YEY", "You win! Your enemy didn't reply!");
					return;
				}
			}
		}

		public async Task SendEmbed(IGuildUser user, string title, string text)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, text)
				.WithFooter("Bot made by kevz#2073")
				.WithAuthor("Tic Tac Toe")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithCurrentTimestamp();
			await user.SendMessageAsync("", false, e.Build());
		}

		public async Task SendEmbedAI(string title, string text)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, text)
				.WithFooter("Bot made by kevz#2073")
				.WithAuthor("Tic Tac Toe")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithCurrentTimestamp();
			await Context.User.SendMessageAsync("", false, e.Build());
		}

		public static void SendEmbedAI2(string title, string text)
		{
			TicTacToeMain t = new TicTacToeMain();
			t.SendEEEEE(title,text);
		}

		public void SendEEEEE(string title,string text)
		{
			Random r = new Random();
			EmbedBuilder e = new EmbedBuilder();
			e.AddField(title, text)
				.WithFooter("Bot made by kevz#2073")
				.WithAuthor("Tic Tac Toe")
				.WithColor(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))
				.WithCurrentTimestamp();
			Context.User.SendMessageAsync("", false, e.Build());
		}
	}
}
