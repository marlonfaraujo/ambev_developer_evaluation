## Use case

See [Use Case](/.doc/use-case.md)

# Table of Contents

- [Project description](#project-description)
- [Process and Decisions](#process-and-decisions)
- [Achievements](#achievements)
- [What can be improved](#what-can-be-improved)
- [Getting started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Running the project](#running-the-project)
- [Technologies and Technical Features](#technologies-and-technical-features)
  - [Technical Features](#technical-features)
  - [Technologies Used - Overview](#technologies-used---overview)
- [Documentation](#documentation)
- [Project structure](#project-structure)
- [Conclusion](#conclusion)

# Project description 

Backend API project developed to manage sales records and calculate discounts based on the quantity of identical products sold. This API was created to act as a standalone service or a microservice within a comprehensive e-commerce ecosystem, it can be integrated with other microservices that communicate with payment gateways, inventory control and invoice generation, etc.

# Process and decisions

A sales context was created, and a product cart for simulating prices with the discount rules [See:](/.doc/use-case.md)). Therefore, it is not possible to make sales without a registered cart.

The cart is just data saved in cache using the redis database in memory. The cart APIs only interact with the application and domain layer, for the purpose of using the domain service that simulates prices with discounts.

# Achievements

I removed the dependency on the MediatR framework from the application layer. MediatR is used to implement the CQRS pattern, centralizing the sending of commands and queries, promoting decoupling between application layers and facilitating maintenance and testing.

Additionally, when listing data from all APIs, I opted for paginated SQL queries to avoid retrieving all records through the repository layer and loading them entirely into memory.

# What can be improved

With more time, my goal for the project would be to remove the Fluent framework from the domain layer, so as not to run the risk of having to change it for another one in the future. The impact would be great on this important layer. I believe that with the inversion of dependencies and the use of adapter patterns, this case would be solved.

In addition, the next step would be to write consolidated and denormalized data to a NoSQL database (MongoDB), to facilitate queries and use by some other microservice.

# Getting started

## Prerequisites

Make sure you have docker installed.
 
To run the project, you must have the following installed on your local environment:

* **Git** to clone repository ([link](https://git-scm.com/downloads))
* **Docker** should be installed ([link](https://docs.docker.com/engine/install/))
* **docker-compose** should be installed, if your docker installation does not install it automatically ([link](https://docs.docker.com/compose/install/))
* **.NET-SDK-8** For running database migrations and local development ([link](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))

## Running the project

In order to run the application you need to follow the steps below:
* Run: 
```bash
 git clone https://github.com/marlonfaraujo/ambev_developerevaluation_backend.git
```
* Access the folder, example: 
```bash
cd ambev_developerevaluation_backend
```

* Run docker compose: 

```bash
docker-compose up -d
``` 

* Run migrations: 

```bash
cd ambev_developerevaluation_backend

``` 

```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj --context Ambev.DeveloperEvaluation.ORM.DefaultContext
``` 

# Technologies and Technical Features 


## Technical Features 

* **Clean Code**
* **DDD**
* **SOLID**
* **DRY**
* **Clean Code**
* **CQRS**
* **JWT**
* **TDD, Unit and integration tests**
* **Git Flow**

## Technologies Used - Overview
Here are listed some of the specific technologies used for the implementation of the project:
* **Authentication**: [JSON Web Tokens - JWT](https://jwt.io/)
* **Databases - Persistence**: [Redis](https://redis.io/), [PostgeSQL](https://www.postgresql.org/), [EntityFrameworkCore](https://learn.microsoft.com/en-us/ef/core/)
* **Testing**: [XUnit](https://xunit.net/), [Bogus](https://github.com/bchavez/Bogus), [Moq](https://github.com/devlooped/moq)
* **External Communication Protocols**: [REST](https://en.wikipedia.org/wiki/Representational_state_transfer)
* **Frameworks**: [AutoMapper](https://automapper.org/), [FluentValidation](https://docs.fluentvalidation.net/en/latest/), [MediatR](https://www.nuget.org/packages/mediatr/), [Newtonsoft.Json](https://www.newtonsoft.com/json)
* **Container Technology**: [Docker](https://www.docker.com/)
* **Tools**: Github Copilot


# Documentation
Access the documentation and make requests to the API at https://localhost:8081/swagger/index.html


# Project structure
```
  $ tree
  .
  ├── docker-compose.yml
  ├── Dockerfile
  ├── launchSettings.json
  ├── README.md
  └──tests
  │  ├── Functional
  │  ├── Integration
  │  └── Unit
  │  
  └── src
       ├── Application
       │     ├── Auth
       │     ├── Branchs
       │     ├── Exceptions
       │     ├── Products
       │     ├── Requests
       │     ├── Sales
       │     └── Users  
       ├── Common
       ├── Domain
       │     ├── Common
       │     ├── Entities
       │     ├── Enums
       │     ├── Events
       │     ├── Exceptions
       │     ├── Factories
       │     ├── Repositories
       │     ├── Services
       │     ├── Specifications
       │     ├── Validation
       │     └── ValueObjects  
       ├── IoC
       ├── ORM
       │     ├── Common
       │     ├── Dtos
       │     ├── Mapping
       │     ├── Migrations
       │     ├── Queries
       │     ├── Repositories
       │     └── Services  
       └── WebApi
             ├── Adapters
             ├── Common
             ├── Features
             ├── Mappings
             ├── Middleware
             └── Notifications  
      

```

# Conclusion

My goal was to generate value and pay special attention to avoiding layer corruption in the project. In addition to complying with the business rules of the use case, I value clean, testable and reusable code.
