# Ambev Developer Evaluation Project

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-14+-blue.svg)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

A comprehensive .NET 8.0 application demonstrating modern software architecture patterns and best practices. This project showcases Domain-Driven Design (DDD), CQRS/Mediator pattern, Entity Framework Core with PostgreSQL, and comprehensive unit testing.

## 🚀 Features

- **Domain-Driven Design** with clear separation of concerns
- **CQRS Pattern** using MediatR for request/response handling
- **Entity Framework Core** with PostgreSQL database
- **JWT Authentication** with role-based authorization
- **Comprehensive Unit Testing** with xUnit and NSubstitute
- **API Documentation** with Swagger/OpenAPI
- **Structured Logging** with Serilog
- **Health Checks** for monitoring

## 📋 Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [Tech Stack](#tech-stack)
- [API Documentation](#api-documentation)
- [Business Rules](#business-rules)
- [Contributing](#contributing)

## 🛠 Prerequisites

Ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)
- IDE: [Visual Studio 2022](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/ambev-developer-evaluation.git
cd ambev-developer-evaluation
```

### 2. Switch to Development Branch

```bash
git checkout develop
```

### 3. Build the Solution

```bash
cd src
dotnet build
```

## ⚙️ Configuration

### Application Settings

Update `src/Ambev.DeveloperEvaluation.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AmbevDevEval;Username=<YOUR_DB_USER>;Password=<YOUR_DB_PASSWORD>"
  },
  "Jwt": {
    "Key": "YourStrongJwtSigningKeyHere",
    "Issuer": "Ambev.Dev.Eval",
    "Audience": "Ambev.Dev.Eval"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" }
    ]
  }
}
```

**Important Configuration Notes:**
- Replace `<YOUR_DB_USER>` and `<YOUR_DB_PASSWORD>` with your PostgreSQL credentials
- Use a secure JWT key (minimum 32 characters)
- Adjust logging levels as needed

## 🗄️ Database Setup

### 1. Create PostgreSQL Database

```bash
# Connect to PostgreSQL
psql -U postgres

# Create database
CREATE DATABASE "AmbevDevEval";
\q
```

### 2. Apply Entity Framework Migrations

```bash
cd src/Ambev.DeveloperEvaluation.ORM
dotnet ef database update --context DefaultContext --startup-project ../Ambev.DeveloperEvaluation.WebApi
```

### 3. Create New Migration (if needed)

```bash
dotnet ef migrations add <MigrationName> \
  --project ../Ambev.DeveloperEvaluation.ORM \
  --startup-project . \
  --context DefaultContext
```

## 🏃‍♂️ Running the Application

### Start the Web API

```bash
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

The application will be available at:
- **HTTPS**: `https://localhost:7181`
- **Swagger UI**: `https://localhost:7181/swagger`

### Authentication in Swagger

1. Click the lock icon in Swagger UI
2. Enter: `Bearer <Your_JWT_Token>`
3. Obtain JWT token via `/api/auth/login` endpoint

## 🧪 Testing

### Run Unit Tests

```bash
cd tests/Ambev.DeveloperEvaluation.Unit
dotnet test
```

**Testing Framework:**
- **xUnit** for test framework
- **FluentAssertions** for readable assertions
- **NSubstitute** for mocking

## 📁 Project Structure

```
root/
├── src/
│   ├── Ambev.DeveloperEvaluation.Application/   # CQRS handlers, DTOs, Profiles
│   ├── Ambev.DeveloperEvaluation.Common/        # Shared utilities
│   ├── Ambev.DeveloperEvaluation.Domain/        # Domain entities and logic
│   ├── Ambev.DeveloperEvaluation.ORM/           # EF Core infrastructure
│   ├── Ambev.DeveloperEvaluation.IoC/           # Dependency injection
│   └── Ambev.DeveloperEvaluation.WebApi/        # API controllers and startup
└── tests/
    └── Ambev.DeveloperEvaluation.Unit/          # Unit tests
```

### Layer Responsibilities

| Layer | Purpose |
|-------|---------|
| **Application** | CQRS Commands/Queries, AutoMapper profiles, FluentValidation |
| **Domain** | Business entities, value objects, domain logic, repository interfaces |
| **ORM** | EF Core context, repository implementations, database migrations |
| **IoC** | Dependency injection configuration and module registration |
| **WebApi** | HTTP API, controllers, middleware, authentication |

## 🛠 Tech Stack

### Core Technologies
- **.NET 8.0** - Latest framework with C# 12 features
- **PostgreSQL** - Robust relational database
- **Entity Framework Core 8.0** - ORM with Npgsql provider

### Architecture Patterns
- **Domain-Driven Design (DDD)** - Domain-centric architecture
- **CQRS** - Command Query Responsibility Segregation
- **Mediator Pattern** - Decoupled request handling
- **Repository Pattern** - Data access abstraction

### Libraries & Tools
- **MediatR** - In-process messaging
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Serilog** - Structured logging
- **JWT Bearer** - Authentication & authorization
- **Swagger/OpenAPI** - API documentation

## 📚 API Documentation

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/login` | Authenticate user and receive JWT token |

### Users Management

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/users` | List all users (paginated) | Required |
| POST | `/api/users` | Create new user | Required |
| GET | `/api/users/{id}` | Get user by ID | Required |
| PUT | `/api/users/{id}` | Update user | Required |
| DELETE | `/api/users/{id}` | Delete user | Required |

### Products Catalog

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/products` | List products (paginated) | Required |
| GET | `/api/products/{id}` | Get product details | Required |
| GET | `/api/products/categories` | Get product categories | Required |
| GET | `/api/products/category/{category}` | Products by category | Required |
| POST | `/api/products` | Create product | Required |
| PUT | `/api/products/{id}` | Update product | Required |
| DELETE | `/api/products/{id}` | Delete product | Required |

### Sales/Cart Management

| Method | Endpoint | Description | Authorization |
|--------|----------|-------------|---------------|
| GET | `/api/carts` | List all sales/carts | Manager, Admin |
| GET | `/api/carts/{id}` | Get sale/cart details | Manager, Admin |
| POST | `/api/carts` | Create new sale/cart | Manager, Admin |
| PUT | `/api/carts/{id}` | Update sale/cart | Manager, Admin |
| DELETE | `/api/carts/{id}` | Cancel sale/cart | Manager, Admin |

### Query Parameters

| Parameter | Description | Default |
|-----------|-------------|---------|
| `_page` | Page number | 1 |
| `_size` | Page size | 10 |
| `_order` | Sort order (e.g., `price desc,title asc`) | - |

### Filtering Examples

```
GET /api/products?category=Electronics&_minPrice=100
GET /api/products?title=*Phone*&_page=2&_size=5
GET /api/users?_order=username asc&_page=1&_size=20
```

## 📋 Business Rules

### Discount Tiers (Applied per line item)

| Quantity Range | Discount Rate |
|----------------|---------------|
| 1-3 items | 0% |
| 4-9 items | 10% |
| 10-20 items | 20% |
| >20 items | ❌ Not allowed (throws exception) |

### Sales/Cart Validation

- **Cart Number**: Cannot be empty
- **Date**: Cannot be more than 5 minutes in the future
- **Customer**: Must exist and have `Customer` role
- **Items**: Quantity must be between 1-20, unit price > 0
- **Total Calculation**: `Sum(Quantity × UnitPrice × (1 - DiscountRate))`

### Cart Operations

- **Create**: Validates all business rules and applies discounts
- **Update**: Replaces all existing items with new ones
- **Cancel**: Soft delete (sets `IsCancelled = true`)

## 🤝 Contributing

We follow the **GitFlow** workflow for development:

### Branch Strategy

| Branch | Purpose |
|--------|---------|
| `main` | Production-ready code |
| `develop` | Integration branch |
| `feature/*` | New features (e.g., `feature/sales-crud`) |

### Development Workflow

1. **Create Feature Branch**
   ```bash
   git checkout develop
   git checkout -b feature/<short-description>
   ```

2. **Commit Changes**
   ```bash
   git add .
   git commit -m "feat: add cart cancel logic"
   ```

3. **Push and Create PR**
   ```bash
   git push origin feature/<short-description>
   ```

4. **Open Pull Request** against `develop` branch

### Code Quality Standards

- ✅ Follow C# naming conventions (PascalCase for public, camelCase for local)
- ✅ Write unit tests for all new handlers/logic
- ✅ Ensure all tests pass before merging
- ✅ Use semantic commit messages (`feat:`, `fix:`, `docs:`, etc.)
- ✅ Keep methods focused and apply Single Responsibility Principle

### Pre-merge Checklist

- [ ] All unit tests pass
- [ ] EF Core migrations are up to date
- [ ] Swagger UI reflects new endpoints
- [ ] Code follows project conventions
- [ ] PR has been reviewed and approved

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. Check the [existing issues](https://github.com/your-username/ambev-developer-evaluation/issues)
2. Create a new issue with detailed description
3. Include steps to reproduce the problem

---

**Happy Coding! 🚀**
