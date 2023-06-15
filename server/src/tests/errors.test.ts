// Start a new server instance. This is the entry point for the server.
import { WebSocket } from "ws";

import { ErrorMessage } from "@/messages/error";

// The test helper functions
import createConnection, { closeConnection } from "@/helpers/createConnection";
import waitForResponse from "@/helpers/waitForResponse";
import Server from "@/server";

// Some basic tests related to errors
// Each message should receive a response with type "error" and a message
// More specific errors are tested in other files
// These are just the most basic ones
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

    // Expect the connection to be still open (cuz like, the server didn't crash or anything)
    // The waitForResponse function closes the connection if false isn't passed as the second argument
    expect(ws.readyState).toBe(WebSocket.OPEN);
  });

  test("Send random json", async () => {
    ws.send(JSON.stringify({ hello: "world" }));

    const response = await waitForResponse<ErrorMessage>(ws, false);

    // Expect the response to be an error message
    expect(response).toEqual({ type: "error", message: "Invalid message type" });

    // Expect the connection to be still open
    expect(ws.readyState).toBe(WebSocket.OPEN);
  });

  test("Send valid json with a valid type but without a registration", async () => {
    ws.send(JSON.stringify({ type: "list" }));

    const response = await waitForResponse<ErrorMessage>(ws, false);

    // Expect the response to be an error message
    expect(response).toEqual({ type: "error", message: "Not registered" });

    // Expect the connection to be still open
    expect(ws.readyState).toBe(WebSocket.OPEN);
  });
});
