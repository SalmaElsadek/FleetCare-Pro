# FleetCare Pro - Vehicle Maintenance System

FleetCare Pro is an enterprise fleet and vehicle maintenance management application built with **ASP.NET Core MVC** (.NET 8), Entity Framework Core (Code-First), and ASP.NET Core Identity.

## 🚀 Features Implemented
* **Module 1:** Role-based Authorization (Admin, FleetManager, Driver) with custom user properties (`FullName`, `EmployeeId`).
* **Module 2:** Vehicle Management with `IFormFile` image uploads, GUID naming, and custom `[ValidVIN]` validation attribute.
* **Module 3:** Master-Detail Service Logging with dynamic line items, PDF/Image invoice validation, and atomic transactions (`IDbContextTransaction`).
* **Module 4:** Analytics Dashboards via View Components (`OverdueMaintenance`, `FleetCostSummary`) and reusable Partial Views (`_VehicleCard`).
* **Module 5:** Advanced Infrastructure including Custom Action Filters (`AuditLogAttribute`), Maintenance Mode Middleware, and Global Error Handling (`/Home/Error` & 404/500 pages).

---

## 🔑 Test Credentials (Default Roles)

| Role | Email | Password |
| :--- | :--- | :--- |
| **Admin** | `admin@gmail.com` | `Admin@0123` |
| **Fleet Manager** | `omar@gmail.com` | `Omar@0123` |
| **Driver** | `selsadek@gmail.com` | `12345@Salma` |

---

## 🛠️ Getting Started
1. Clone the repository or open the solution in Visual Studio.
2. Update the connection string in `appsettings.json` to point to your local SQL Server instance.
3. Run `Update-Database` in the Package Manager Console to apply EF Core migrations.
4. Run the project and log in using the credentials above.
