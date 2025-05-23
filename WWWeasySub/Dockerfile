FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY WWWeasySub.csproj .
RUN dotnet restore WWWeasySub.csproj
COPY . .
RUN dotnet build WWWeasySub.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish WWWeasySub.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf