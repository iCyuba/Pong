/** @type {import('@jest/types').Config} */
const config = {
  testEnvironment: "node",
  roots: ["<rootDir>"],
  modulePaths: ["<rootDir>"],
  transformIgnorePatterns: ["node_modules/(?!(lodash-es))"],
  moduleNameMapper: {
    "^@/(.*)$": "<rootDir>/src/$1",
  },
};

module.exports = config;
