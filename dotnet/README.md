# A pong game written in C#

The full project can be found on [GitHub](https://github.com/iCyuba/Pong)

## License

This project is licensed under the [MIT License](https://github.com/iCyuba/Pong/blob/main/LICENSE)

aka. do whatever you want with it, idc. but don't claim it as your own and credit me

## Usage

You gonna have to build it because I said so

## Controls

- Player 1: W/S (Up/Down also works if player2 is a bot or an online player)
- Player 2: Up/Down

## Features

- Play against a bot
  - Easy
  - Medium
  - Hard
  - (Impossible is technically in the code but it's only used by the main menu background)
- Play against a friend
  - Local
  - Online (WIP)

## Naming scheme

Apparently my class names aren't clear soo..

- Games

  - `Game` is the abstract base class for them
  - `GameServer` is used for local games. Kinda like how Minecraft has an integrated server
  - `GameClient` is used for online games.. as in it's a client that connects to a server
  - `GameFake` is used for the main menu screen. Key presses are ignored and the paddles move on their own

- Ball
  - `Ball` is the abstract base class for them
  - `BallServer` is used for local games. It has bounce and game logic. Used by `GameServer` and `GameFake`
  - `BallClient` is used for online games. It uses data from the server to move. Used by `GameClient`
