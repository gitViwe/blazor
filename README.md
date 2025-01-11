<!-- ABOUT THE PROJECT -->
# Blazor

.NET client library using Blazor Web Assembly to run C#

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

Things you need to use the software and how to install them.
* [Visual Studio / Visual Studio Code](https://visualstudio.microsoft.com/)
* [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet)
* [Node.js v18.16.0](https://nodejs.org/en/download)
* [Docker](https://www.docker.com/)

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/gitViwe/blazor.git
   ```
2. Generate certificate. [Starting a container with https support using docker compose](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-5.0#starting-a-container-with-https-support-using-docker-compose)
   ```
   dotnet dev-certs https -ep .aspnet\https\blazor-wasm.pfx -p password!
   dotnet dev-certs https --trust
   ```
3. Run via Docker
   ```
   docker compose up --build -d
   ```
3. Run via Visual Studio / Visual Studio Code
   ```
   cd src/Blazor.Presentation/NpmJS
   npm install
   npm run build_local
   
   docker compose --file docker-compose.development.yml up -d
   cd src/Blazor
   dotnet run
   ```

Then navigate to [localhost:5086](http://localhost:5086)
