# Android-Multiplayer-Chess

CMPT 436 Final Project

Daniel Blanche & Michal Luczynski

The two main components of this project are the Android App and a main server. 

The chess game was developed in Xamarin Studio and was targeted for the Android platform. It allows you to connect to a server and play a game against another oponent. Upon your turn, you can drag and drop your game pieces and the client will determine if it is a legal move. Legal moves are then passed to the server which in turn communicates with the oponnent and passes on the "move" information. The client also has a chat capability for communicating with the other player. 

The server is written in C# and handles all the communication between clients. This includes "moves", game state and chat messages. It is threaded in order to support multiple games between multiple clients. All the communication is done using sockets.

To connect to (and create) a game, you first specify the location (IP) of the server you are running so the game knows who to connect to. Also you need to specify what you want your username to be, and who your opponent's username is. The server then looks for a game matching your opponent and username, if non is found it creates one.

If you created the game, you will be set as the white player, if you join a game you will be set as the black player.

![Main Fragment](https://github.com/dblanche54/Android-Multiplayer-Chess/blob/master/Screenshots/Screenshot_2015-12-03-16-15-15.png?raw=true) ![Login Fragment](https://github.com/dblanche54/Android-Multiplayer-Chess/blob/master/Screenshots/Screenshot_2015-12-05-19-01-27.png?raw=true)
