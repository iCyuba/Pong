# The backend server for [my pong](https://github.com/iCyuba/Pong)

This is the code for the backend server used by the [C# client](https://github.com/iCyuba/Pong/tree/main/dotnet) for multiplayer

## Usage

The server isn't hosted anywhere as of writing. Self host it (if you want), idc.

### API

Literally just ws://localhost:3000. Nothing else.

#### Events you can send

- `register` - Registers the current connection as a player. The server will respond with a `register` event.

  - **Example**: `{"type": "register", "name": "Player1"}`
  - **Response**: `{"type": "register", "name": "Player1"}`
  - The response will be sent to all players who aren't in a session.

- `unregister` - Unregisters the connection. The server doesn't respond but the connection stays open.

  - **Example**: `{"type": "unregister"}`
  - **Response (you)**: _None_
  - **Response (others)**: `{"type": "unregister", "player": "Player1"}`
  - All players (who aren't in a session) will receive the `unregister` event.

- `list` - Lists all players who aren't in any session. The server will respond with a `list` event.

  - **Example**: `{"type": "list"}`
  - **Response**: `{"type": "list", "players": ["Player1", "Player2"]}`
  - You must be registered to use this event.

- `invite` - Invite another player to a new session

  - **Example**: `{"type": "invite", "player": "Player2"}`
  - **Response 1**: `{"type": "invite", "by": "Player1", "to": "Player2"}`
  - **Response 2**: `{"type": "create", "player1": "Player1", "player2": "Player2"}`
  - The player you invite will receive the same event.
  - You must be registered and the other player must also be registered and not in a session.
  - For the send response look here: [create](#create---this-is-the-2nd-response-to-an-invite)

- `ready` - Tell the server that you're ready to start the game.

  - **Example**: `{"type": "ready"}`
  - **Response**: `{"type": "ready", "player": "Player1"}`
  - You must be registered and in a session to use this event.

#### Events you can receive

_**Please also see the responses above. I didn't include them twice**_

- `error` - You did something you weren't supposed to do.. Good job.

  - **Example**: `{"type": "error", "message": "You're not registered"}`
  - You'll receive this event if you try to do something you're not supposed to do. Read the "documentation" above to see what you can and can't do.

- ##### `create` - This is the 2nd response to an invite

  - **Example**: `{"type": "create", "player1": "Player1", "player2": "Player2"}`
  - This is sent once player2 invites player1 after player1 sent an invite to player2.

- `start` - The game started

  - **Example**: `{"type": "start", "velX": 1234, "velY": 1234, "timestamp": 123456789}`
  - This is sent 3 seconds after both players are ready.
  - It is also sent 3 seconds after a player scores a point.
  - `velX` and `velY` are the initial velocity of the ball. (Which are randomized)
  - `timestamp` is the timestamp when the game started. (Used for calculating the ball position)

- `update` - The ball position was updated

  - **Example**: `{"type": "update", "posX": 1234, "posY": 1234, "velX": 1234, "velY": 1234, "timestamp": 123456789}`
  - This is sent every time the ball bounces off a wall or a paddle.
  - `posX` and `posY` are the current position of the ball.
  - `velX` and `velY` are the current velocity of the ball.
  - `timestamp` is the timestamp when the ball position was updated. (Used for calculating the ball position)

- `score` - A player scored a point
  - **Example**: `{"type": "score", "you": 1234, "opponent": 1234}`
  - Sent whenever the ball goes out of bounds.
  - The message is sent to both players. (you is the player who received the message. opponent is the other player)

## How to run

You can choose to run the server inside a docker container or standalone with nodejs.

For both variants you’ll first need to clone the repo and cd into the server folder.

### Docker

Literally just run this command:

```sh
$ docker compose up
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

3. Build the server:

#### Production build

2. A) Production build:

```sh
$ npm run build
```

2. B) Development build

```sh
$ npm run dev:build
```

If you want to run the server in watch mode you can use this command instead:

```sh
$ npm run dev
```

3. Start it:

```sh
$ npm start
```
