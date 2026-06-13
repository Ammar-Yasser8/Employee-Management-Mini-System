# Employee Management Mini System

## Overview

Employee Management Mini System is a simple ASP.NET Core Web API application developed as part of the AI Makers technical assessment.

The system allows users to manage employees and departments, perform CRUD operations, search employees by name or department, and maintain the relationship between employees and departments.

---

## Project Architecture

The solution is organized into four separate projects following a layered architecture approach to improve maintainability, scalability, and separation of concerns.

### 1. EmployeeManagement.API

The Presentation Layer responsible for:

* Controllers
* API Endpoints
* Dependency Injection Configuration
* Swagger Configuration
* Application Startup

### 2. EmployeeManagement.Application

The Business Logic Layer responsible for:

* Services
* DTOs (Data Transfer Objects)
* Business Rules
* Entity-to-DTO Mapping

### 3. EmployeeManagement.Infrastructure

The Data Access Layer responsible for:

* Entity Framework Core
* DbContext
* Repository Implementations
* Database Configuration
* Migrations

### 4. EmployeeManagement.Domain

The Core Layer responsible for:

* Domain Entities
* Base Entity
* Core Business Models

---

## Design Patterns & Principles

The project applies several software engineering principles and patterns:

### Repository Pattern

Used to abstract database access logic and provide a clean interface for data operations.

Repositories implemented:

* GenericRepository<T>
* EmployeeRepository
* DepartmentRepository

### Service Layer Pattern

Used to separate business logic from controllers.

Services implemented:

* EmployeeService
* DepartmentService

### Dependency Injection

Used throughout the application to achieve loose coupling between components.

### Separation of Concerns

Each layer has a single responsibility and communicates through abstractions.

---
## Error Handling

The application uses a Global Exception Middleware to handle unhandled exceptions in a centralized manner.

### Benefits

* Centralized exception handling
* Consistent API error responses
* Cleaner controllers and services
* Improved maintainability

### Exception Handling Strategy

The application uses built-in exceptions such as:

* KeyNotFoundException → Returns HTTP 404 Not Found

Example response:

```json
{
  "statusCode": 404,
  "message": "Employee not found"
}
```

The Global Exception Middleware converts exceptions into meaningful HTTP responses and JSON error messages.

## Technologies Used

* ASP.NET Core Web API
* C#
* Entity Framework Core
* SQL Server
* Swagger / OpenAPI
* Dependency Injection
* Repository Pattern
* Service Layer Pattern
* Git & GitHub

---

## Database Design

### Employee

* Id
* FullName
* Email
* MobileNumber
* DepartmentId
* JobTitle
* HireDate
* IsActive

### Department

* Id
* Name

### Relationship

* One Department can have many Employees.
* Each Employee belongs to one Department.

---
## Features

### Employee Management

* Create Employee
* Update Employee
* Delete Employee
* Get Employee By Id
* Get All Employees
* Search Employees By Name
* Search Employees By Department

### Department Management

* Create Department
* Update Department
* Delete Department
* Get Department By Id
* Get All Departments

---

## API Endpoints

### Employees

| Method | Endpoint                              | Description        |
| ------ | ------------------------------------- | ------------------ |
| GET    | /api/Employee                         | Get all employees  |
| GET    | /api/Employee/{id}                    | Get employee by id |
| GET    | /api/Employee/search?searchTerm=value | Search employees   |
| POST   | /api/Employee                         | Create employee    |
| PUT    | /api/Employee/{id}                    | Update employee    |
| DELETE | /api/Employee/{id}                    | Delete employee    |

### Departments

| Method | Endpoint             | Description          |
| ------ | -------------------- | -------------------- |
| GET    | /api/Department      | Get all departments  |
| GET    | /api/Department/{id} | Get department by id |
| POST   | /api/Department      | Create department    |
| PUT    | /api/Department/{id} | Update department    |
| DELETE | /api/Department/{id} | Delete department    |

---

## How To Run

### 1. Clone Repository

```bash
git clone https://github.com/Ammar-Yasser8/Employee-Management-Mini-System.git
```

### 2. Open The Solution

Open the solution using Visual Studio .

### 3. Configure Database Connection

Update the connection string inside:

```json
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 4. Apply Database Migration

Using Package Manager Console:

```powershell
Update-Database
```

Or using .NET CLI:

```bash
dotnet ef database update -p EmployeeManagement.Infrastructure -s EmployeeP_Management.API
```

### 5. Run The Application

Press:

```text
F5
```

or:

```bash
dotnet run
```

### 6. Open Swagger

Navigate to:

```text
https://localhost:{port}/swagger
```

and test the available API endpoints.

---

## Author

Ammar Yasser

.NET Backend Developer
