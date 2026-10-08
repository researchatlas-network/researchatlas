# ResearchAtlas Agent Guide

## Language

Use English exclusively for project instructions, skills, class names, variables, methods, code comments, database objects, and other technical artifacts.

## Glossary

- **Luna:** Public-facing ASP.NET Core host and its React client application.
- **Terra:** Internal ASP.NET Core host and its React client application for managing profiles and evaluation workflows.
- **Worker:** Executable host for scheduled, queued, and long-running background work.
- **Domain:** Core business model, rules, invariants, and domain services, independent of delivery and infrastructure.
- **Application:** Use-case orchestration layer that coordinates domain behavior, contracts, and transactions.
- **Abstractions:** Stable interfaces, contracts, commands, queries, and DTOs shared across project boundaries.
- **Infrastructure:** Implementations of external concerns such as persistence, caching, storage, HTTP integrations, and templating.
- **Host:** Deployable application entry point that configures and composes dependencies for a delivery channel.
- **Migration:** Versioned SQL Server schema change defined in `ResearchAtlas.Database.Migrations.SqlServer`.

## Business Glossary

- **Research Profile:** A structured public record that presents a researcher's professional and scholarly information.
- **Researcher:** A person whose research activity, expertise, and outputs are represented in a research profile.
- **Institution:** An organization associated with researchers, research profiles, or evaluation activity.
- **Committee:** A group of appointed reviewers responsible for conducting or overseeing a defined type of review.
- **Evaluation Committee:** A committee responsible for conducting or overseeing evaluations.
- **Appeals Committee:** An appointed committee responsible for reviewing appeals and allegations submitted against resolutions.
- **Committee Appointment Document:** A formal document generated when a committee is established to record its appointment.
- **Committee Cessation Document:** A formal document generated when a committee is deactivated to record its cessation.
- **Committee Meeting:** A formal meeting held by a committee to review applications and decide their outcomes.
- **Call for Applications:** An announced opportunity through which an institution invites applicants to seek a defined benefit or outcome.
- **Application:** A formal submission by an applicant to a call for applications.
- **Evaluation:** A structured assessment of an application against the criteria defined by its call for applications.
- **Evaluator:** A qualified person assigned to assess an application and prepare an evaluation report.
- **Evaluation Report:** A written assessment of an application prepared by a committee member or evaluator.
- **Final Evaluation Report:** The committee's consolidated assessment and recommendation for an application.
- **Resolution:** The formal decision document issued for an application based on its final evaluation report.
- **Institutional Insight:** An aggregated observation that helps an institution understand its research activity or evaluation outcomes.

## Business Processes

- **Evaluation Workflow:** The ordered process used to review eligibility, assign applications to committees, assign evaluators, conduct, review, and complete an evaluation.
- **Eligibility Review:** The pre-evaluation review that verifies whether an application meets the requirements of its call for applications and excludes applications that do not.
- **Committee Assignment:** The process of assigning an application to the evaluation committee responsible for its review.
- **Evaluator Assignment:** The process of assigning evaluators to eligible applications.
- **Committee Meeting:** The committee reviews applications and decides their outcomes, producing a final evaluation report and a resolution document for each application.
- **Appeal Procedure:** The formal process for submitting, reviewing, and resolving an appeal or allegation against a resolution.

## Business Rules

- Each committee must include a chair and a secretary, may include members and other roles such as external advisors, and is organized by a knowledge area such as Sciences, Humanities, or Social Sciences.
- A committee's chair and secretary digitally sign its resolutions.
- Establishing or deactivating a committee generates its appointment or cessation document, respectively.
- A call for applications includes a defined application period during which applications may be submitted.
- Each excluded application results in a digitally signed resolution delivered to the applicant, which may be appealed.
- Evaluator assignment ideally assigns three evaluators to each applicant. Evaluators are not required to belong to the assigned committee and may be drawn from other committees with closely related knowledge areas.
- Before evaluations begin, the assigned committee's chair reviews and confirms evaluator assignments.
- An unfavorable evaluation report issued by an evaluator must include its justification. When an evaluator submits or finalizes a report, they receive a copy by email.
- When preparing a final evaluation report, the committee can review each evaluator's assessment to determine the final assessment.
- For an unsuccessful application, the final evaluation report is issued, digitally signed, and delivered to the applicant.
- A resolution is digitally signed by the issuing committee's chair and secretary and delivered to the applicant.


## React Frontend

## .NET Backend

### Domain

#### `src/ResearchAtlas.Domain`

Contains the core business model, including entities, value objects, domain services, and business invariants. This project must remain independent of application delivery, persistence, and other external concerns.

#### `src/ResearchAtlas.Domain.Abstractions`

Defines domain-level contracts that must be shared outside the domain project without exposing infrastructure implementations. Keep these contracts focused on domain concepts and avoid framework-specific types.

### Application

#### `src/ResearchAtlas.Application`

Implements use cases and orchestrates domain behavior. Application services coordinate repositories, external contracts, and transactions, but must not contain infrastructure-specific implementations or duplicate domain invariants.

#### `src/ResearchAtlas.Application.Abstractions`

Defines contracts, commands, queries, DTOs, and interfaces consumed by hosts or infrastructure implementations. Use this project as the stable boundary for application capabilities.

#### `src/ResearchAtlas.Configuration`

Contains strongly typed configuration models and options definitions shared by hosts and service-registration code. Do not place secrets or environment-specific configuration values here.

#### `src/ResearchAtlas.DependencyInjection`

Centralizes dependency-injection registration for application and infrastructure services using Autofac. Hosts should reference this project rather than duplicating service registration in their `Program.cs` files.

#### `src/ResearchAtlas.Extensions`

Provides small, reusable extension methods and helpers with no specific host responsibility. Keep additions cohesive and avoid turning this project into a dependency for unrelated business logic.


### Infrastructure


#### `src/ResearchAtlas.Infrastructure.Persistence.SqlServer`

Implements SQL Server persistence contracts, including data access, repository implementations, and database-specific mapping. Database details must stay in this project and never leak into domain code.

#### `src/ResearchAtlas.Infrastructure.Cache.Memory`

Provides the in-memory implementation of cache persistence contracts. Use it for local development, tests, or deployments that do not require distributed caching.

#### `src/ResearchAtlas.Infrastructure.Cache.Redis`

Provides the Redis-backed implementation of cache persistence contracts. Keep Redis client and serialization details contained within this project.

#### `src/ResearchAtlas.Infrastructure.Storage.AzureBlob`

Implements file and object storage using Azure Blob Storage. Azure SDK types and storage-specific behavior belong exclusively in this project.

#### `src/ResearchAtlas.Infrastructure.Storage.FileSystem`

Implements file and object storage on the local file system. Use it where local storage is appropriate without changing application or domain contracts.

#### `src/ResearchAtlas.Infrastructure.Identity.BuiltIn`

Implements the built-in identity provider. Keep identity-provider-specific authentication and user-management behavior isolated from application and domain code.

#### `src/ResearchAtlas.Infrastructure.Identity.EntraId`

Implements identity integration with Microsoft Entra ID. Keep Entra ID SDK types, configuration, and protocol details contained in this project.

#### `src/ResearchAtlas.Infrastructure.Identity.OpenIdConnect`

Implements identity integration through OpenID Connect. Keep protocol-specific authentication and token-handling details contained in this project.

#### `src/ResearchAtlas.Infrastructure.Identity.Saml`

Implements identity integration through SAML. Keep SAML protocol, assertion, and provider-specific behavior contained in this project.

#### `src/ResearchAtlas.Infrastructure.Remote.Http`

Implements outbound HTTP integrations for remote services. Encapsulate HTTP client configuration, request handling, and remote DTO mapping here.

#### `src/ResearchAtlas.Infrastructure.Remote.Sftp`

Implements outbound SFTP integrations for remote file transfers. Keep SFTP client configuration, connection handling, and file transfer logic contained in this project.


### Database Tooling

#### `src/ResearchAtlas.Database.Migrations.SqlServer`

Contains the versioned SQL Server schema migrations and migration definitions. Add a new migration for every schema change; never alter migrations that may already have been applied.

### Hosts

#### `src/ResearchAtlas.Api`

ASP.NET Core Web API host that exposes HTTP endpoints and OpenAPI documentation. It uses Serilog for host logging and composes backend services; keep `Program.cs` limited to host configuration and composition.

#### `src/ResearchAtlas.Worker`

Executable host for scheduled, queued, and long-running background work. It uses Serilog for host logging. Background job orchestration belongs here; reusable application behavior belongs in the application layer.

### Architecture Rules

- The .NET backend follows Clean Architecture and Domain-Driven Design (DDD): model the business domain independently from application delivery and external infrastructure.
- Keep dependency flow inward: infrastructure and hosts may depend on application and domain layers; domain layers must not depend on infrastructure or hosts.
- Put business invariants in `ResearchAtlas.Domain`; put workflow orchestration in `ResearchAtlas.Application`.
- Define interfaces and cross-layer contracts in the corresponding `*.Abstractions` project; implement external concerns in an `Infrastructure.*` project.
- Keep `Program.cs` files focused on host configuration and composition. Register services through the configuration and dependency-injection projects when applicable.
- Do not access SQL Server, caches, files, blob storage, or HTTP services directly from domain code.

### Development Commands

### Configuration And Data

### Verification