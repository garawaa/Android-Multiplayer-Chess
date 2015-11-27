﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Program
    {

        // Data types to represent what state a board block could be in
        // chessmanColour repreents the colour of the piece, or in case of no piece; empty
        public enum chessmanColour { empty, white, black }

        // chesman represents the type of pieces, or if none; empty
        public enum chessman { empty, King, Queen, Rook, Bishop, Knight, Pawn }

        // Used to keep track of what the last move was, so we only need to send
            // this structure, rather than the entire gamestate each round
        public struct lastMove
        {
            public string lastPlayer; // the last player who took the the turn
            public int xOrigin; // the original x coordinate that was moved
            public int yOrigin; // the original y coordinate that was moved
            public int xMoved; // the x coordinate that the player moved to
            public int yMoved; // the y coordinate that the player moved to
        }

        // What is in a square
        public struct gameSquare
        {
            public chessmanColour colour; // colour of the piece on the square 
            public chessman piece; // content of the game piece
        }

        // Basic structure of all the gamedata for each game
        public struct gameObject
        {
            public gameSquare[,] boardGame; // The boardgame state
            public List<String> chatRoom; // List of string that make up the chatroom
            public lastMove lastPlayed; // Last move made by a player
            public string playerOne; // Username of player 1
            public string playerTwo; // Username of player 2
        }

        // List of all games that are going on
        public static List<gameObject> gamesInPlay;

        // List of usernames to make sure new connections have unique usernames
        public static List<String> userNames;
        
        // delegate init.
        public delegate void Del(TcpClient client);

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
        
        public static void handleNewConnection(TcpClient client)
        {
            try
            {
                // get streams
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());

                Console.Write("New Delegate created to handle new connection.\n");

                gameObject myGame = new gameObject();

                // Setup where the command data will be stored
                string[] command;

                // Read in from the socket here
                string recieve;

                // username of active connection
                string username = "";

                // Username of person they are playing against
                string usernameOpponent = "";

                // While logged in, recieve and process data
                bool loggedIn = true;

                // Does this user have an active game?
                bool isActive = false;

                // Last line of chatroom that has been sent to client
                int lastLine = 0;

                // Recieve input
                do {
                    // Read in from socket
                    recieve = reader.ReadLine();

                    command = recieve.Split(' ');

                    // Switch based on command recieved
                    switch (command[0])
                    {
                        // Login to the server
                        // Expected string "LOGIN username"
                        case "LOGIN":
                            if (command.Length == 2) {
                                username = command[1];
                                Console.WriteLine("Logging in " + username);
                                // make sure there are usernames to check
                                if (userNames.Count > 0)
                                {
                                    // search in use usernames for the desired username
                                    foreach (string user in userNames)
                                    {
                                        // if username already in list, it's already in use, thus log them out
                                        if (user == username)
                                        {
                                            loggedIn = false;
                                            writer.WriteLine("Username already in use, try with another");
                                            writer.Flush();
                                            break;
                                        }
                                    }
                                }
                                if (loggedIn)
                                {
                                    // if still logged in (i.e. not been logged out because username in use)
                                    // add username to the list of usernames in use
                                    userNames.Add(username);
                                }
                            }
                            else
                            {
                                // If command login command isn't what was expected let the player know
                                writer.WriteLine("Username not provided.");
                                writer.Flush();
                            }
                            break;
                        // Start New Game
                        // Expected string "NEW username_of_other_player"
                        case "NEW":
                            if (loggedIn)
                            {
                                myGame.boardGame = generateDefaultBoard();
                                myGame.playerOne = username;
                                usernameOpponent = command[1];
                                myGame.playerOne = usernameOpponent;
                                myGame.lastPlayed = new lastMove();
                                isActive = true;
                            }
                            break;
                        // Join a game (other player needs to be expecting user)
                        // Expected string "JOIN username_of_other_player"
                        case "JOIN":
                            if (loggedIn)
                            {
                                // set opponent username
                                usernameOpponent = command[1];
                                // search all games for a game that matches the state I'm expecting
                                foreach (gameObject game in gamesInPlay)
                                {
                                    // if game I'm looking for 
                                    if (game.playerOne == usernameOpponent && game.playerTwo == username)
                                    {
                                        // add game to my game
                                        myGame = game;
                                        isActive = true;
                                    }
                                }
                            }
                            break;
                        // Move a chess piece
                        // Expecting string "MOVE x1 y1 x2 y2"
                        case "MOVE":
                            if (isActive)
                            {
                                // Add moves to last played structure for other player to use
                                myGame.lastPlayed.xOrigin = Convert.ToInt32(command[1]);
                                myGame.lastPlayed.yOrigin = Convert.ToInt32(command[2]);
                                myGame.lastPlayed.xMoved = Convert.ToInt32(command[3]);
                                myGame.lastPlayed.yMoved = Convert.ToInt32(command[4]);
                                // flag that I'm the last player
                                myGame.lastPlayed.lastPlayer = username;
                                // update the game state
                                // move the piece
                                myGame.boardGame[myGame.lastPlayed.xMoved, myGame.lastPlayed.yMoved]
                                    = myGame.boardGame[myGame.lastPlayed.xOrigin, myGame.lastPlayed.yOrigin];
                                // clear old square
                                myGame.boardGame[myGame.lastPlayed.xOrigin, myGame.lastPlayed.yOrigin]
                                    = new gameSquare { colour = chessmanColour.empty, piece = chessman.empty };
                            }
                            break;
                        // Get new moves that have been made
                        case "GETMOVE":
                            if (isActive)
                            {
                                if (myGame.lastPlayed.lastPlayer != username)
                                {
                                    writer.WriteLine("MOVE " + myGame.lastPlayed.xOrigin.ToString() + " "
                                        + myGame.lastPlayed.yOrigin.ToString() + " "
                                        + myGame.lastPlayed.xMoved.ToString() + " "
                                        + myGame.lastPlayed.yMoved.ToString());
                                    writer.Flush();
                                } else
                                {
                                    writer.WriteLine("NOMOVE");
                                    writer.Flush();
                                }
                            }
                            break;
                        // logout from the server
                        case "LOGOUT":
                            if (loggedIn)
                            {
                                Console.WriteLine("Logging out " + username);

                                // logout
                                loggedIn = false;
                                // remove username from list of active users
                                userNames.Remove(username);
                            } else
                            {
                                loggedIn = false;
                            }
                            break;
                        // print gamestate to console. It's really more of a debug option
                        case "PRINT":
                            if (isActive)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        switch (myGame.boardGame[i, j].piece)
                                        {
                                            case chessman.Bishop:
                                                Console.Write("B ");
                                                break;
                                            case chessman.King:
                                                Console.Write("K ");
                                                break;
                                            case chessman.Knight:
                                                Console.Write("N ");
                                                break;
                                            case chessman.Pawn:
                                                Console.Write("P ");
                                                break;
                                            case chessman.Queen:
                                                Console.Write("Q ");
                                                break;
                                            case chessman.Rook:
                                                Console.Write("R ");
                                                break;
                                            default:
                                                Console.Write("  ");
                                                break;
                                        }
                                    }
                                    Console.WriteLine("");
                                }
                            }
                            break;
                        // Get new messages that are in the chatroom
                        case "GET":
                            if (loggedIn)
                            {
                                for (int i = lastLine; i < myGame.chatRoom.Count; i++)
                                {
                                    // send all unread messages
                                    writer.WriteLine(myGame.chatRoom[i]);
                                    lastLine++;
                                    writer.Flush();
                                }
                            }
                            break;
                        // default, any other string sent will be posted to the chatroom
                        default:
                            if (isActive)
                            {
                                string connectedString = "";
                                for (int i = 1; i < command.Length; i++)
                                {
                                    // catnate the command string to allow spaces to reform
                                    connectedString = connectedString + command[i];
                                }
                                // add line to chatroom
                                myGame.chatRoom.Add(username + ": " + connectedString);
                            }
                            break;

                    }
                } while (loggedIn);

            }
            catch (Exception E) { Console.WriteLine("Exception " + E); }

        }

        static void Main(string[] args)
        {
            Console.Write("Server Starting Up\n");

            // Initalize the lists
            userNames = new List<string>();
            gamesInPlay = new List<gameObject>();

            // create server, on port 8180
            TcpListener server = new TcpListener(8080);

            // start server
            server.Start();

            // Setup delegates
            List<Del> handleConnection = new List<Del>();
            List<IAsyncResult> results = new List<IAsyncResult>();

            // How many clients
            int connected = 0;

            //Allow connections
            while (true)
            {

                // Wait for client to connect
                TcpClient client = server.AcceptTcpClient();

                // Add blank field to list so we can use them for new connections
                results.Add(null);
                handleConnection.Add(null);

                Console.Write("Creating new Delegate to handle connection.\n");
                // get streams
                handleConnection[connected] = handleNewConnection;
                // Start new delegate
                results[connected] = handleConnection[connected].BeginInvoke(client, null, null);
                
                // count up connected connections
                connected++;

            }


        }
    }
}
