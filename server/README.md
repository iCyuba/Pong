# The backend server for [my pong](https://github.com/iCyuba/Pong)

This is the code for the backend server used by the [C# client](https://github.com/iCyuba/Pong/tree/main/dotnet) for multiplayer

## Usage

A public server is available here: `wss://pong.icy.cx`

- Protected by Cloudflare <3

### API

Open a websocket to `/`. (eg. `ws://localhost:3000/`)

When you open a connection you won't receive anything until you send the `register` event. (see below)

#### Events you can send

- ##### `register` - Registers the current connection as a player

  - **Example**: `{"type": "register", "name": "Player1"}`
  - Params:
    - name: Any string that is not already taken and 16 or less characters long. (can't be empty tho)
  - **Response**: [register](#register---a-player-registered)
  - In my .NET client, I am sending a version tag as well. (eg. `{"type": "register", "name": "Player1", "version": "1.0.0"}`)
  - This is not required and the server doesn't handle it. But it could be used in the future to notify the client that it needs to update.

- ##### `unregister` - Unregisters the connection

  - **Example**: `{"type": "unregister"}`
  - **Response (you)**: _None_
  - **Response (others)**: [unregister](#unregister---a-player-unregistered)
  - All players (who aren't in a session) will receive the `unregister` event.

- ##### `list` - Lists all players who aren't in any session

  - **Example**: `{"type": "list"}`
  - **Response**: [list](#list---sends-a-list-of-all-players-who-arent-in-any-session)
  - You must be registered to use this event.

- ##### `invite` - Invite another player to a new session

  - **Example**: `{"type": "invite", "player": "Player2"}`
  - Params:
    - player: The name of the player you want to invite.
  - **Response 1**: [invite](#invite---a-player-or-you-invited-invited-another-player-or-you)
  - **Response 2**: [create](#create---this-is-the-2nd-response-to-an-invite)
  - The player you invite will receive the same event.
  - You must be registered and the other player must also be registered and not in a session.

- ##### `move` - Notify the server that you're moving

  - **Example**: `{"type": "move", "position": 50, "velocity": 80}`
  - Params:
    - position: The position of your paddle. (0-100)
    - velocity: The velocity of your paddle. usually -80, 0 or 80
  - **Response (you)**: _None_
  - **Response (other player)**: [move](#move---the-other-player-moved-their-paddle)

- ##### `ready` - Tell the server that you're ready to start the game.

  - **Example**: `{"type": "ready"}`
  - **Response 1**: [ready](#ready---a-player-is-ready-to-start)
  - **Response 2**: [start](#start---the-game-started)
  - You must be registered and in a session to use this event.

- ##### `end` - Close the session you're currently in

  - **Example**: `{"type": "end"}`
  - **Response**: [end](#end---a-game-session-was-closed-by-a-player)
  - Send this event to close the session you're currently in.

#### Events you can receive

- ##### `message` - A message from the server

  - **Example**: `{"type": "message", "message": "Hello World!"}`
  - Currently this ain't implemented yet. Could be useful for something in the future.

- ##### `error` - You did something you weren't supposed to do.. Good job.

  - **Example**: `{"type": "error", "message": "You're not registered"}`
  - You'll receive this event if you try to do something you're not supposed to do. Read the "documentation" above to see what you can and can't do.

- ##### `register` - A player registered

  - **Example**: `{"type": "register", "name": "Player1"}`
  - Note: this is also sent back to the player that just registered
  - This won't be sent to you if you're in a session

- ##### `unregister` - A player unregistered

  - **Example**: `{"type": "unregister", "player": "Player1"}`
  - This is sent to all players (who aren't in a session) when a player unregisters.
  - The player who unregisterd will not receive this event.

- ##### `list` - Sends a list of all players who aren't in any session

  - **Example**: `{"type": "list", "players": ["Player1", "Player2"]}`
  - You'll receive this event as a response to the `list` event.
  - It is also sent to you on registration and game end.

- ##### `invite` - A player (or you) invited invited another player (or you)

  - **Example**: `{"type": "invite", "by": "Player1", "to": "Player2"}`
  - Sent only to the 2 players involved.

- ##### `create` - This is the 2nd response to an invite

  - **Example**: `{"type": "create", "player1": "Player1", "player2": "Player2"}`
  - This is sent once player2 invites player1 after player1 sent an invite to player2.

- ##### `ready` - A player is ready to start

  - **Example**: `{"type": "ready", "player": "Player1"}`
  - Sent in response to the `ready` event.

- ##### `move` - The other player moved their paddle

  - **Example**: `{"type": "move", "position": 50, "velocity": 80}`
  - Sent when the other player moves their paddle..

- ##### `start` - The game started

  - **Example**: `{"type": "start", "angle": 180}`
  - This is sent once both players are ready.
  - It is also sent every time a player scores a point.
  - The angle is random and is in degrees.

- ##### `update` - The ball position was updated

  - **Example**: `{"type": "update", "posX": 1234, "posY": 1234, "angle": 180, "velocity": 55}`
  - This is sent every time the ball bounces off a wall or a paddle.
  - `posX` and `posY` are the current position of the ball.

- ##### `score` - A player scored a point

  - **Example**: `{"type": "score", "you": 1234, "opponent": 1234}`
  - Sent whenever the ball goes out of bounds.
  - The message is sent to both players. (you is the player who received the message. opponent is the other player)

- ##### `win` - A player won the game

  - **Example**: `{"type": "win", "player": "Player1"}`
  - Sent to both players when a player wins the game.
  - Note: a ready event must be sent by both players again to start.

- ##### `end` - A game session was closed by a player

  - **Example**: `{"type": "end", "player1": "Player1", "player2": "Player2"}`
  - Sent to all players who aren't in a session when a session is closed.
  - This can be caused by either one of the players in the session sending a "end" event or by the server when a player disconnects.

## How to run

You can choose to run the server inside a docker container or standalone with nodejs.

For both variants you’ll first need to clone the repo and cd into the server folder.

### Docker

Literally just run this command:

```sh
$ docker compose up --build -d
```

For development you can use this command instead which runs the server in watch mode with nodemon:

```sh
$ docker compose -f docker-compose.dev.yml up
```

### Standalone

Note: The dev scripts don't work on Windows. Use Docker or WSL. Or just run them manually without the environment variables prefixed.

1. Install required dependencies:

```sh
$ npm ci
```

2. Build the server:

> Production build:

```sh
$ npm run build
```

> Production build with ssl (you must have key.pem and cert.pem in /server/ssl/)

```sh
$ npm run build:ssl
```

> Development build:

```sh
$ npm run build:dev
```

> If you want to run the server in watch mode with nodemon you can use this command instead and skip step 3:

```sh
$ npm run dev
```

3. Start it:

```sh
$ npm start
```
