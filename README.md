# Library Management System (ASP.NET Core Web API)

## Features
- JWT-based authentication (register + login)
- Protected Books CRUD endpoints
- EF Core + MSSQL persistence
- Seeded sample book data on startup
- Swagger/OpenAPI UI
- Bonus: search + pagination on GET /api/books

## Prerequisites
- .NET SDK 8.x (or 7.x)
- MSSQL Server (LocalDB or SQL Server)
- EF Core Tools (installed via NuGet packages)

## Setup
1) Clone repo
2) Configure DB connection in `src/LibraryManagementSystem/appsettings.json`:
   - `ConnectionStrings:DefaultConnection`

3) Configure JWT key in `appsettings.json`:
   - Set `Jwt:Key` to a long random secret (32+ chars recommended)

## Run migrations
From `src/LibraryManagementSystem`:
``` Package Manager Console
 --- Package Manager Console
Add-Migrations InitialCreate
Update-Database
``` Bash
dotnet ef migrations add InitialCreate
dotnet ef database update
