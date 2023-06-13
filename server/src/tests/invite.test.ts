import { WebSocket } from "ws";

import Server from "@/server";

// The test helper functions
import createConnection, { closeConnection } from "@/helpers/createConnection";
import waitForResponse, { waitForResponses } from "@/helpers/waitForResponse";

// These tests are all about sending invites (and receiving them)
describe("Sending invitations from player to player", () => {
  let server: Server;

  // Create a new server instance before all tests with a random port
  beforeAll(done => {
    server = new Server(0, done);
  });

  // Once all tests are done, close the server instance
  afterAll(() => server.close());

  describe("With 2 players", () => {
    let websockets: [WebSocket, WebSocket];

    // Create a new connection to the server before each test
    beforeEach(async () => {
      websockets = [await createConnection(server), await createConnection(server)];
    });

    // Close the connection after each test (if it's open)
    afterEach(() => Promise.all(websockets.map(closeConnection)));

    test("Player 1 invites player 2 twice", async () => {
      // Register both players
      await Promise.all(
        websockets.map((ws, i) => {
          ws.send(JSON.stringify({ type: "register", name: `Player_${i + 1}` }));
          return waitForResponses(ws, 2, false);
        })
      );

      // Invite player 2 for the first time
      websockets[0].send(JSON.stringify({ type: "invite", name: "Player_2" }));

      // Expect player 1 and player 2 to receive an invite message
      await Promise.all(
        websockets.map(async ws => {
          const response = await waitForResponse(ws, false);
          expect(response).toEqual({ type: "invite", by: "Player_1", to: "Player_2" });
        })
      );

      // Invite player 2 for the second time
      websockets[0].send(JSON.stringify({ type: "invite", name: "Player_2" }));

      // Expect player 1 to receive an error message and player 2 to receive nothing
      await Promise.all(
        websockets.map(async (ws, i) => {
          // The 2nd player should receive nothing
          if (i == 1) expect(waitForResponse(ws, false, 50)).rejects.toThrow();
          else
            expect(await waitForResponse(ws, false)).toEqual({
              type: "error",
              message: "Invite already exists",
            });
        })
      );

      // Expect the server to have only one invite from player 1 to player 2
      expect(server.game.invites.all.length).toBe(1);
      expect(server.game.invites.all[0].player1.name).toBe("Player_1");
      expect(server.game.invites.all[0].player2.name).toBe("Player_2");
    });

    test("Player 1 invites player 2 and player 2 accepts", async () => {
      // Register both players
      await Promise.all(
        websockets.map((ws, i) => {
          ws.send(JSON.stringify({ type: "register", name: `Player_${i + 1}` }));
          return waitForResponses(ws, 2, false);
        })
      );

      // Invite player 2
      websockets[0].send(JSON.stringify({ type: "invite", name: "Player_2" }));

      // Expect player 1 and player 2 to receive an invite message
      await Promise.all(
        websockets.map(async ws => {
          const response = await waitForResponse(ws, false);
          expect(response).toEqual({ type: "invite", by: "Player_1", to: "Player_2" });
        })
      );

      // Accept the invite (this is basically just player 2 inviting player 1. the server takes this as an acception)
      websockets[1].send(JSON.stringify({ type: "invite", name: "Player_1" }));

      // Expect player 1 and player 2 to receive an accept message
      await Promise.all(
        websockets.map(async ws => {
          const response = await waitForResponse(ws, false);
          expect(response).toEqual({ type: "create", player1: "Player_1", player2: "Player_2" });
        })
      );

      // Expect the server to have no invites
      expect(server.game.invites.all.length).toBe(0);
    });
  });

  describe("With 3 players", () => {
    let websockets: [WebSocket, WebSocket, WebSocket];

    // Create a new connection to the server before each test
    beforeEach(async () => {
      websockets = [
        await createConnection(server),
        await createConnection(server),
        await createConnection(server),
      ];
    });

    // Close the connection after each test (if it's open)
    afterEach(() => Promise.all(websockets.map(closeConnection)));

    test("Player 1 invites player 2, player 3 shouldn't receive anything", async () => {
      // Register all three players
      await Promise.all(
        websockets.map((ws, i) => {
          ws.send(JSON.stringify({ type: "register", name: `p: ${i + 1}` }));
          return waitForResponses(ws, 2, false);
        })
      );

      // Make player 1 invite player 2
      websockets[0].send(JSON.stringify({ type: "invite", name: "p: 2" }));

      // Expect player 1 and player 2 to receive an invite message
      await Promise.all(
        websockets.map(async (ws, i) => {
          // If this is player 3, expect the response to throw (timeout is 50ms because no response is expected)
          if (i == 2) expect(waitForResponse(ws, false)).rejects.toThrow();
          // Otherwise, expect the response to be an invite message
          else
            expect(await waitForResponse(ws, false)).toEqual({
              type: "invite",
              by: "p: 1",
              to: "p: 2",
            });
        })
      );

      // Expect game.invites to have 1 invitation (p1 -> p2)
      expect(server.game.invites.all.length).toBe(1);
      expect(server.game.invites.all[0].player1.name).toBe("p: 1");
      expect(server.game.invites.all[0].player2.name).toBe("p: 2");

      // Expect all 3 players to be still in the game
      expect(server.game.players.all).toHaveLength(3);
      expect(server.game.players.all[0].name).toBe("p: 1");
      expect(server.game.players.all[1].name).toBe("p: 2");
      expect(server.game.players.all[2].name).toBe("p: 3");
    });

    test("Player 1 invites player 2, player 2 accepts, player 3 should be notified", async () => {
      // Register all players
      await Promise.all(
        websockets.map((ws, i) => {
          ws.send(JSON.stringify({ type: "register", name: `Player_${i + 1}` }));
          return waitForResponses(ws, 2, false);
        })
      );

      // Player 1 invites player 2
      websockets[0].send(JSON.stringify({ type: "invite", name: "Player_2" }));
      await Promise.all([
        waitForResponse(websockets[0], false),
        waitForResponse(websockets[1], false),
      ]);

      // Player 2 accepts the invite
      websockets[1].send(JSON.stringify({ type: "invite", name: "Player_1" }));
      await Promise.all(
        websockets.map(async ws => {
          const response = await waitForResponse(ws, false);

          // Expect all players to receive a create message
          expect(response).toEqual({ type: "create", player1: "Player_1", player2: "Player_2" });
        })
      );

      // Expect the server to have no invites
      expect(server.game.invites.all.length).toBe(0);
    });
  });
});
