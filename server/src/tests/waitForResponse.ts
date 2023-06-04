import { RawData, WebSocket } from "ws";

/**
 * Wait for a response from the server
 * @param {WebSocket} ws The WebSocket connection
 * @param {boolean} close Whether or not to close the connection after the response is received (defaults to true)
 * @param {number} timeoutMs The timeout in milliseconds (defaults to 1s)
 * @returns {Promise<T>} A promise that resolves to the response from the server
 */
function waitForResponse<T = any>(
  ws: WebSocket,
  close: boolean = true,
  timeoutMs: number = 1000
): Promise<T> {
  // Create a new promise (because we get an event handler function..)
  return new Promise((resolve, reject) => {
    // This is so that the promise doesn't hang forever
    // If the response isn't received after x milliseconds, reject the promise
    const timeout = setTimeout(() => {
      reject(new Error("Response timed out"));

      // Remove the event listener
      ws.off("message", onMessage);

      // Terminate the connection
      ws.terminate();
    }, timeoutMs);

    function onMessage(data: RawData) {
      // Remove the timeout
      clearTimeout(timeout);

      // Parse the message as JSON
      let message: T;
      try {
        message = JSON.parse(data.toString());
      } catch (err) {
        // If an error occurs, reject the promise
        return reject(err);
      }

      if (close) ws.close();

      // Resolve the promise
      resolve(message);
    }

    ws.once("message", onMessage);
  });
}

/**
 * Wait for multiple responses from the server
 * (just calls waitForResponse multiple times lmao)
 * @param {WebSocket} ws The WebSocket connection
 * @param {number} count The number of responses to wait for
 * @param {boolean} close Whether or not to close the connection after the response is received (defaults to true)
 * @param {number} timeout The timeout in milliseconds (defaults to 1s)
 * @returns {Promise<any>} A promise that resolves to an array of responses from the server
 */
export async function waitForResponses<T extends any[] = any[]>(
  ws: WebSocket,
  count: number,
  close: boolean = true,
  timeout: number = 1000
): Promise<T> {
  // Create a new array of responses
  // Ignore the types here. I have no clue what's up with this
  const responses: T = [] as unknown as T;

  // Wait for the specified number of responses
  for (let i = 0; i < count; i++) {
    // Wait for a response
    // The type is T[number] because T is an array and we want the type of the array items
    // Don't close the connection after the response is received because we need to wait for more responses
    const response = await waitForResponse<T[number]>(ws, false, timeout);

    // Push the response to the array
    responses.push(response);
  }

  if (close) ws.close();

  // Return the array
  return responses;
}

export default waitForResponse;
