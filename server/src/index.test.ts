import { WebSocket } from "ws";

// Start a new server instance. This is the entry point for the server.
import Server from "@/server";

// This is required for the require.context() function to work
// Cuz this ain't running under webpack...
require("babel-plugin-require-context-hook/register")();

// The server and the websocket connection
// They're used in multiple tests, so they're declared here
let server: Server;
let ws: WebSocket;

// Close the server after all tests are done
afterAll(() => server.close());

// Start the server
// This resolves when the server is ready
test("start server", () =>
  new Promise<void>(resolve => {
    server = new Server({ port: 3000 }, () => {
      console.log(new Date(), "Server started! (port 3000)");

      resolve();
    });
  }));

// Connect to the server
// This resolves when the connection is open
test("connect to server", () =>
  new Promise<void>(resolve => {
    ws = new WebSocket("ws://localhost:3000");

    ws.once("open", () => {
      // Look, idk what this is. Don't ask
      expect(ws.readyState).toBe(WebSocket.OPEN);
      resolve();
    });
  }));

// Invalid messages should not close the connection
// Simply an { type: "error" } message should be sent back
describe("invalid messages", () => {
  // Attempt to send an invalid json
  test("send invalid json", () =>
    new Promise<void>(resolve => {
      expect(ws.readyState).toBe(WebSocket.OPEN);

      // Expect the server to send an error message
      ws.once("message", data => {
        resolve();

        const response = JSON.parse(data.toString());

        // Expect the error message to be: Unexpected token 'i', "invalid message" is not valid JSON
        expect(response.type).toBe("error");
        expect(response.message).toBe(
          "Unexpected token 'i', \"invalid message\" is not valid JSON"
        );
      });

      // Expect the connection to remain open after sending an invalid message
      ws.send("invalid message");
      expect(ws.readyState).toBe(WebSocket.OPEN);
    }));

  // Send valid json but with an invalid type
  test("send invalid type", () =>
    new Promise<void>(resolve => {
      expect(ws.readyState).toBe(WebSocket.OPEN);

      // Expect the server to send an error message
      ws.once("message", data => {
        resolve();

        const response = JSON.parse(data.toString());

        // Expect the error message to be "Invalid message type"
        expect(response.type).toBe("error");
        expect(response.message).toBe("Invalid message type");
      });

      // Expect the connection to remain open after sending an invalid message
      ws.send(JSON.stringify({ type: "invalid" }));
      expect(ws.readyState).toBe(WebSocket.OPEN);
    }));

  // Valid event but not registered (so it's invalid)
  test("send valid but unregistered event", () =>
    new Promise<void>(resolve => {
      expect(ws.readyState).toBe(WebSocket.OPEN);

      // Expect the server to send an error message
      ws.once("message", data => {
        resolve();

        const response = JSON.parse(data.toString());

        // Expect the error message to be "Not registered"
        expect(response.type).toBe("error");
        expect(response.message).toBe("Not registered");
      });

      // Expect the connection to remain open after sending a list event (without being registered yet)
      ws.send(JSON.stringify({ type: "list" }));
      expect(ws.readyState).toBe(WebSocket.OPEN);
    }));
});

// Disconnect from the server
test("disconnect from server", () =>
  new Promise<void>(resolve => {
    ws.once("close", () => {
      // Expect the connection to be closed
      expect(ws.readyState).toBe(WebSocket.CLOSED);

      resolve();
    });

    ws.close();
  }));

// Stop the server
test("stop server", () =>
  new Promise<void>(resolve => {
    server.close(() => {
      console.log(new Date(), "Server stopped!");

      resolve();
    });
  }));
