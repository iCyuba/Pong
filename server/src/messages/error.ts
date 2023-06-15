export interface ErrorMessage {
  type: "error";
  message: string;
}

/**
 * A message that is sent when an error occurs
 *
 * Sent to the player who caused the error
 * @param {string | Error} err The error message
 * @returns {ErrorMessage} An Error message
 */
// Note: This is named NewError because Error is a built-in type. This is to avoid confusion of both myself and the compiler lmaoo
export default function NewError(err: string | Error): ErrorMessage {
  // If the error is an Error object, get the message otherwise use the string (I'm using toString cuz of the types)
  const message = err instanceof Error ? err.message : err.toString();

  // Also log the error (this isn't console.error because the test fails if it is)
  console.log(new Date(), message);

  // Generate the message
  return {
    type: "error",
    message,
  };
}
