# Étape 1 : base d'exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# Étape 2 : build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copier uniquement le fichier projet (dans le dossier actuel)
COPY ["API.csproj", "."]
RUN dotnet restore "./API.csproj"

# Copier tout le code (dans le dossier actuel)
COPY . .
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape 3 : publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape 4 : image finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
