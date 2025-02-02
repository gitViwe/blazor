<!-- ABOUT THE PROJECT -->
# Blazor

Blazor WebAssembly is a popular framework for building interactive web applications using C# and .NET

<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

Things you need to use the software and how to install them.
* Your IDE of choice: 
  * [Visual Studio](https://visualstudio.microsoft.com/)
  * [Rider](https://www.jetbrains.com/rider/)
  * [WebStorm](https://www.jetbrains.com/webstorm/)
* [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet)
* [Docker](https://www.docker.com/) (optional)
* [Node.js](https://nodejs.org/en/download) (optional)

### Installation

- Clone the repo
   ```shell
   git clone https://github.com/gitViwe/blazor.git
   ```
- Run with Docker: navigate to the root directory `blazor`
   ```shell
   docker compose up --build -d
   ```
  - Then navigate to [localhost:5249](http://localhost:5249)
  

- Run with IDE: navigate to the root directory `blazor`
   ```shell
   dotnet run --project src\Blazor\Blazor.csproj
   ```
  - Then navigate to [localhost:5249](http://localhost:5249)
