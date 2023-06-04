/** @type {import('@jest/types').Config.ProjectConfig} */
const config = {
  testEnvironment: "node",
  roots: ["<rootDir>"],
  modulePaths: ["<rootDir>"],
  transformIgnorePatterns: ["node_modules/(?!(lodash-es))"],
  moduleNameMapper: {
    "^@/(.*)$": "<rootDir>/src/$1",
  },
  setupFiles: ["<rootDir>/src/tests/setup.ts"],
};

module.exports = config;
