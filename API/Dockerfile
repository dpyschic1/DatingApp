# Stage 1: Build the Angular app
FROM node:18 AS angular-build
WORKDIR /app
COPY client/package*.json ./
RUN npm install
COPY client/ ./
RUN npm run build --prod

# Stage 2: Build the ASP.NET Web API
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS dotnet-build
WORKDIR /app
COPY API/*.csproj ./API/
WORKDIR /app/API
RUN dotnet restore
COPY API/ ./API/
RUN dotnet publish -c Release -o /app/out

# Stage 3: Serve both Angular app and ASP.NET Web API
FROM nginx:alpine
COPY --from=angular-build /app/dist/your-angular-app /usr/share/nginx/html
COPY --from=dotnet-build /app/out /app
COPY nginx.conf /etc/nginx/nginx.conf

# Configure Nginx to proxy requests to the API
