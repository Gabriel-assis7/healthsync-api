# HealthSync API — Personal Learning Project

> Note: This is a personal, learning-focused project aimed at exploring scalable distributed systems, cloud-native architecture, and observability. The repository is under active development and many pieces are intentionally experimental.

## Status
- Project purpose: study and prototype patterns for microservices, messaging, caching, telemetry, and infrastructure-as-code.
- Active development: features, services and infra are in-progress. Expect frequent changes and incomplete implementations.
- Incomplete services & planned features: the `Doctors` and `Patients` services are currently scaffolds (no runtime APIs implemented). Planned capabilities include request/response APIs, background message processors, end-to-end tracing, CI/CD, Kubernetes manifests, Terraform provisioning and an AI-driven review/insights service.

## Purpose
The primary goal is to learn and demonstrate patterns for:
- Scalable distributed systems (microservices, message brokers)
- Cloud-native architecture (containerized services, infra-as-code)
- Observability (metrics, traces, logs, dashboards)

## Architecture Overview

This repository follows a small microservice-style layout with reusable building blocks and example services. Key components:
- Services: lightweight service projects (scaffolds) under `src/Services`.
- Building blocks: reusable libraries for logging, telemetry, transports, resiliency, caching and data access in `src/BuildingBlocks`.
- Messaging: RabbitMQ for async messaging (building blocks + transports).
- Data: MongoDB as primary document store; Redis for caching.
- Observability: OpenTelemetry instrumentation, Prometheus (metrics), Jaeger (traces), Grafana (dashboards), Loki/Elasticsearch for logs.
- Local infra: Docker Compose manifests under `deployments/docker-compose` to bring up Prometheus, Grafana, Jaeger, MongoDB, Redis and exporters.

**Mermaid architecture diagram**

```mermaid
graph LR
  subgraph Clients
    C[Clients / API Consumers]
  end

  subgraph Services
    S1[Doctors Service]
    S2[Patients Service]
  end

  subgraph Messaging
    MQ[RabbitMQ]
  end

  subgraph Data
    MDB[MongoDB]
    RED[Redis Cache]
  end

  subgraph Observability
    OTEL[OpenTelemetry]
    JG[Jaeger]
    PR[Prometheus]
    GF[Grafana]
    LK[Loki]
    NE[Node Exporter]
  end

  C -->|HTTP/gRPC| S1
  C -->|HTTP/gRPC| S2

  S1 -->|read/write| MDB
  S2 -->|read/write| MDB
  S1 -->|cache| RED
  S2 -->|cache| RED
  S1 -->|publish/consume| MQ
  S2 -->|publish/consume| MQ

  S1 --> OTEL
  S2 --> OTEL
  OTEL --> JG
  OTEL --> PR
  PR --> GF
  JG --> GF
  LK --> GF
  NE --> PR

  subgraph Local Infra
    DC[docker-compose (deployments/docker-compose)]
    DC --> PR
    DC --> GF
    DC --> JG
    DC --> MDB
    DC --> RED
  end
```

## Technologies, Libraries and Versions

- **Platform**: .NET 8.0 (TargetFramework: `net8.0`, see `src/Directory.Build.Props`)
- **.NET packages (selected, by project)**
  - `HealthSync.BuildingBlocks.OpenTelemetry`
    - Microsoft.Extensions.Hosting 10.0.7
    - OpenTelemetry.Exporter.Console 1.15.3
    - OpenTelemetry.Exporter.Prometheus.AspNetCore 1.15.3-beta.1
    - OpenTelemetry.Extensions.Hosting 1.9.0
    - OpenTelemetry.Instrumentation.* 1.9.0
    - OpenTelemetry.Exporter.OpenTelemetryProtocol 1.15.3
  - `HealthSync.BuildingBlocks.MongoDB`
    - MongoDB.Driver 3.6.0
    - Microsoft.Extensions.Hosting 8.0.0
  - `HealthSync.BuildingBlocks.RabbitMQ` / `Transport`
    - RabbitMQ.Client 7.2.1
  - `HealthSync.BuildingBlocks.Redis`
    - Microsoft.Extensions.Caching.StackExchangeRedis 10.0.7
    - StackExchange.Redis 2.12.14
  - `HealthSync.BuildingBlocks.Resiliency`
    - Microsoft.Extensions.Http.Resilience 10.4.0
    - Polly.Core 8.6.6
  - `HealthSync.BuildingBlocks.Logging`
    - Serilog 4.3.1, Serilog.Sinks.Console 6.1.1, Serilog.Sinks.File 7.0.0
    - Elastic.Serilog.Sinks 9.0.0, Serilog.Sinks.Seq 9.0.0

- **Node / Dev tools** (repository root `package.json`)
  - `yargs-parser` ^21.1.1
  - Dev: `@commitlint/cli` ^21.0.1, `@commitlint/config-conventional` ^21.0.1, `lefthook` ^2.1.6

- **Containers / Images (local docker-compose)**
  - prom/prometheus (v3.6.0 in `docker-compose.infrastructure.yml`, or `latest` in the main compose)
  - grafana/grafana (no tag in compose, default to latest)
  - jaegertracing/all-in-one:latest
  - prom/node-exporter
  - mongo:4.4
  - redis (no tag in compose)

- **Terraform (infrastructure modules)**
  - Terraform module skeletons exist under `infrastructure/modules/networking` (AWS VPC/subnets resources present; see files in that folder).

Files that help locate these settings:
- [src/Directory.Build.Props](src/Directory.Build.Props)
- [package.json](package.json)
- [deployments/docker-compose/docker-compose.yml](deployments/docker-compose/docker-compose.yml)
- [deployments/docker-compose/docker-compose.infrastructure.yml](deployments/docker-compose/docker-compose.infrastructure.yml)
- [deployments/configs/grafana/provisioning/datasources/datasource.yml](deployments/configs/grafana/provisioning/datasources/datasource.yml)

## Observability Stack

This project includes an observability-focused local stack (docker-compose):

- **OpenTelemetry**: instrumentation libraries are included in the building blocks to emit traces and metrics.
- **Prometheus**: metrics scraping and storage (`deployments/docker-compose/docker-compose.yml`, `deployments/configs/prometheus.yml`).
- **Grafana**: dashboards and provisioning are included under `deployments/configs/grafana/provisioning` (datasource provisioning for Prometheus, Jaeger, Loki).
- **Jaeger**: tracing backend (all-in-one used in compose).
- **Loki / Elasticsearch**: Grafana provisioning references Loki and Elasticsearch; a Loki config file exists under `deployments/configs/loki-config.yml` (Loki service is referenced in provisioning but not enabled by default in the main compose).
- **Node exporter**: OS metrics exporter for Prometheus.

See `deployments/docker-compose/docker-compose.yml` and `deployments/configs` for the local observability configuration.

## Infrastructure

There is an initial Terraform networking module under `infrastructure/modules/networking` which contains VPC, subnet and region helpers intended for future AWS provisioning. These files are scaffolding for the eventual infrastructure-as-code workflow:

- [infrastructure/modules/networking](infrastructure/modules/networking)

At the moment the Terraform code is module-level scaffolding (variables, locals, VPC/subnet resources) and should be reviewed/extended before use.

## Roadmap & Planned Features

- Short term
  - Stabilize building blocks (logging, telemetry, resiliency)
  - Implement HTTP/gRPC endpoints for `Doctors` and `Patients` services
  - Add integration tests and basic CI pipeline
- Mid term
  - Add message-based workflows and durable background processors (RabbitMQ)
  - Add persistent tracing/metrics storage and retention strategies
  - Add Loki/Elastic logging pipeline and dashboarding
  - Implement an AI review/analysis microservice (branch: feature/add-ai-review)
- Long term
  - Full AWS provisioning via Terraform (VPC, EKS, managed databases)
  - Kubernetes manifests + Helm charts, migrate local compose to k8s
  - GitHub Actions / CI/CD with automated tests, linting and rollout
  - Service mesh evaluation (e.g., Linkerd/Istio) and multi-region experiments

## Future AWS / Terraform / Kubernetes Plans

The long-term plan is to deploy the system to AWS using Terraform to provision networking, clusters (EKS) and managed services (DocumentDB/MongoDB Atlas or managed Mongo, ElastiCache for Redis, RabbitMQ as a managed/external service). Kubernetes manifests / Helm charts will be added to support production-like clusters and GitOps-style deployments.

## Setup (currently implemented services)

Only the local observability/infrastructure items and the .NET building blocks are currently runnable from source. The example service projects are scaffolds and do not expose production-ready endpoints yet.

Prerequisites
- Docker & Docker Compose (stable release)
- .NET 8 SDK (matching `net8.0` target)
- Optional: Node.js (for commit hooks and dev tooling)

Bring up the local observability & infra stack

```bash
cd deployments/docker-compose
docker compose up -d
```

## Contributing

This is a personal learning repo. Contributions are welcome as discussions or suggested PRs, but please respect that the main goal is learning and experimentation.

## License

This repository is for personal learning. No explicit license is included — treat it as a learning artifact.
