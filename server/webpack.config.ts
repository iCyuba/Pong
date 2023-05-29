import NodemonPlugin from "nodemon-webpack-plugin";
import TsconfigPathsPlugin from "tsconfig-paths-webpack-plugin";

export default {
  mode: process.env.NODE_ENV === "development" ? "development" : "production",
  target: "node",
  resolve: {
    plugins: [new TsconfigPathsPlugin()],
    // Add `.ts` and `.tsx` as a resolvable extension.
    extensions: [".ts", ".tsx", ".js"],
    // Add support for TypeScripts fully qualified ESM imports.
    extensionAlias: {
      ".js": [".js", ".ts"],
      ".cjs": [".cjs", ".cts"],
      ".mjs": [".mjs", ".mts"],
    },
  },
  module: {
    rules: [
      // all files with a `.ts`, `.cts`, `.mts` or `.tsx` extension will be handled by `ts-loader`
      { test: /\.([cm]?ts|tsx)$/, loader: "ts-loader" },
    ],
  },
  plugins: [new NodemonPlugin()],
  stats: "errors-only",
};
