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

- **.NET 9**: Latest .NET framework with Aspire
- **Blazor WebAssembly**: Client-side web UI framework
- **Azure CosmosDB**: NoSQL database for meeting data storage
- **ASP.NET Core**: Web API for backend services
- **OpenTelemetry**: For monitoring and diagnostics

## Architecture

Orbital follows a clean architecture approach with a repository pattern for data access:

### Repository Pattern Implementation

The application uses the **Repository Pattern** to abstract data access operations, providing flexibility between different data sources:

- **`IMeetingRepository`** interface defines the contract with three core operations:
  - `ListMeetingsAsync()` - Retrieve all meetings
  - `GetMeetingByIdAsync(string id)` - Get a specific meeting by ID
  - `CreateMeetingAsync(Meeting meeting)` - Create a new meeting

- **`CosmosMeetingRepository`** - Production implementation using Azure Cosmos DB
  - Handles Azure Cosmos DB operations with proper error handling
  - Uses partition key strategy for optimal performance
  - Supports asynchronous operations for scalability

- **`InMemoryMeetingRepository`** - Testing implementation using in-memory collections
  - Provides fast, isolated testing without external dependencies
  - Pre-populated with test data for consistent test scenarios
  - Used automatically in unit tests through dependency injection

### Benefits

- **Testability**: Easy to swap implementations for testing vs production
- **Maintainability**: Clean separation between business logic and data access
- **Flexibility**: Can easily add new data sources (SQL Server, MongoDB, etc.)
- **Performance**: Optimized implementations for different scenarios

## Testing

The application uses a comprehensive testing strategy that leverages the repository pattern:

### Unit Testing with In-Memory Repository

- **Test Infrastructure**: Uses ASP.NET Core's `WebApplicationFactory` for integration testing
- **Repository Substitution**: Test setup automatically replaces `CosmosMeetingRepository` with `InMemoryMeetingRepository`
- **Isolated Testing**: Tests run without external dependencies (no Cosmos DB required)
- **Consistent Test Data**: In-memory repository provides predictable test scenarios

### Test Coverage

- **API Endpoint Testing**: Validates all CRUD operations (GET, POST)
- **Status Code Verification**: Ensures proper HTTP responses
- **Data Validation**: Tests request/response serialization and business logic
- **Integration Testing**: Full request pipeline testing through the web host

The testing approach ensures reliable, fast-running tests while maintaining confidence in the production code path.

## Local Development

To run this project locally:

1. Ensure you have .NET 9 SDK installed with Aspire workload
2. Clone the repository
3. Open the solution in Visual Studio 2022 or later
4. Start the project with the orbital.AppHost as the startup project

The application will start with a local CosmosDB emulator pre-configured for development.

## Project Structure

- **orbital.core**: Core domain models and business logic, including the `IMeetingRepository` interface
- **orbital.api**: Backend API service with dependency injection and repository pattern usage
- **orbital.data**: Data access layer containing the `CosmosMeetingRepository` implementation
- **orbital.web**: Blazor WebAssembly frontend
- **orbital.test**: Integration tests using .NET Aspire testing framework
- **orbital.test.api**: Unit tests with `InMemoryMeetingRepository` for fast, isolated testing
- **orbital.AppHost**: Application host for orchestrating the distributed application
- **orbital.ServiceDefaults**: Common service configurations

## Contributing

Contributions are welcome! This project is actively developed on The Dev Talk Show stream.
