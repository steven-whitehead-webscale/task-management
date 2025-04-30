# Simple Horizontal Layered C# Web Application

This is a simple task management web application demonstrating a clean layered architecture in C# with CQRS pattern.

![image](https://github.com/user-attachments/assets/6ba7a4a9-2c0f-4177-b313-19211cff8bdc)


## Project Structure

- **TaskManagement.Web** - Presentation Layer (ASP.NET Core Web App)
- **TaskManagement.Application** - Application Layer (Services)
- **TaskManagement.DataAccessLayer** - Data Access Layer (Command and Query Handlers)
- **TaskManagement.BusinessLogic** - Business Logic, Commands, Queries, and Domain Models
- **TaskManagement.Infrastructure** - Infrastructure concerns (Logging, Caching, etc.)
- **TaskManagement.Common** - Common Models and Utilities

## Architecture

The application follows a clean layered architecture with CQRS pattern:

- **Presentation Layer**: ASP.NET Core MVC controllers and views
- **Application Layer**: Service classes that orchestrate business operations and map between different model types
- **Data Access Layer**: Command and Query handlers for data access
- **Business Logic Layer**: Commands, Queries, and domain models
- **Infrastructure Layer**: Cross-cutting concerns like logging, caching, etc.
- **Common Layer**: Common models and utilities

### CQRS Pattern

The application uses Command Query Responsibility Segregation (CQRS) pattern:

- **Commands**: Represent actions that change the state of the system (CreateTaskCommand, UpdateTaskCommand, etc.)
- **Queries**: Represent requests for data (GetAllTasksQuery, GetTaskByIdQuery)
- **Command Handlers**: Process commands and update the system state
- **Query Handlers**: Process queries and return data

### Model Mapping

The application uses different model types in different layers:

- **Common.Models.Task**: Used in the DataAccessLayer
- **BusinessLogic.Models.Task**: Used in the Application layer

The TaskService in the Application layer is responsible for mapping between these two model types.

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server (LocalDB is fine)

## How to Run

1. Clone the repository
2. Open the solution in Visual Studio or VS Code
3. Update the connection string in `appsettings.json` if needed
4. Run the following commands in the terminal:
   ```bash
   dotnet restore
   dotnet build
   cd TaskManagement.Web
   dotnet run
   ```
5. Open your browser and navigate to `https://localhost:5001`

## Features

- View all tasks
- Add new tasks
- Mark tasks as complete
- Delete tasks

## Architecture

The application follows a clean layered architecture:

- **Presentation Layer**: ASP.NET Core MVC controllers and views
- **Application Layer**: Service classes that orchestrate business operations
- **Data Access Layer**: Repository pattern for data access
- **Business Logic Layer**: Domain models and business rules
- **Infrastructure Layer**: Cross-cutting concerns like logging, caching, etc.
- **Common Layer**: Helper classes and utilities 

## Things Missing
- Simple Injector
- NHibernate
- CommandRunner/our Command pattern - how does our commands and services differ?
- DAL is slightly different in that it doesnt use CommandRunner
- Manager/Utility classes
- database to persist to, which will be good to demonstrate when vertical layering added to this sample
- vertical layering and intercommunication between domains
- region seperation