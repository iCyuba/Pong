// This is just a barrel file. It re-exports all of the message generators in this folder.

// This is defined here because I only use the type in tests
export interface ErrorMessage {
  type: "error";
  message: string;
}

export { default as List } from "@/messages/list";
export { default as Register } from "@/messages/register";
export { default as Unregister } from "@/messages/unregister";
