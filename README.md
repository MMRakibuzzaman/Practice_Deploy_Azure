# .NET 10 Azure Web API with CI/CD Pipeline

A production-ready ASP.NET Core Web API featuring JWT Authentication, Master-Detail CRUD operations, file uploads, and a fully automated CI/CD deployment pipeline to Microsoft Azure. 

## 🚀 Features

* **Master-Detail CRUD Architecture:** Manages relationships between `Teams` (Master) and `Players` (Detail) using Entity Framework Core.
* **Secure Authentication:** Implements JWT (JSON Web Token) bearer authentication to protect API endpoints.
* **File Uploads:** Handles `multipart/form-data` for uploading and storing image files (e.g., Team Logos) securely on the server.
* **Automated CI/CD Pipeline:** Utilizes GitHub Actions to automatically build the application, generate idempotent SQL scripts, update the cloud database, and deploy to an Azure Web App on every push to the `master` branch.
* **Cloud Infrastructure:** Hosted on Azure App Service with data persistence handled by Azure SQL Database.

## 🛠️ Tech Stack

* **Backend:** C#, .NET 10.0, ASP.NET Core Web API
* **ORM:** Entity Framework Core
* **Database:** LocalDB (Development) / Azure SQL Database (Production)
* **Deployment:** GitHub Actions, Azure Web Apps
* **Authentication:** JWT (JSON Web Tokens)

## 🏗️ Architecture & Deployment

This repository includes a fully configured Continuous Integration and Continuous Deployment (CI/CD) pipeline located at `.github/workflows/deploy.yml`. 

Upon pushing to the `master` branch, the GitHub Actions runner performs the following sequence:
1. Provision an Ubuntu runner and set up the .NET 10 SDK.
2. Restore NuGet dependencies.
3. Install the EF Core CLI tools globally.
4. Generate an idempotent SQL migration script directly from the C# models.
5. Apply the SQL script to the Azure SQL Database to ensure the schema is up-to-date.
6. Build and publish the .NET API in `Release` mode.
7. Deploy the compiled application to the Azure Web App.

## 💻 Local Development Setup

To run this project locally on your machine, follow these steps:

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* Visual Studio, Rider, or VS Code
* SQL Server (or LocalDB)

### 1. Clone the Repository
    git clone https://github.com/yourusername/Practice_Deploy_Azure.git
    cd Practice_Deploy_Azure

### 2. Configure Local Database
Update the `ConnectionStrings` in your `appsettings.Development.json` to point to your local SQL Server instance:

    "ConnectionStrings": {
      "appCon": "Server=localhost;Database=LocalDbName;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True"
    }

### 3. Apply Database Migrations
Open your terminal in the project root and run the following command to generate the tables locally:

    dotnet ef database update --project ./Auth/Auth.csproj

### 4. Run the Application

    dotnet run --project ./Auth/Auth.csproj

The API will launch locally (typically at `http://localhost:5000` or `https://localhost:5001`). You can test the endpoints using Postman or Swagger.

## 📡 API Endpoints 

### Authentication
* `POST /api/auth/register` - Register a new user
* `POST /api/auth/login` - Login and receive a JWT

### Teams (Requires Bearer Token)
* `GET /api/teams` - Retrieve all teams and associated players
* `GET /api/teams/{id}` - Retrieve a specific team by ID
* `POST /api/teams` - Create a new team with players and an image upload (Form-Data)
* `PUT /api/teams/{id}` - Update an existing team
* `DELETE /api/teams/{id}` - Delete a team and its associated players
* 
