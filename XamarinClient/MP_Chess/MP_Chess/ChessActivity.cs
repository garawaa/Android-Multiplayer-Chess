﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MP_Chess
{
	[Activity (Label = "ChessActivity")]			
	public class ChessActivity : Activity
	{

		// Data types to represent what state a board block could be in
		// chessmanColour repreents the colour of the piece, or in case of no piece; empty
		public enum chessmanColour { empty, white, black }

		// chesman represents the type of pieces, or if none; empty
		public enum chessman { empty, King, Queen, Rook, Bishop, Knight, Pawn }

		// What is in a square
		public struct gameSquare
		{
			public chessmanColour colour; // colour of the piece on the square 
			public chessman piece; // content of the game piece
		}

		// Genearte a standard board
		public static gameSquare[,] generateDefaultBoard()
		{
			return
				new gameSquare[8, 8]
			{ 
				// ROW 1
				{
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Rook },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Knight },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Bishop },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.King },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Queen },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Bishop },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Knight },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Rook }
				},
				// ROW 2
				{
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.black, piece = chessman.Pawn }
				},
				// ROW 3
				{
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
				},
				//ROW 4
				{
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
				},
				// ROW 5
				{
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
				},
				// ROW 6
				{
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty },
					new gameSquare() { colour = chessmanColour.empty, piece = chessman.empty }
				},
				// ROW 7
				{
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Pawn }
				},
				// ROW 8
				{
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Rook },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Knight },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Bishop },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Queen },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.King },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Bishop },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Knight },
					new gameSquare() { colour = chessmanColour.white, piece = chessman.Rook }
				}
			};
		}

		public string printBoard(gameSquare[,] square){
			string board = "";
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					switch (square[i, j].piece)
					{
					case chessman.Bishop:
						board = board + "B ";
						break;
					case chessman.King:
						board = board + "K ";
						break;
					case chessman.Knight:
						board = board + "N ";
						break;
					case chessman.Pawn:
						board = board + "P ";
						break;
					case chessman.Queen:
						board = board + "Q ";
						break;
					case chessman.Rook:
						board = board + "R ";
						break;
					default:
						board = board + "  ";
						break;
					}
				}
				board = board + "\n";
			}
			return board;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Chess);

			// Make a chess board
			gameSquare[,] chessBoard = generateDefaultBoard ();

			TextView board = FindViewById<TextView> (Resource.Id.ChessBoard);
			board.Text = printBoard (chessBoard);

			// Create your application here
			Button exitButton = FindViewById<Button>(Resource.Id.ExitButton);

			//Wire up the connnect button
			exitButton.Click += (object sender, EventArgs e) =>
			{
				Intent intent = new Intent(Intent.ActionMain);
				intent.AddCategory(Intent.CategoryHome);
				intent.SetFlags(ActivityFlags.NewTask);
				StartActivity(intent);
				Finish();
			};
		}
	}
}

