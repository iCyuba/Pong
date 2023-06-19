import { WebSocket } from "ws";

import Server from "@/server";

import createConnection, { closeConnection } from "@/helpers/createConnection";
import waitForResponse, { waitForResponses } from "@/helpers/waitForResponse";
import Player from "@/players/player";

/** Don't ask. Just use them */
export const usernames = [
  "test",
  "what",
  "i dont even know",
  "16 characters ^^",
  "a",
  "why did i",
  "even make this",
  "stupid list",
  "of names?",
  "---",
  "oh, special",
  "characters are",
  "supported as",
  "well.",
  "~±<+-;œ∑´®†¥¨ˆø",
  "i value my time",
  "as you can see",
  "from this list",
  "of creative",
  "usernames <3",
];

export const invalidUsernames: { name: unknown; error: string }[] = [
  { name: undefined, error: "A name must be a string (got undefined)" },
  { name: 123, error: "A name must be a string (got number)" },
  { name: /a/, error: "A name must be a string (got object)" },
  { name: null, error: "A name must be a string (got object)" },
  { name: false, error: "A name must be a string (got boolean)" },
  { name: "", error: "A name must not be empty" },
  { name: "AAAAAAAAAAAAAAAAAAAA", error: "A name must not be longer than 16 characters" },
];

describe("Offline tests", () => {
  describe("Player.validateName and Player.safeValidateName", () => {
    // Try validating all sorts of garbage. What is the point of this??
    test.each(invalidUsernames)("Invalid: $name", ({ name, error }) => {
      // Calling Player.validateName should throw.
      expect(() => Player.validateName(name as string)).toThrowError(error);

      // Calling Player.safeValidateName should return false
      expect(Player.safeValidateName(name as string)).toBe(false);
    });

    // Very great (valid) names
    test.each(usernames)("Valid: %s", name => {
      expect(() => Player.validateName(name)).not.toThrow();

      expect(Player.safeValidateName(name)).toBe(true);
    });
  });
});

describe("Online tests", () => {
  let server: Server;

  // Create a new server instance before all tests with a random port
  beforeAll(done => {
    server = new Server(0, undefined, done);
  });

  // Close the server after all tests
  afterAll(() => server.close());

  describe("Register with names", () => {
    let ws: WebSocket;

    // Create a new connection to the server before each test
    beforeEach(async () => {
      ws = await createConnection(server);
    });

    // Close the connection after each test (if it's open)
    afterEach(async () => {
      await closeConnection(ws);
    });

    // Try registering with an invalid name
    test.each(invalidUsernames)("Invalid: $name", async ({ name, error }) => {
      ws.send(JSON.stringify({ type: "register", name }));

      const response = await waitForResponse(ws, false);

      // Expect the response to be an error message
      expect(response).toMatchObject({ type: "error", message: error });
    });

    // Register with a valid name
    test.each(usernames)("Valid: %s", async name => {
      const register = { type: "register", name };
      ws.send(JSON.stringify(register));

      const [response] = await waitForResponses(ws, 2, false);

      // Expect the response to be a register message
      expect(response).toMatchObject(register);
    });
  });
});
