# Build
FROM node:20-alpine AS build

WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

COPY . .

EXPOSE 65196

# Run client app in dev mode
CMD ["npm", "run", "dev"]