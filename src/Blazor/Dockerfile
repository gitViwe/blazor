# https://chrissainty.com/containerising-blazor-applications-with-docker-containerising-a-blazor-webassembly-app/

FROM node:alpine AS nodebuild
ADD npm ./npm
WORKDIR /npm
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
EXPOSE 8080
EXPOSE 8081

WORKDIR /src

COPY ["Blazor/Blazor.csproj", "Blazor/"]
COPY ["Blazor.Component/Blazor.Component.csproj", "Blazor.Component/"]
COPY ["Blazor.Component.WebAuthn/Blazor.Component.WebAuthn.csproj", "Blazor.Component.WebAuthn/"]
COPY ["Blazor.Shared/Blazor.Shared.csproj", "Blazor.Shared/"]

RUN dotnet restore "Blazor/Blazor.csproj"

COPY . .
WORKDIR /src/Blazor
RUN dotnet build "Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazor.csproj" --no-restore -c Release -o /published

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /published/wwwroot .
COPY --from=publish /published/nginx.conf /etc/nginx/nginx.conf

# Use sed to replace the GatewayApi value
RUN sed -i 's~<gateway-api-endpoint>~https://hubviwe-api-gateway.azurewebsites.net~g' appsettings.json