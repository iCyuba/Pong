FROM node:18-alpine as base

# Build step
FROM base as build

WORKDIR /app

# Install all dependencies
COPY package*.json ./
RUN npm ci

# Build the app
COPY tsconfig.json ./
COPY src ./src
RUN npm run build

# Run step
FROM base as run

WORKDIR /app

# Install only production dependencies
COPY package*.json ./
RUN npm ci --only=production

# Copy the built app
COPY --from=build /app/dist ./dist

# Run the app
CMD ["npm", "run", "start"]
