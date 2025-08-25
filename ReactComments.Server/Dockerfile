# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Copy projects and restore
WORKDIR /src
COPY ["ReactComments.Server/ReactComments.Server.csproj", "/src/ReactComments.Server/"]
COPY ["ReactComments.DAL/ReactComments.DAL.csproj", "/src/ReactComments.DAL/"]
COPY ["ReactComments.Services/ReactComments.Services.csproj", "/src/ReactComments.Services/"]
COPY ["ReactComments.Server/appsettings.json", "/src/ReactComments.Server/appsettings.json"]

WORKDIR "/src/ReactComments.Server"
RUN dotnet restore "ReactComments.Server.csproj"
COPY . .

WORKDIR "/src/ReactComments.Server"
RUN dotnet build "./ReactComments.Server.csproj" -c Release -o /app/build /p:UseAppHost=false

RUN dotnet publish "./ReactComments.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published app
COPY --from=build /app/publish ./

# App entrypoint script
#COPY --from=build /src/ReactComments.Server/entrypoint.sh ./entrypoint.sh
#RUN chmod +x /app/entrypoint.sh

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
#ENTRYPOINT ["/app/entrypoint.sh"]
