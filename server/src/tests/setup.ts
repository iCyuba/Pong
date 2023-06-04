// This is required for the require.context() function to work
// Cuz this ain't running under webpack...
require("babel-plugin-require-context-hook/register")();

// Disable console logs
// They're annoying
global.console.log = () => {};
