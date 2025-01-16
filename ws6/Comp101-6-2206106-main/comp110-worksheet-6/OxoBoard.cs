using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_6
{
	public enum Mark { None, O, X };

	public class OxoBoard

	   /// ALL CODE SHOULD WORK FOR SQUARE BOARD STRETCH GOAL///
	   /// ALL CODE SHOULD WORK FOR SQUARE BOARD STRETCH GOAL///

	{
		// Stores the width of the board (3 unless you are attempting the stretch goal)
		private int m_width;

		// Stores the height of the board (3 unless you are attempting the stretch goal)
		private int m_height;

		// Stores the number of cells you need in a row to win (3 unless you are attempting the stretch goal)
		private int m_inARow;

		private Mark[,] m_board;

		// Constructor. Perform any necessary data initialisation here.


		// The parameters are for attempting the stretch goal -- the unit tests keep them all as their default values.
		public OxoBoard(int width = 3, int height = 3, int inARow = 3)
		{
			m_width = width;
			m_height = height;
			m_inARow = inARow;

			m_board = new Mark[m_width, m_height];

			//fills the board with None.
			for (int i = 0; i < m_width; i++)
            {
				for(int j = 0; j < m_height; j++)
                {
					m_board[i, j] = Mark.None;
				}
			}
		



		}

		// Return the contents of the specified square.
		public Mark GetSquare(int x, int y)
		{
			return m_board[x, y]; // returns whatever is in x,y
		}

		// If the specified square is currently empty, fill it with mark and return true.
		// If the square is not empty, leave it as-is and return False.
		public bool SetSquare(int x, int y, Mark mark)
		{
			if (m_board[x, y] == Mark.None) //checks for spots filled with none
            {
				m_board[x, y] = mark; //fills them in with "mark"
				return true;
            }

			return false; //leaves the spot none as is

		}

		// If there are still empty squares on the board, return false.
		// If there are no empty squares, return true.
		public bool IsBoardFull()
		{
			int FullCount = 0; //variable for counting how many are filled in

			//checks for anything that isnt None and adds to the fullcount
			for (int x = 0; x < m_width; x++)
            {
				for( int y = 0; y < m_height; y++)
                {
					if (m_board[x, y] != Mark.None)
					{
						FullCount++;
					}
				}
            }

			//if fullcount reaches width*height meaning the full board meaning something in everyslot then it returns true
			if (FullCount >= m_width * m_height)
            {
				return true;
            }

			//if even one slot is missing returns false
            else
            {
				return false;
            }

		}

		// If a player has three in a row, return Mark.O or Mark.X depending on which player.
		// Otherwise, return Mark.None.
		public Mark GetWinner()
		{


			//variables for counting the rows
			int counter = 0;
			int OCounter = 0;
			int XCounter = 0;

			// Checks the horizontal for O
			for (int y = 0; y < m_height; y++)
			{
				counter = 0; //resets the counter for every row

				//checks in that row for O and adds to count if yes
				for (int x = 0; x < m_width; x++)
				{
					if (m_board[x, y] == Mark.O)
					{
						counter++;
					}
				}

				//returns mark.O if count adds up
				if (counter == m_inARow)
                {
					return Mark.O;
                }

			}
			//Checks the horizontal for X
			for (int y = 0; y < m_height; y++)
			{
				counter = 0; //resets after every collum

				for (int x = 0; x < m_width; x++)
				{
					if (m_board[x, y] == Mark.X)
					{
						counter++;
					}
				}

				if (counter == m_inARow)
				{
					return Mark.X;
				}

			}

			//Checks the vertical for O
			for (int x = 0; x < m_width; x++)
			{
				counter = 0; 

				for (int y = 0; y < m_height; y++)
				{
					if (m_board[x, y] == Mark.O)
					{
						counter++;
					}
				}

				if (counter == m_inARow)
				{
					return Mark.O;
				}

			}

			//Checks the vertical for X
			for (int x = 0; x < m_width; x++)
			{
				counter = 0;

				for (int y = 0; y < m_height; y++)
				{
					if (m_board[x, y] == Mark.X)
					{
						counter++;
					}
				}

				if (counter == m_inARow)
				{
					return Mark.X;
				}

			}

			//Checks for Diagonal1 (TL BR) for X and O
			for (int x = 0; x < m_width; x++)
            {
				//checks the position on the board and adds to counter if correct
				if (m_board[x, x] == Mark.O)
                {
					OCounter++;
				}
				if (m_board[x, x] == Mark.X)
                {
					XCounter++;
                }

				//checks for the counters count to see if someone has reached the Winning condition amount in a row and returns the winner if yes
				if (OCounter == m_inARow)
				{
					return Mark.O;
				}
				if (XCounter == m_inARow)
				{
					return Mark.X;
				}

			}

			//resets the counters
			XCounter = 0;
			OCounter = 0;

			//Checks for Diagonal2 (TR BL) for X and O
			int i = 0;
			for (int y = m_width -1; y >= 0; y -= 1, i++)//I goes up whilst Y goes down. creates positions for Diagonal2
			{
				if (m_board[i, y] == Mark.O)
				{
					OCounter++;
				}
				if (m_board[i, y] == Mark.X)
				{
					XCounter++;
				}
				if (OCounter == m_inARow)
				{
					return Mark.O;
				}
				if (XCounter == m_inARow)
				{
					return Mark.X;

				}
			}

			//if no one wins then its a draw so it returns none
			return Mark.None;




			//experimental code for checking Diagonal Line 2

			//for (int x = 0; x < m_width; x++)
			//{
			//	for (int y = 2; y >= 0; y-=1 , x++)
			//    {
			//		if (m_board[x, y] == Mark.O)
			//		{
			//			OCounter++;
			//		}
			//		if (m_board[x, y] == Mark.X)
			//		{
			//			XCounter++;
			//		}
			//		if (OCounter == m_inARow)
			//		{
			//			return Mark.O;
			//		}
			//		if (XCounter == m_inARow)
			//		{
			//			return Mark.X;
			//		}
			//	}
			//}


			//Checks Winning conditions for Everything in an if statement (manual input)
			//works perfectly apart from stretch goal

			////test Winning conditions for O
			//if ((m_board[0, 0] == Mark.O && m_board[0, 1] == Mark.O && m_board[0, 2] == Mark.O)||(m_board[1, 0] == Mark.O && m_board[1, 1] == Mark.O && m_board[1, 2] == Mark.O)||(m_board[2, 0] == Mark.O && m_board[2, 1] == Mark.O && m_board[2, 2] == Mark.O)||(m_board[0, 0] == Mark.O && m_board[1, 0] == Mark.O && m_board[2, 0] == Mark.O)||(m_board[0, 1] == Mark.O && m_board[1, 1] == Mark.O && m_board[2, 1] == Mark.O)||(m_board[0, 2] == Mark.O && m_board[1, 2] == Mark.O && m_board[2, 2] == Mark.O)||(m_board[0, 0] == Mark.O && m_board[1, 1] == Mark.O && m_board[2, 2] == Mark.O)||(m_board[2, 0] == Mark.O && m_board[1, 1] == Mark.O && m_board[0, 2] == Mark.O)) return Mark.O;
			//
			////test Winning conditions for X
			//if ((m_board[0, 0] == Mark.X && m_board[0, 1] == Mark.X && m_board[0, 2] == Mark.X)||(m_board[1, 0] == Mark.X && m_board[1, 1] == Mark.X && m_board[1, 2] == Mark.X)||(m_board[2, 0] == Mark.X && m_board[2, 1] == Mark.X && m_board[2, 2] == Mark.X)||(m_board[0, 0] == Mark.X && m_board[1, 0] == Mark.X && m_board[2, 0] == Mark.X)||(m_board[0, 1] == Mark.X && m_board[1, 1] == Mark.X && m_board[2, 1] == Mark.X)||(m_board[0, 2] == Mark.X && m_board[1, 2] == Mark.X && m_board[2, 2] == Mark.X)|| (m_board[0, 0] == Mark.X && m_board[1, 1] == Mark.X && m_board[2, 2] == Mark.X)|| (m_board[2, 0] == Mark.X && m_board[1, 1] == Mark.X && m_board[0, 2] == Mark.X)) { return Mark.X; }
			//
			//else return Mark.None;

		}

		// ---------------------------------------------------------------------------------------------
		// You do not need to edit anything below this line!
		// ---------------------------------------------------------------------------------------------

		// Display the current board state in the terminal.
		public void PrintBoard()
		{
			for (int y = 0; y < m_height; y++)
			{
				if (y > 0)
				{
					Console.Write("---");
					for (int x = 1; x < m_width; x++)
						Console.Write("+---");
					Console.WriteLine();
				}

				for (int x = 0; x < m_width; x++)
				{
					if (x > 0)
						Console.Write(" | ");
					else
						Console.Write(" ");

					switch (GetSquare(x, y))
					{
						case Mark.None:
							Console.Write(" "); break;
						case Mark.O:
							Console.Write("O"); break;
						case Mark.X:
							Console.Write("X"); break;
					}
				}

				Console.WriteLine();
			}
		}
	}
}

