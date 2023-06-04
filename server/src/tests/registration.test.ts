// Start a new server instance. This is the entry point for the server.
import { WebSocket } from "ws";

// The test helper functions
import createConnection, { closeConnection } from "@/tests/createConnection";
import waitForResponse, { waitForResponses } from "@/tests/waitForResponse";

import { ErrorMessage } from "@/messages";
import { ListMessage } from "@/messages/list";
import { RegisterMessage } from "@/messages/register";
import { UnregisterMessage } from "@/messages/unregister";
import Server from "@/server";

// Tests regarding registration / unregistration

describe("Registrations / Unregistrations", () => {
  let server: Server;
  // Create a new server instance before all tests with a random port
  beforeAll(done => {
    server = new Server({ port: 0 }, done);
  });

  // Once all tests are done, close the server instance
  afterAll(done => server.close(done));

  describe("With 1 player", () => {
    let ws: WebSocket;

    // Create a new connection to the server before each test
    beforeEach(async () => {
      ws = await createConnection(server);
    });

    // Close the connection after each test (if it's open)
    afterEach(() => closeConnection(ws));

    // Register with an invalid name (undefined, empty, too long, or a not a string)
    test.each([{}, { name: "" }, { name: "a".repeat(17) }, { name: 123 }])(
      "Register with an invalid name: %p",
      async data => {
        ws.send(JSON.stringify({ type: "register", ...data }));

        const response = await waitForResponse<ErrorMessage>(ws, false);

        // Expect the response to be an error message
        expect(response).toEqual({ type: "error", message: `Invalid name: ${data.name}` });

        // Expect the connection to be still open
        expect(ws.readyState).toBe(WebSocket.OPEN);
      }
    );

    test("Register and disconnect", async () => {
      ws.send(JSON.stringify({ type: "register", name: "test" }));

      const responses = await waitForResponses<[RegisterMessage, ListMessage]>(ws, 2, false);

      // Expect the fisrt response to be a registration message
      expect(responses[0]).toEqual({ type: "register", name: "test" });

      // Expect the second response to be a list message with no players (because we are the only one)
      expect(responses[1]).toEqual({ type: "list", players: [] });

      // Expect that the player is inside game.players and that the player is the only one
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("test");

      // Close the connection
      ws.close();

      // Wait like 50ms for the server to handle the close event (this should be enough. this is needed because it's over the network)
      await new Promise(resolve => setTimeout(resolve, 50));

      // Expect that the player is no longer inside game.players
      expect(server.game.players.length).toBe(0);
    });

    test("Register and try to register again", async () => {
      ws.send(JSON.stringify({ type: "register", name: "testing" }));

      // Wait for the 2 responses (registration and list). Tested above
      await waitForResponses(ws, 2, false);

      // Try to register again
      ws.send(JSON.stringify({ type: "register", name: "not_testing" }));

      const response = await waitForResponse<ErrorMessage>(ws, false);

      // Expect the response to be an error message
      expect(response).toEqual({ type: "error", message: "Already registered" });

      // Expect that the player is still inside game.players and that the player is the only one
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("testing");

      // Expect the socket to be still open
      expect(ws.readyState).toBe(WebSocket.OPEN);
    });

    test("Register, unregister and register again", async () => {
      ws.send(JSON.stringify({ type: "register", name: "some_player" }));
      await waitForResponses(ws, 2, false); // Wait for the registration and list messages

      // Unregister the player
      ws.send(JSON.stringify({ type: "unregister" }));

      // Wait like 50ms for the server to handle the unregister event (again, this should hopefully be enough)
      await new Promise(resolve => setTimeout(resolve, 50));

      // Expect that the player is no longer inside game.players
      expect(server.game.players.length).toBe(0);

      // Expect the socket to be still open
      expect(ws.readyState).toBe(WebSocket.OPEN);

      // Register again
      ws.send(JSON.stringify({ type: "register", name: "some_player" }));
      await waitForResponses(ws, 2, false);

      // Expect that the player is inside game.players and that the player is the only one
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("some_player");
    });
  });

  describe("With 2 players", () => {
    let websockets: [WebSocket, WebSocket];

    // Create 2 new connections to the server before each test
    beforeEach(async () => {
      // Create 2 websockets
      websockets = [await createConnection(server), await createConnection(server)];
    });

    // Close each connection after each test (if they're open)
    afterEach(() => websockets.forEach(ws => ws.readyState === WebSocket.OPEN && ws.close()));

    test("Register with the same name", async () => {
      websockets[0].send(JSON.stringify({ type: "register", name: "test" }));
      await waitForResponses(websockets[0], 2, false);

      websockets[1].send(JSON.stringify({ type: "register", name: "test" }));
      const response = await waitForResponse<ErrorMessage>(websockets[1], false);

      // Expect the response to be an error message
      expect(response).toEqual({ type: "error", message: "Username already taken" });

      // Expect that the player is inside game.players and that the player is the only one
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("test");

      // Expect the sockets to be still open
      expect(websockets[0].readyState).toBe(WebSocket.OPEN);
      expect(websockets[1].readyState).toBe(WebSocket.OPEN);
    });

    test("Register normally, list and unregister", async () => {
      websockets[0].send(JSON.stringify({ type: "register", name: "test1" }));
      await waitForResponse(websockets[0], false);

      websockets[1].send(JSON.stringify({ type: "register", name: "test2" }));

      // Await messages from both sockets
      // I'm using promise.all so that both promises are awaited at the same time
      await Promise.all([
        // The first socket should receive a registration message about the second socket
        waitForResponse<RegisterMessage>(websockets[0], false).then(response => {
          // Expect the response to be a registration message (but of the second socket)
          expect(response).toEqual({ type: "register", name: "test2" });
        }),

        waitForResponses<[RegisterMessage, ListMessage]>(websockets[1], 2, false).then(
          responses => {
            // Expect the first response to be a registration message (but of the second socket)
            expect(responses[0]).toEqual({ type: "register", name: "test2" });

            // Expect the second response to be a list message with the first player
            expect(responses[1]).toEqual({ type: "list", players: ["test1"] });
          }
        ),
      ]);

      // Request the list of players from the first socket
      websockets[0].send(JSON.stringify({ type: "list" }));

      // Wait for the list message
      const response1 = await waitForResponse<ListMessage>(websockets[0], false);

      // Expect the response to be a list message with the second player
      expect(response1).toEqual({ type: "list", players: ["test2"] });

      // Unregister the first player
      websockets[0].send(JSON.stringify({ type: "unregister" }));

      // Wait for the unregister message of the first socket on the second socket
      const response2 = await waitForResponse<UnregisterMessage>(websockets[1], false);

      // Expect the response to be an unregister message
      expect(response2).toEqual({ type: "unregister", name: "test1" });

      // Expect that the player is no longer inside game.players
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("test2");

      // Request the list of players from the second socket
      websockets[1].send(JSON.stringify({ type: "list" }));

      // Wait for the list message
      const response3 = await waitForResponse<ListMessage>(websockets[1], false);

      // Expect the response to be a list message with no players
      expect(response3).toEqual({ type: "list", players: [] });
    });
  });
});
