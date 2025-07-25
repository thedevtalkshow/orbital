# Orbital

Orbital is a user group meeting management application developed for The Dev Talk Show stream. This application helps organizers create, manage, and schedule meetings for user groups or communities.

## Project Overview

Orbital is built as a modern, cloud-native application using .NET technologies:

- **Backend API**: REST API for meeting and metadata management with full CRUD operations
- **Frontend**: Blazor WebAssembly client for a responsive user interface
- **Database**: Azure CosmosDB for data storage with separate containers for meetings and metadata
- **Metadata System**: Configurable metadata definitions supporting Schema.org Event specification
- **Architecture**: Built with .NET Aspire for cloud-native distributed application development

## Features

- Create and manage user group meetings
- Track meeting details including title, description, start/end times
- Automatic duration calculation
- **Metadata management system** for event properties
- **Schema.org Event specification** compliance
- Event status tracking (scheduled, cancelled, rescheduled, etc.)
- Event attendance mode support (online, offline, mixed)
- Configurable event metadata through database-driven definitions
- Dynamic form components for metadata selection
- Responsive web interface

## Technology Stack

- **.NET 9**: Latest .NET framework with Aspire
- **Blazor WebAssembly**: Client-side web UI framework
- **Azure CosmosDB**: NoSQL database for meeting and metadata storage
- **ASP.NET Core**: Web API for backend services
- **Schema.org**: Event specification compliance for structured data
- **Repository Pattern**: Clean architecture with data access abstraction
- **OpenTelemetry**: For monitoring and diagnostics

## Metadata System

The application includes a comprehensive metadata management system that supports the Schema.org Event specification:

### Key Features
- **Dynamic Metadata Definitions**: Database-driven metadata types and values
- **Event Status Management**: Support for scheduled, cancelled, rescheduled, and postponed events
- **Attendance Mode Options**: Online, offline, and mixed event attendance modes
- **Extensible Architecture**: Easy addition of new metadata types through the system
- **Client-Side Caching**: Optimized performance with HTTP client-side caching
- **Validation Support**: Built-in validation for metadata values

### Architecture Components
- **IMetadataItem Interface**: Core contract for all metadata definitions
- **MetadataDefinition**: Base class for metadata items with common properties
- **Specialized Definitions**: `EventStatusDefinition` and `AttendanceModeDefinition` for specific metadata types
- **Repository Pattern**: `IMetadataRepository` with CosmosDB implementation
- **HTTP Client Service**: `MetadataHttpClient` for frontend API communication
- **Dynamic UI Components**: Razor components for metadata selection and management

## Local Development

To run this project locally:

1. Ensure you have .NET 9 SDK installed with Aspire workload
2. Clone the repository
3. Open the solution in Visual Studio 2022 or later
4. Start the project with the orbital.AppHost as the startup project

The application will start with a local CosmosDB emulator pre-configured for development with separate containers for meetings and metadata.

## Project Structure

- **orbital.core**: Core domain models and business logic
  - Meeting entity with Schema.org Event specification compliance
  - Metadata system interfaces (`IMetadataItem`, `IMetadataRepository`, `IMetadataService`)
  - Metadata definition models (`MetadataDefinition`, `EventStatusDefinition`, `AttendanceModeDefinition`)
- **orbital.api**: Backend API service
  - RESTful endpoints for meeting and metadata management
  - Integration with Azure CosmosDB through repository pattern
- **orbital.data**: Data access layer
  - CosmosDB repository implementations for meetings and metadata
  - Database-agnostic interface implementations
- **orbital.web**: Blazor WebAssembly frontend
  - Responsive meeting management interface
  - Dynamic metadata selection components
  - HTTP client services for API communication
- **orbital.AppHost**: Application host for orchestrating the distributed application
- **orbital.ServiceDefaults**: Common service configurations
- **orbital.test**: Integration tests for the distributed application
- **orbital.test.api**: Unit tests for API services and HTTP client implementations

## Testing

The project includes comprehensive testing coverage:

- **Unit Tests**: Complete test coverage for metadata HTTP client functionality
- **Integration Tests**: API resource integration testing with in-memory repositories
- **Mock Testing**: Proper mocking of HTTP message handlers for reliable testing
- **Test Fixtures**: Reusable test data and setup for consistent testing scenarios

## Contributing

Contributions are welcome! This project is actively developed on The Dev Talk Show stream.
