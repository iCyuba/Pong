// Start a new server instance. This is the entry point for the server.
import { WebSocket } from "ws";

// The test helper functions
import createConnection, { closeConnection } from "@/tests/createConnection";
import waitForResponse from "@/tests/waitForResponse";

import { ErrorMessage } from "@/messages";
import Server from "@/server";

// All tests related to errors
// Each message should receive a response with type "error" and a message
describe("Try sending invalid messages to the server", () => {
  let server: Server;
  let ws: WebSocket;

  // Create a new server instance before all tests with a random port
  beforeAll(done => {
    server = new Server({ port: 0 }, done);
  });

  // Once all tests are done, close the server instance
  afterAll(done => server.close(done));

  // Create a new connection to the server before each test
  beforeEach(async () => {
    ws = await createConnection(server);
  });

  // Close the connection after each test (if it's open)
  afterEach(() => closeConnection(ws));

  test("Send a string (non-json)", async () => {
    ws.send("hello world");

    const response = await waitForResponse<ErrorMessage>(ws, false);

    // Expect the response to be an error message
    expect(response.type).toBe("error");
    expect(response.message).toBe("Unexpected token 'h', \"hello world\" is not valid JSON");

    // Expect the connection to be still open (cuz like, the server didn't crash or anything)
    // The waitForResponse function closes the connection if false isn't passed as the second argument
    expect(ws.readyState).toBe(WebSocket.OPEN);
  });

  test("Send random json", async () => {
    ws.send(JSON.stringify({ hello: "world" }));

    const response = await waitForResponse<ErrorMessage>(ws, false);

    // Expect the response to be an error message
    expect(response.type).toBe("error");
    expect(response.message).toBe("Invalid message type");

    // Expect the connection to be still open
    expect(ws.readyState).toBe(WebSocket.OPEN);
  });

  test("Send valid json with a valid type but without a registration", async () => {
    ws.send(JSON.stringify({ type: "list" }));

    const response = await waitForResponse<ErrorMessage>(ws, false);

    // Expect the response to be an error message
    expect(response.type).toBe("error");
    expect(response.message).toBe("Not registered");

    // Expect the connection to be still open
    expect(ws.readyState).toBe(WebSocket.OPEN);
  });
});
