# Stage 1: Build Angular App
FROM node:14-alpine AS angular-build
WORKDIR /app

# Copy the Angular project from the client directory
COPY ./client/package*.json ./
RUN npm install

COPY ./client ./

# Build the Angular app
RUN npm run build --prod

# Stage 2: Build ASP.NET Web API
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build ASP.NET Web API
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 3: Build Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy ASP.NET Web API from the previous stage
COPY --from=build-env /app/out .

# Copy Angular app from the first stage
COPY --from=angular-build /app/dist /app/wwwroot

# Expose port and entry point
EXPOSE 8080
ENTRYPOINT [ "dotnet", "API.dll" ]
