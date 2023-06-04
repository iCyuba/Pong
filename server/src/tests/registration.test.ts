// Start a new server instance. This is the entry point for the server.
import { WebSocket } from "ws";

// The test helper functions
import createConnection, { closeConnection } from "@/tests/createConnection";
import waitForResponse, { waitForResponses } from "@/tests/waitForResponse";

import { ErrorMessage } from "@/messages";
import { ListMessage } from "@/messages/list";
import { RegisterMessage } from "@/messages/register";
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
        expect(response.type).toBe("error");
        expect(response.message).toBe(`Invalid name: ${data.name}`);

        // Expect the connection to be still open
        expect(ws.readyState).toBe(WebSocket.OPEN);
      }
    );

    test("Register and disconnect", async () => {
      ws.send(JSON.stringify({ type: "register", name: "test" }));

      const responses = await waitForResponses<[RegisterMessage, ListMessage]>(ws, 2, false);

      // Expect the fisrt response to be a registration message
      expect(responses[0].type).toBe("register");
      expect(responses[0].name).toBe("test");

      // Expect the second response to be a list message with no players (because we are the only one)
      expect(responses[1].type).toBe("list");
      expect(responses[1].players).toEqual([]);

      // Expect that the player is inside game.players and that the player is the only one
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("test");

      // Close the connection
      ws.close();

      // Wait like 50ms for the server to handle the close event
      await new Promise(resolve => setTimeout(resolve, 50));

      // Expect that the player is no longer inside game.players
      expect(server.game.players.length).toBe(0);
    });

    test("Register and try to register again", async () => {
      ws.send(JSON.stringify({ type: "register", name: "testing" }));

      // Wait for the registration message. I'm not gonna check it again, the previous test already does that
      await waitForResponse(ws, false);

      // Try to register again
      ws.send(JSON.stringify({ type: "register", name: "not_testing" }));

      const response = await waitForResponse<ErrorMessage>(ws, false);

      // Expect the response to be an error message
      expect(response.type).toBe("error");
      expect(response.message).toBe("Already registered");

      // Expect that the player is still inside game.players and that the player is the only one
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("testing");

      // Expect the socket to be still open
      expect(ws.readyState).toBe(WebSocket.OPEN);
    });

    test("Register, unregister and re-register", async () => {
      ws.send(JSON.stringify({ type: "register", name: "some_player" }));
      await waitForResponse(ws, false);

      // Unregister the player
      ws.send(JSON.stringify({ type: "unregister" }));

      // Wait like 50ms for the server to handle the unregister event
      await new Promise(resolve => setTimeout(resolve, 50));

      // Expect that the player is no longer inside game.players
      expect(server.game.players.length).toBe(0);

      // Expect the socket to be still open
      expect(ws.readyState).toBe(WebSocket.OPEN);

      // Register again
      ws.send(JSON.stringify({ type: "register", name: "some_player" }));

      // Wait for the registration message
      await waitForResponse(ws, false);

      // Expect that the player is inside game.players and that the player is the only one
      expect(server.game.players.length).toBe(1);
      expect(server.game.players[0].name).toBe("some_player");
    });
  });
});
