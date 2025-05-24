# Étape 1 : build du projet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY WWWeasySub.csproj .
RUN dotnet restore WWWeasySub.csproj
COPY . .
RUN dotnet build WWWeasySub.csproj -c Release -o /app/build

# Étape 2 : publish sans le host
FROM build AS publish
RUN dotnet publish WWWeasySub.csproj -c Release -o /app/publish /p:UseAppHost=false

# Étape 3 : image finale NGINX pour servir le site statique
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
