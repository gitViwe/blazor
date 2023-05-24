# https://chrissainty.com/containerising-blazor-applications-with-docker-containerising-a-blazor-webassembly-app/

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
WORKDIR "/src/Blazor"
RUN dotnet build "Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazor.csproj" --no-restore -c Release -o /published

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /published/wwwroot .
COPY --from=publish /published/nginx.conf /etc/nginx/nginx.conf

RUN rm appsettings.json # remove the default 'appsettings.json' file
RUN mv appsettings.Docker.json appsettings.json # rename 'appsettings.Docker.json' to 'appsettings.json'