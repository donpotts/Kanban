# Kanban

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/donpotts/Kanban/KanbanTasks.yml?logo=github)

## Overview

This repository contains an ASP.NET Core application with a Blazor WebAssembly (WASM) UI that uses the SyncFusion Kanban control in .NET 8. It also includes user authentication using ASP.NET Core 8 Identity, uses Entity Framework Core SQLite as the database, and supports OData for efficient querying.

## Features

- ASP.NET Core Kestrel web server: A robust and high-performance server.
- Blazor WASM UI: A modern web UI framework for .NET.
- SyncFusion Kanban control: A UI control for agile workflows. **Requires a SyncFusion License to use**.
- Swagger UI: An interactive documentation for your API.
- ASP.NET Core 8 Identity: A membership system that adds login functionality to your application.
- Entity Framework Core SQLite: A lightweight database provider for Entity Framework Core.
- OData Support: A standard for building and consuming RESTful APIs.

## Getting Started

### Prerequisites

- Visual Studio 2022
- .NET 8
- ASP.NET Core
- Blazor WASM
- Swagger UI
- ASP.NET Core 8 Identity
- SyncFusion License
- Entity Framework Core SQLite
- OData

### Installation

1. Clone the repo
  git clone https://github.com/donpotts/Kanban.git
2. Install .NET packages
3. Install SyncFusion Kanban, Data, & Themes packages
4. Install any missing packages
5. dotnet restore
   
## API Documentation

You can access the API documentation at your Swagger UI endpoint (using `/swagger` on your application's URL).

Example:  https://kanbantasks.azurewebsites.net/swagger

## Authentication

This application uses ASP.NET Core 8 Identity for user authentication. To log in, navigate to the login page and enter your credentials.

Username:  testUser@example.com

Password:  testUser123!

## OData Support

This application supports OData for efficient querying. You can use OData query options on the API endpoints.

## Contact

Don.Potts@DonPotts.com
