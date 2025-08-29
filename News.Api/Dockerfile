# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["News.Api/News.Api.csproj", "News.Api/"]
RUN dotnet restore "News.Api/News.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/News.Api"
RUN dotnet build "News.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "News.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Development stage with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy csproj and restore dependencies
COPY ["News.Api/News.Api.csproj", "News.Api/"]
RUN dotnet restore "News.Api/News.Api.csproj"

# Copy everything else
COPY . .

# For development with hot reload
ENTRYPOINT ["dotnet", "watch", "run", "--project", "/app/News.Api/News.Api.csproj", "--urls", "http://+:8080"]

# Production stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "News.Api.dll"]
