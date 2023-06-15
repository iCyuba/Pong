// This is the entry point of the server, where the server is created and started (yes, it's literally just one line)
import Server from "@/server";

// Create a new WebSocket server on port 3000
const server = new Server({ port: 3000 }, () =>
  console.log(new Date(), "Server started! (port 3000)")
);

// Add a listener for SIGINT (Control + C) to stop the server
process.on("SIGINT", () => {
  // Stop the server
  server.close();

  // Log that the server has stopped
  console.log(new Date(), "Server stopped!");

  // Exit the process
  process.exit();
});
