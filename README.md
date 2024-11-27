# Car Rental System API

## Project Overview

The **Car Rental System API** is a RESTful API built using C#, Entity Framework (EF), and follows best practices for creating and managing a fleet of cars, including booking, availability checks, and user management. 

The system supports:
- User registration and authentication via JWT.
- Car rental services.
- Administrative tasks like adding, updating, and deleting cars.

This API provides endpoints for:
- Users to view available cars and book rentals.
- Admins to manage the fleet.

---

## Technologies Used

The project leverages the following technologies:

- **C#**
- **.NET Core / .NET 8+**
- **Entity Framework Core (EF Core)**
- **JWT (JSON Web Token)** for Authentication
- **SQL Server / SQLite** for the database
- **Mailtrap** for email notifications

---

## Prerequisites

Before running the project, ensure you have the following installed or configured:

1. **.NET 8.0 SDK** or later
2. **SQL Server** or **SQLite** for database management
3. **Mailtrap SMTP Credentials** for email notifications

---

# API Endpoints for Car Management

**Base URL**: `/api/Car`

| **HTTP Method** | **Endpoint**                     | **Description**                                                                                   |
|------------------|----------------------------------|---------------------------------------------------------------------------------------------------|
| **GET**          | `/all`                          | Retrieve all cars (available and unavailable).                                                   |
| **GET**          | `/{id}`                         | Retrieve details of a car by ID.                                                                 |
| **GET**          | `/available`                    | Retrieve all available cars.                                                                     |
| **POST**         | `/`                             | Add a new car to the fleet (Admin only).                                                         |
| **POST**         | `/rent/{id}`                    | Rent a car by ID (User only).                                                                    |
| **PUT**          | `/{id}`                         | Update details of a car by ID (Admin only).                                                      |
| **PATCH**        | `/{id}/availability`            | Update the availability status of a car by ID (Admin only).                                      |
| **DELETE**       | `/{id}`                         | Delete a car from the fleet by ID (Admin only).                                                  |
| **GET**          | `/filter`                       | Filter cars based on availability, make, and year.                                               |
| **GET**          | `/sort`                         | Sort cars based on a specified field (`year` or `price`) and order (`asc` or `desc`).            |

---

# API Endpoints for User Management

**Base URL**: `/api/User`

| **HTTP Method** | **Endpoint**         | **Description**                                                                                   |
|------------------|----------------------|---------------------------------------------------------------------------------------------------|
| **GET**          | `/`                 | Retrieve a list of all users (Admin only).                                                       |
| **POST**         | `/register`         | Register a new user.                                                                              |
| **POST**         | `/login`            | Authenticate a user and return a JWT token.                                                      |
| **DELETE**       | `/{id}`             | Delete a user by ID (Admin only).                                                                |

---

# How to Run the API

1. **Clone the repository**  
   Clone this repository to your local machine using Git or by downloading the ZIP file.

2. **Install required dependencies**  
   Ensure you have the necessary dependencies, including the .NET Core SDK.

3. **Set up your database**  
   Configure your database (SQL Server or SQLite) and ensure it is ready for use.

4. **Update `appsettings.json`**  
   Modify the `appsettings.json` file to include your database connection string and SendGrid API key for email notifications.

5. **Run the application**  
   You can run the application using one of the following methods:

   - **Command Line**:  
     Open a terminal, navigate to the project directory, and run:  
     ```bash
     dotnet run
     ```

   - **Microsoft Visual Studio**:  
     - Open the `CarRentalService.sln` file in Microsoft Visual Studio 2022.  
     - At the top center, click on the "https" button to launch the API in your default browser.  

6. **Access the API**  
   The API will be hosted locally on the default port (e.g., `https://localhost:5000`). You can access and test the endpoints from your browser or a tool like Postman.
