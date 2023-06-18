# https://chrissainty.com/containerising-blazor-applications-with-docker-containerising-a-blazor-webassembly-app/

FROM node:alpine AS nodebuild
ADD Blazor.Presentation ./Blazor.Presentation
WORKDIR /Blazor.Presentation/NpmJS
RUN npm install
RUN npm run build_local

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
EXPOSE 80
EXPOSE 443

WORKDIR /src

COPY ["Blazor/Blazor.csproj", "Blazor/"]
COPY ["Blazor.Infrastructure/Blazor.Infrastructure.csproj", "Blazor.Infrastructure/"]
COPY ["Blazor.Presentation/Blazor.Presentation.csproj", "Blazor.Presentation/"]
COPY ["Shared/Shared.csproj", "Shared/"]

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
COPY --from=nodebuild /Blazor.Presentation/wwwroot/js/index.bundle.js _content/Blazor.Presentation/js/index.bundle.js
COPY --from=nodebuild /Blazor.Presentation/wwwroot/js/open_telemetry.bundle.js _content/Blazor.Presentation/wwwroot/js/open_telemetry.bundle.js

# remove the default 'appsettings.json' file
RUN rm appsettings.json
# rename 'appsettings.Docker.json' to 'appsettings.json'
RUN mv appsettings.Docker.json appsettings.json