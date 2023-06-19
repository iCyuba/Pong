import path from "path";

// This is the entry point of the server except it uses ssl
import Server from "@/server";

// Create a new WebSocket server on port 3000
const server = new Server(
  3000,
  {
    key_file_name: path.join(__dirname, require("../ssl/key.pem")),
    cert_file_name: path.join(__dirname, require("../ssl/cert.pem")),
  },
  () => console.log(new Date(), "Server started! (port 3000)")
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
