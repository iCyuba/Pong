// This is the entry point of the server, where the server is created and started (yes, it's literally just one line)
import Server from "@/server";

// Create a new WebSocket server on port 3000
new Server({ port: 3000 }, () => console.log(new Date(), "Server started! (port 3000)"));
