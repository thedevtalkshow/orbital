---
description: Guidance for working with the metadata system in the Orbital application
applyTo: 
  - 'src/orbital.core/Metadata/**'
  - 'src/orbital.core/Models/**Definition.cs'
  - 'src/orbital.core/Data/IMetadataRepository.cs'
  - 'src/orbital.api/MetadataEndpoints.cs'
  - 'tests/orbital.api.tests/MetadataEndpointsTests.cs'
  - 'tests/orbital.core.tests/**'
---

# Metadata System Architecture

## Overview

The Orbital application uses a metadata system to manage various types of reference data (lookups, enumerations, definitions) used throughout the application. The metadata system is designed to be flexible and extensible while maintaining type safety.

## Core Concepts

### Base Classes and Interfaces

- **`IMetadataItem`**: Core interface in `src/orbital.core/Metadata/IMetadataItem.cs`
- **`MetadataDefinition`**: Base class in `src/orbital.core/Metadata/MetadataDefinition.cs` implementing `IMetadataItem`

### Specific Metadata Types

Each specific metadata type (e.g., `EventStatusDefinition`, `AttendanceModeDefinition`) inherits from `MetadataDefinition` and **must set the `Type` property in its constructor**. This ensures consistent type identification across the system.

**Key Pattern**: 
- All metadata classes in `src/orbital.core/Models/` that end with `Definition.cs` follow this pattern
- The `Type` property set in the constructor identifies which Cosmos DB partition the data belongs to
- Example files: `EventStatusDefinition.cs`, `AttendanceModeDefinition.cs`

## Repository Pattern

The metadata repository is defined in `src/orbital.core/Data/IMetadataRepository.cs` and uses generic methods to work with any metadata type.

### Key Points

1. **Generic Type Parameter**: Methods use `<T>` where `T : IMetadataItem` to ensure type safety
2. **MetadataType Parameter**: The `metadataType` string (e.g., "EventStatusType") identifies which Cosmos DB partition to query
3. **Return Types**: Methods return the specific type requested, not just the base `MetadataDefinition`
4. **Available Operations**: GetAll, GetByValue, IsValid, Create, Update, Delete

## Data Storage

Metadata items are stored in Cosmos DB with:
- **Container**: `metadata`
- **Partition Key**: `type` (the metadata type, e.g., "EventStatusType")
- **Document Structure**: Each metadata item is stored as a JSON document with properties matching the `MetadataDefinition` structure

## Common Metadata Types

| Type String | Class Name | Description |
|------------|------------|-------------|
| `EventStatusType` | `EventStatusDefinition` | Status values for events (scheduled, cancelled, etc.) |
| `EventAttendanceModeEnumeration` | `AttendanceModeDefinition` | How events can be attended (online, offline, mixed) |
| `metadata` | `MetadataDefinition` | Generic metadata definitions |

## API Endpoints

Metadata endpoints are defined in `src/orbital.api/MetadataEndpoints.cs`:

- `GET /api/metadata/EventStatusType` - Get event status definitions
- `GET /api/metadata/EventAttendanceModeEnumeration` - Get attendance mode definitions
- `GET /api/metadata/{metadataType}` - Get all metadata items of any type (generic endpoint)
- `POST /api/metadata/admin/` - Create a new metadata item
- `PUT /api/metadata/admin/` - Update an existing metadata item
- `DELETE /api/metadata/admin/{metadataType}/{id}` - Delete a metadata item

## Testing Pattern

Tests are located in `tests/orbital.api.tests/MetadataEndpointsTests.cs`.

### Key Testing Principles

1. **Mock the repository**: Use `Mock<IMetadataRepository>` 
2. **Setup with specific types**: Configure mocks with the exact generic type parameter (e.g., `<EventStatusDefinition>`)
3. **Use helper methods**: Follow the pattern of `InvokeXxxEndpoint()` methods that simulate endpoint behavior
4. **Verify calls**: Always verify the repository method was called with correct parameters

See existing tests in the file for examples of this pattern.

## Adding New Metadata Types

To add a new metadata type:

1. **Create a new class** inheriting from `MetadataDefinition`
2. **Set the Type property** in the constructor to a unique type identifier
3. **Add domain-specific properties** if needed
4. **Create endpoint methods** in `MetadataEndpoints.cs`
5. **Write tests** following the existing pattern
6. **Seed initial data** if required

## Best Practices

1. **Use specific types**: Always use the specific metadata type (e.g., `EventStatusDefinition`) rather than the base `MetadataDefinition` when possible
2. **Type consistency**: The `Type` property should always be set in the constructor and remain constant
3. **Generic methods**: Use the repository's generic methods to maintain type safety
4. **Validation**: Validate that `Type` and `Value` are provided before creating/updating metadata items
5. **Testing**: Test with specific types, not just the base class
