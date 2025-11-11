# orbital.test.api

This project contains unit tests for HTTP client services in the Orbital application.

## Purpose

Unit tests for HTTP-based service clients, using mocking to test client behavior without actual HTTP calls.

## Current Tests

### MetadataHttpClientTests
Unit tests for the `MetadataHttpClient` class which handles metadata API calls from the Blazor frontend.

Tests include:
- HTTP request/response handling
- Caching behavior
- Error handling
- API endpoint construction

Uses Moq to mock `HttpMessageHandler` for isolated testing without actual network calls.

## Note

Integration tests that exercise full API endpoints have been moved to `tests/orbital.integration.tests`.
This project focuses on unit testing HTTP client implementations.
