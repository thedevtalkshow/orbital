# Orbital

Orbital is a user group meeting management application developed for The Dev Talk Show stream. This application helps organizers create, manage, and schedule meetings for user groups or communities.

## Project Overview

Orbital is built as a modern, cloud-native application using .NET technologies:

- **Backend API**: REST API for meeting management with CRUD operations
- **Frontend**: Blazor WebAssembly client for a responsive user interface
- **Database**: Azure CosmosDB for data storage
- **Architecture**: Built with .NET Aspire for cloud-native distributed application development

## Features

- Create and manage user group meetings
- Track meeting details including title, description, start/end times
- Automatic duration calculation
- Responsive web interface

## Technology Stack

- **.NET 8**: Latest .NET framework with Aspire
- **Blazor WebAssembly**: Client-side web UI framework
- **Azure CosmosDB**: NoSQL database for meeting data storage
- **ASP.NET Core**: Web API for backend services
- **OpenTelemetry**: For monitoring and diagnostics

## Local Development

To run this project locally:

1. Ensure you have .NET 8 SDK installed with Aspire workload
2. Clone the repository
3. Open the solution in Visual Studio 2022 or later
4. Start the project with the orbital.AppHost as the startup project

The application will start with a local CosmosDB emulator pre-configured for development.

## Project Structure

- **orbital.core**: Core domain models and business logic
- **orbital.api**: Backend API service
- **orbital.web**: Blazor WebAssembly frontend
- **orbital.AppHost**: Application host for orchestrating the distributed application
- **orbital.ServiceDefaults**: Common service configurations

## Contributing

Contributions are welcome! This project is actively developed on The Dev Talk Show stream.
