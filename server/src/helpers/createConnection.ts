import { WebSocket } from "ws";

import Server from "@/server";

/**
 * Creates a new websocket connection to the server
 * @param {Server} server The server to connect to
 * @returns {Promise<WebSocket>} A promise that resolves to a new websocket connection
 */
function createConnection(server: Server): Promise<WebSocket> {
  return new Promise((resolve, reject) => {
    // This is so that the promise doesn't hang forever
    // If the connection isn't open after a second, reject the promise
    const timeout = setTimeout(() => {
      if (ws.readyState !== WebSocket.OPEN) {
        reject(new Error("Connection timed out"));

        ws.terminate();
      }
    }, 1000);

    // Get the port from the server (this is the random port we generated)
    const port = server.port;

    // Create a new websocket connection to the server on the random port
    const ws = new WebSocket(`ws://localhost:${port}`);

    // Once the connection is open, resolve the promise and clear the timeout
    ws.once("open", () => {
      clearTimeout(timeout);

      resolve(ws);
    });
  });
}

/**
 * Closes a websocket connection
 * @param {WebSocket} ws The websocket connection to close
 * @returns {Promise<void>} A promise that resolves when the connection is closed
 */
export async function closeConnection(ws: WebSocket): Promise<void> {
  return new Promise((resolve, reject) => {
    // If the connection is already closed, resolve the promise
    if (ws.readyState !== WebSocket.OPEN) return resolve();

    // This is so that the promise doesn't hang forever
    // If the connection isn't closed after a second, reject the promise and terminate the connection
    const timeout = setTimeout(() => {
      reject(new Error("Connection timed out"));
      ws.terminate();
    }, 1000);

    // Once the connection is closed, resolve the promise and clear the timeout
    ws.once("close", () => {
      clearTimeout(timeout);

      resolve();
    });

    // Close the connection
    ws.close();
  });
}

export default createConnection;
