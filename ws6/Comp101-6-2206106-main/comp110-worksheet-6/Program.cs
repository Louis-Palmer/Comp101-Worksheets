using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_6
{
	class Program
	{
		// Change these to test the stretch goal
		const int c_width = 3;
		const int c_height = 3;
		const int c_inARow = 3;

		// Read square coordinates from standard input in the form x,y. Return null on error.
		static Tuple<int, int> InputSquare()
		{
			// Get input from console
			string input = Console.ReadLine();

			// Split by comma
			var splitString = input.Split(',');

			// There should be exactly one comma -- if not then error
			if (splitString.Length != 2)
			{
				Console.WriteLine("Invalid input!");
				return null;
			}

			// Try parsing the two parts as ints -- error if either of them fails
			if (!int.TryParse(splitString[0], out int x) || !int.TryParse(splitString[1], out int y))
			{
				Console.WriteLine("Invalid input!");
				return null;
			}

			// Check that inputs are in range
			if (x < 0 || x >= c_width || y < 0 || y >= c_height)
			{
				Console.WriteLine("Invalid input!");
				return null;
			}

			// No errors, so return the coordinates
			return new Tuple<int, int>(x, y);
		}

		static void Main(string[] args)
		{
			bool gameIsOver = false;
			Mark currentPlayer = Mark.O;
			OxoBoard board = new OxoBoard(c_width, c_height, c_inARow);

			while (!gameIsOver)
			{
				// Print board
				Console.WriteLine();
				board.PrintBoard();
				Console.WriteLine("Player turn: {0}", currentPlayer);

				// Read input
				Tuple<int, int> square = null;
				while (square == null)
				{
					Console.WriteLine("Enter a square in the form x,y:");
					square = InputSquare();
				}
				
				// Try to play the move
				if (board.SetSquare(square.Item1, square.Item2, currentPlayer))
				{
					// Success, so switch players
					currentPlayer = (currentPlayer == Mark.O) ? Mark.X : Mark.O;

					// Check for a win
					var winner = board.GetWinner();
					if (winner != Mark.None)
					{
						board.PrintBoard();
						Console.WriteLine("Player {0} wins!", winner);
						gameIsOver = true;
					}

					// Check for board full
					if (board.IsBoardFull())
					{
						board.PrintBoard();
						Console.WriteLine("It's a draw!");
						gameIsOver = true;
					}
				}
				else
				{
					Console.WriteLine("Invalid move!");
				}
			}

			Console.WriteLine("Press enter to continue");
			Console.ReadLine();
		}
	}
}
