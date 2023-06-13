import ForkTsCheckerWebpackPlugin from "fork-ts-checker-webpack-plugin";
import NodemonPlugin from "nodemon-webpack-plugin";
import TsconfigPathsPlugin from "tsconfig-paths-webpack-plugin";

export default {
  mode: process.env.NODE_ENV === "development" ? "development" : "production",
  target: "node",
  resolve: {
    plugins: [new TsconfigPathsPlugin()],
    extensions: [".js", ".ts", ".jsx", ".tsx"],
  },
  module: {
    rules: [
      // JavaScript and TypeScript is handled by babel-loader
      { test: /\.(js|jsx|tsx|ts)$/, exclude: /node_modules/, loader: "babel-loader" },
      // .node files are handled by node-loader
      {
        test: /\.node$/,
        loader: "node-loader",
      },
    ],
  },
  plugins: [
    new ForkTsCheckerWebpackPlugin({
      typescript: {
        diagnosticOptions: {
          semantic: true,
          syntactic: true,
        },
      },
    }),
    new NodemonPlugin(),
  ],
  watchOptions: {
    ignored: /node_modules/,
  },
  stats: "errors-only",
};
