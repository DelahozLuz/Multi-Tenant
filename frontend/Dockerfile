FROM node:22-alpine AS build

ARG API_URL=/api

WORKDIR /app
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN echo "export const environment = { production: true, apiUrl: '${API_URL}' };" > src/environments/environment.prod.ts
RUN npm run build

FROM nginx:alpine AS runtime
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html
EXPOSE 80
