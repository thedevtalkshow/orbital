using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using orbital.core.Metadata;
using orbital.core.Data;
using orbital.core.Models;

namespace orbital.core.tests.Metadata
{
    public class MetadataServiceTests
    {
        private readonly Mock<IMetadataRepository> _mockMetadataRepository;
        private readonly MetadataService _metadataService;

        public MetadataServiceTests()
        {
            _mockMetadataRepository = new Mock<IMetadataRepository>();
            _metadataService = new MetadataService(_mockMetadataRepository.Object);
        }

        [Fact]
        public async Task UpdateMetadataValueAsync_WithValidPayload_PersistsChanges()
        {
            // Arrange
            var metadataToUpdate = new EventStatusDefinition
            {
                Id = Guid.NewGuid().ToString(),
                Type = "Meeting",
                Value = "UpdatedValue",
                IsActive = true,
                SortOrder = 1,
            };

            _mockMetadataRepository
                .Setup(x => x.UpdateMetadataItemAsync(metadataToUpdate))
                .ReturnsAsync(true);

            // Act
            var result = await _metadataService.UpdateMetadataAsync(metadataToUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(metadataToUpdate.Id, result.Id);
            _mockMetadataRepository.Verify(x => x.UpdateMetadataItemAsync(metadataToUpdate), Times.Once);
        }

        [Fact]
        public async Task UpdateMetadataValueAsync_WithNonAlphaNumericValue_ThrowsArgumentException()
        {
            // Arrange
            var metadataToUpdate = new EventStatusDefinition
            {
                Id = Guid.NewGuid().ToString(),
                Type = "Meeting",
                Value = "Invalid@Value!",
                IsActive = true,
                SortOrder = 1,
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _metadataService.UpdateMetadataAsync(metadataToUpdate);
            });
        }

        // [Fact]
        // public async Task GetMetadataAsync_WithValidId_ReturnsMetadata()
        // {
        //     // Arrange
        //     var metadataId = Guid.NewGuid().ToString();
        //     var expectedMetadata = new MetadataEntity
        //     {
        //         Id = metadataId,
        //         Type = "Meeting",
        //         Properties = new Dictionary<string, object>
        //         {
        //             { "title", "Test Meeting" },
        //             { "startDate", DateTime.UtcNow }
        //         }
        //     };

        //     _mockMetadataRepository
        //         .Setup(x => x.GetByIdAsync(metadataId))
        //         .ReturnsAsync(expectedMetadata);

        //     // Act
        //     var result = await _metadataService.GetMetadataAsync(metadataId);

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal(expectedMetadata.Id, result.Id);
        //     Assert.Equal(expectedMetadata.Type, result.Type);
        // }

        // [Fact]
        // public async Task GetMetadataAsync_WithInvalidId_ReturnsNull()
        // {
        //     // Arrange
        //     var invalidId = Guid.NewGuid().ToString();
            
        //     _mockMetadataRepository
        //         .Setup(x => x.GetByIdAsync(invalidId))
        //         .ReturnsAsync((MetadataEntity)null);

        //     // Act
        //     var result = await _metadataService.GetMetadataAsync(invalidId);

        //     // Assert
        //     Assert.Null(result);
        // }

        // [Fact]
        // public async Task CreateMetadataAsync_WithValidMetadata_ReturnsCreatedMetadata()
        // {
        //     // Arrange
        //     var metadataToCreate = new MetadataEntity
        //     {
        //         Type = "Meeting",
        //         Properties = new Dictionary<string, object>
        //         {
        //             { "title", "New Meeting" }
        //         }
        //     };

        //     _mockMetadataRepository
        //         .Setup(x => x.CreateAsync(It.IsAny<MetadataEntity>()))
        //         .ReturnsAsync(metadataToCreate);

        //     // Act
        //     var result = await _metadataService.CreateMetadataAsync(metadataToCreate);

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal(metadataToCreate.Type, result.Type);
        //     _mockMetadataRepository.Verify(x => x.CreateAsync(It.IsAny<MetadataEntity>()), Times.Once);
        // }


        // [Fact]
        // public async Task DeleteMetadataAsync_WithValidId_CallsRepositoryDelete()
        // {
        //     // Arrange
        //     var metadataId = Guid.NewGuid().ToString();

        //     _mockMetadataRepository
        //         .Setup(x => x.DeleteAsync(metadataId))
        //         .Returns(Task.CompletedTask);

        //     // Act
        //     await _metadataService.DeleteMetadataAsync(metadataId);

        //     // Assert
        //     _mockMetadataRepository.Verify(x => x.DeleteAsync(metadataId), Times.Once);
        // }
    }
}