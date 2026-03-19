# Project Hub: Blazor WASM

This repository contains a full-stack authentication system featuring a **.NET 10 Web API** backend and a **Blazor WebAssembly** frontend.
The solution is built with scalability in mind, supported by a robust **OpenTelemetry** observability stack and **Nginx** acting as a unified API Gateway.

---

# How to Run All Apps

The quickest way to deploy the entire environment is using **Docker Compose**.

### Prerequisites
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.
* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (required only for local development/testing).

### Execution Steps
1.  Open your terminal in the root directory of the repository.
2.  Spin up the infrastructure:
    ```bash
    docker-compose up -d --build
    ```
3.  **Note:** Wait for the `postgres` health check to pass. The `auth-api` is configured to restart automatically until the database becomes available.

---

## Service Map & Endpoints

| Service | Public URL | Description |
| :--- | :--- | :--- |
| **Blazor WASM** | `http://localhost:5109` | Main UI / Web Entry Point |
| **Auth API** | `http://localhost:5056` | Direct Backend API access |
| **API Documentation** | `http://localhost:5056/scalar` | Scalar/Swagger Docs (via Nginx) |
| **Seq** | `http://localhost:81` | Structured Log UI |
| **Jaeger** | `http://localhost:16686` | Distributed Tracing UI |
| **Grafana** | `http://localhost:3000/d/KdDACDp4z/asp-net-core?orgId=1` | Metrics Dashboards (Admin: `admin/admin`) |
| **OTEL Collector** | `http://localhost:4318` | OTLP HTTP Telemetry Ingestion |

---

## Observability Flow

The entire stack is instrumented with **OpenTelemetry** for deep system insights:

* **Traces:** Sent to **Jaeger** to visualize request flow and identify bottlenecks.
* **Logs:** Sent to **Seq** for centralized, searchable structured logging.
* **Metrics:** Scraped by **Prometheus** and visualized through pre-configured **Grafana** dashboards.
