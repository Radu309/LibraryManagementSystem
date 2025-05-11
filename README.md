Library Management System

Description:
This is a web application built with ASP.NET Core MVC (.NET 8) for managing a library's book collection. 
It supports secure user authentication and role-based access control using ASP.NET Core Identity. 
The application allows administrators to perform all standard operations like adding, editing, deleting, and searching 
books, as well as managing borrowing and returning actions.

Main Features (Tasks):
- Add, edit, and delete books
- Search books by title or author
- Borrow books with stock availability checks
- Prevent borrowing if quantity is zero
- Prevent invalid returns
- New Feature: Authentication and role-based access using ASP.NET Core Identity

Technologies:
- ASP.NET Core MVC (NET 8)
- Entity Framework Core
- PostgreSQL 
- ASP.NET Core Identity
- Razor Views (UI)
- C# 12

Steps to Run the Application:

1. Clone the Repository
   git clone https://github.com/Radu309/LibraryManagementSystem.git
   cd LibraryManagementSystem

2. Restore Dependencies
   dotnet restore

3. Configure PostgreSQL Connection
   Open appsettings.json and replace the connection string with your PostgreSQL database details. Example:
   "ConnectionStrings": {
   "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDB;Username=youruser;Password=yourpassword"
   }

4. Apply Database Migrations
   Make sure EF CLI is installed using: dotnet tool install --global dotnet-ef
   Then run:
   dotnet ef database update

5. Run the Application
   dotnet run


Authentication and Security:
The app uses ASP.NET Core Identity for user registration, login, and secure access. 
Roles can be assigned and book management pages are protected based on user roles.

Database Configuration:
This version uses PostgreSQL. Ensure your PostgreSQL server is running and that the connection string in 
appsettings.json matches your environment.

NuGet Packages Used:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.Identity.UI
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Npgsql.EntityFrameworkCore.PostgreSQL


Screenshots:
![Admin side interface: ](images/admin_side.png)
![User side interface: ](images/user_side.png)


