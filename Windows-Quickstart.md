# Windows Server Deployment Quickstart

This checklist summarizes the deployment process for AppName on Windows Server.
For full details, see the main deployment guide.

---

## 1. Install SQL Server

- Download and install SQL Server and a management tool (e.g., SSMS).
- [SQL Server Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## 2. Create Database and User

- Create a database (e.g., `AppName`).
- Create a SQL login/user for the app and assign permissions.

## 3. Install IIS & .NET 9 Hosting Bundle

- Install IIS via Server Manager or PowerShell.
- Download and install the .NET 9 Hosting Bundle.
  - [.NET 9 Hosting Bundle](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Restart IIS after installation.

## 4. Prepare IIS

- Create an Application Pool (No Managed Code).
- Create a website folder (e.g., `C:\inetpub\wwwroot\AppName`).
- Set folder permissions for the App Pool identity.

## 5. Configure Application

- Update `appsettings.json` with your SQL connection string.
- (Optional) Update `Program.cs` for forwarded headers if needed.

## 6. Build & Publish

- Build the project in Release mode using `dotnet publish`.
- Output to a publish folder (e.g., `Releases/Publish/AppName`).

## 7. Deploy to IIS

- Copy published files to the IIS website folder.
- Create a new Website in IIS Manager, set Application Pool and folder.
- Configure bindings (IP, port, etc.).

## 8. Configure web.config

- Ensure `web.config` exists in the website folder. Use the template from the
  main guide if needed.

## 9. Test Deployment

- Find your server IP address.
- Open a browser and navigate to `http://YOUR_SERVER_IP`.

---
