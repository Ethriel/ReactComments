# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project and restore
COPY ["ReactComments.Server.csproj", "."]
RUN dotnet restore "./ReactComments.Server.csproj"

# Copy all source files
COPY . .
RUN dotnet build "./ReactComments.Server.csproj" -c Release -o /app/build
RUN dotnet publish "./ReactComments.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published app
COPY --from=build /app/publish ./

# Create folder for mounted certificates
RUN mkdir -p /root/.aspnet/https

EXPOSE 8080
EXPOSE 8081

# Set environment variables for Kestrel
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_URLS=https://+:8081;http://+:8080
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/root/.aspnet/https/reactcomments.client.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=""

# Start server
ENTRYPOINT ["dotnet", "ReactComments.Server.dll"]
