using comp110_worksheet_6;
using NUnit.Framework;
using System;
using System.Text;

namespace comp110_worksheet_6_tests
{
	[TestFixture]
	public class UnitTest1
	{
		Mark MarkFromChar(char c)
		{
			switch (c)
			{
				case 'O': return Mark.O;
				case 'X': return Mark.X;
				default: return Mark.None;
			}
		}

		char CharFromMark(Mark m)
		{
			switch (m)
			{
				case Mark.O: return 'O';
				case Mark.X: return 'X';
				default: return '.';
			}
		}

		void CompareBoard(OxoBoard board, string expected)
		{
			for (int y = 0; y < 3; y++)
				for (int x = 0; x < 3; x++)
					Assert.AreEqual(MarkFromChar(expected[y * 3 + x]), board.GetSquare(x, y));
		}

		[Test]
		public void TestEmptyBoard()
		{
			var board = new OxoBoard();
			CompareBoard(board, ".........");
		}

		[Test]
		public void TestEmptyBoardNoWinner()
		{
			var board = new OxoBoard();
			Assert.AreEqual(Mark.None, board.GetWinner());
		}

		[Test]
		public void TestFirstMove(
			[Values(0, 1, 2)] int x,
			[Values(0, 1, 2)] int y,
			[Values(Mark.O, Mark.X)] Mark player
		)
		{
			var board = new OxoBoard();
			Assert.IsTrue(board.SetSquare(x, y, player));
			Assert.AreEqual(Mark.None, board.GetWinner());
			StringBuilder expected = new StringBuilder(".........");
			expected[y * 3 + x] = CharFromMark(player);
			CompareBoard(board, expected.ToString());
		}

		[Test]
		public void TestSecondMove(
			[Values(0, 1, 2)] int x1,
			[Values(0, 1, 2)] int y1,
			[Values(Mark.O, Mark.X)] Mark p1,
			[Values(0, 1, 2)] int x2,
			[Values(0, 1, 2)] int y2
		)
		{
			var board = new OxoBoard();
			Assert.IsTrue(board.SetSquare(x1, y1, p1));
			Assert.AreEqual(Mark.None, board.GetWinner());

			Mark p2 = (p1 == Mark.O) ? Mark.X : Mark.O;

			if (x1 == x2 && y1 == y2)
			{
				Assert.IsFalse(board.SetSquare(x2, y2, p2));
				Assert.AreEqual(Mark.None, board.GetWinner());
				StringBuilder expected = new StringBuilder(".........");
				expected[y1 * 3 + x1] = CharFromMark(p1);
				CompareBoard(board, expected.ToString());
			}
			else
			{
				Assert.IsTrue(board.SetSquare(x2, y2, p2));
				Assert.AreEqual(Mark.None, board.GetWinner());
				StringBuilder expected = new StringBuilder(".........");
				expected[y1 * 3 + x1] = CharFromMark(p1);
				expected[y2 * 3 + x2] = CharFromMark(p2);
				CompareBoard(board, expected.ToString());
			}
		}

		[Test]
		public void TestVerticalLine(
			[Values(0, 1, 2)] int x,
			[Values(Mark.O, Mark.X)] Mark p1
		)
		{
			int x2 = (x + 1) % 3;
			Mark p2 = (p1 == Mark.O) ? Mark.X : Mark.O;

			var board = new OxoBoard();

			for (int y = 0; y < 2; y++)
			{
				Assert.IsTrue(board.SetSquare(x, y, p1));
				Assert.AreEqual(Mark.None, board.GetWinner());

				Assert.IsTrue(board.SetSquare(x2, y, p2));
				Assert.AreEqual(Mark.None, board.GetWinner());
			}

			Assert.IsTrue(board.SetSquare(x, 2, p1));
			Assert.AreEqual(p1, board.GetWinner());
		}

		[Test]
		public void TestHorizontalLine(
			[Values(0, 1, 2)] int y,
			[Values(Mark.O, Mark.X)] Mark p1
		)
		{
			int y2 = (y + 1) % 3;
			Mark p2 = (p1 == Mark.O) ? Mark.X : Mark.O;

			var board = new OxoBoard();

			for (int x = 0; x < 2; x++)
			{
				Assert.IsTrue(board.SetSquare(x, y, p1));
				Assert.AreEqual(Mark.None, board.GetWinner());

				Assert.IsTrue(board.SetSquare(x, y2, p2));
				Assert.AreEqual(Mark.None, board.GetWinner());
			}

			Assert.IsTrue(board.SetSquare(2, y, p1));
			Assert.AreEqual(p1, board.GetWinner());
		}

		[Test]
		public void TestDiagonalLine1(
			[Values(Mark.O, Mark.X)] Mark p1
		)
		{
			Mark p2 = (p1 == Mark.O) ? Mark.X : Mark.O;

			var board = new OxoBoard();

			for (int x = 0; x < 2; x++)
			{
				Assert.IsTrue(board.SetSquare(x, x, p1));
				Assert.AreEqual(Mark.None, board.GetWinner());

				Assert.IsTrue(board.SetSquare(x, (x + 1) % 3, p2));
				Assert.AreEqual(Mark.None, board.GetWinner());
			}

			Assert.IsTrue(board.SetSquare(2, 2, p1));
			Assert.AreEqual(p1, board.GetWinner());
		}

		[Test]
		public void TestDiagonalLine2(
			[Values(Mark.O, Mark.X)] Mark p1
		)
		{
			Mark p2 = (p1 == Mark.O) ? Mark.X : Mark.O;

			var board = new OxoBoard();

			for (int x = 0; x < 2; x++)
			{
				Assert.IsTrue(board.SetSquare(2-x, x, p1));
				Assert.AreEqual(Mark.None, board.GetWinner());

				Assert.IsTrue(board.SetSquare(2-x, (x + 1) % 3, p2));
				Assert.AreEqual(Mark.None, board.GetWinner());
			}

			Assert.IsTrue(board.SetSquare(0, 2, p1));
			Assert.AreEqual(p1, board.GetWinner());
		}

		[Test]
		public void TestFillBoard()
		{
			var board = new OxoBoard();
			Mark p = Mark.O;

			var moves = new int[,] { {1, 1}, {0, 0}, {1, 2}, {1, 0}, {2, 0}, {0, 2}, {0, 1}, {2, 1} };
			for (int i=0; i<moves.GetLength(0); i++)
			{
				int x = moves[i, 0];
				int y = moves[i, 1];

				Assert.IsTrue(board.SetSquare(x, y, p));
				Assert.AreEqual(Mark.None, board.GetWinner());
				Assert.IsFalse(board.IsBoardFull());

				p = (p == Mark.O) ? Mark.X : Mark.O;
			}

			Assert.IsTrue(board.SetSquare(2, 2, p));
			Assert.AreEqual(Mark.None, board.GetWinner());
			Assert.IsTrue(board.IsBoardFull());
		}
	}
}
