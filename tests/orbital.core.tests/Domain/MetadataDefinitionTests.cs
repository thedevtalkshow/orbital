using FluentAssertions;
using orbital.core.Metadata;
using orbital.test.shared;

namespace orbital.core.tests.Domain;

public class MetadataDefinitionTests
{
    #region Creation and Default Values Tests

    [Fact]
    public void MetadataDefinition_WhenCreated_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var metadata = new MetadataDefinition();

        // Assert
        metadata.Id.Should().BeEmpty();
        metadata.Type.Should().BeEmpty();
        metadata.Value.Should().BeEmpty();
        metadata.DisplayName.Should().BeEmpty();
        metadata.Description.Should().BeEmpty();
        metadata.IsActive.Should().BeTrue();
        metadata.SortOrder.Should().Be(0);
    }

    #endregion

    #region Property Behavior Tests

    [Fact]
    public void MetadataDefinition_WhenIdIsSet_ShouldRetainValue()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var metadata = new MetadataDefinition { Id = id };

        // Assert
        metadata.Id.Should().Be(id);
    }

    [Fact]
    public void MetadataDefinition_WhenTypeIsSet_ShouldRetainValue()
    {
        // Arrange
        var type = "EventStatus";

        // Act
        var metadata = new MetadataDefinition { Type = type };

        // Assert
        metadata.Type.Should().Be(type);
    }

    [Fact]
    public void MetadataDefinition_WhenValueIsSet_ShouldRetainValue()
    {
        // Arrange
        var value = "EventScheduled";

        // Act
        var metadata = new MetadataDefinition { Value = value };

        // Assert
        metadata.Value.Should().Be(value);
    }

    [Fact]
    public void MetadataDefinition_WhenDisplayNameIsSet_ShouldRetainValue()
    {
        // Arrange
        var displayName = "Event Scheduled";

        // Act
        var metadata = new MetadataDefinition { DisplayName = displayName };

        // Assert
        metadata.DisplayName.Should().Be(displayName);
    }

    [Fact]
    public void MetadataDefinition_WhenDescriptionIsSet_ShouldRetainValue()
    {
        // Arrange
        var description = "The event is scheduled to occur";

        // Act
        var metadata = new MetadataDefinition { Description = description };

        // Assert
        metadata.Description.Should().Be(description);
    }

    [Fact]
    public void MetadataDefinition_WhenIsActiveIsSet_ShouldRetainValue()
    {
        // Arrange & Act
        var activeMetadata = new MetadataDefinition { IsActive = true };
        var inactiveMetadata = new MetadataDefinition { IsActive = false };

        // Assert
        activeMetadata.IsActive.Should().BeTrue();
        inactiveMetadata.IsActive.Should().BeFalse();
    }

    [Fact]
    public void MetadataDefinition_WhenSortOrderIsSet_ShouldRetainValue()
    {
        // Arrange
        var sortOrder = 10;

        // Act
        var metadata = new MetadataDefinition { SortOrder = sortOrder };

        // Assert
        metadata.SortOrder.Should().Be(sortOrder);
    }

    #endregion

    #region Builder Pattern Tests

    [Fact]
    public void MetadataDefinition_UsingBuilder_ShouldCreateValidInstance()
    {
        // Arrange & Act
        var metadata = TestDataBuilders.CreateMetadataDefinition()
            .WithType("TestType")
            .WithValue("test-value")
            .WithDisplayName("Test Value")
            .WithDescription("A test metadata value")
            .WithIsActive(true)
            .WithSortOrder(5)
            .Build();

        // Assert
        metadata.Type.Should().Be("TestType");
        metadata.Value.Should().Be("test-value");
        metadata.DisplayName.Should().Be("Test Value");
        metadata.Description.Should().Be("A test metadata value");
        metadata.IsActive.Should().BeTrue();
        metadata.SortOrder.Should().Be(5);
    }

    [Fact]
    public void MetadataDefinition_UsingBuilderWithDefaults_ShouldCreateValidInstance()
    {
        // Arrange & Act
        var metadata = TestDataBuilders.CreateMetadataDefinition().Build();

        // Assert
        metadata.Id.Should().NotBeEmpty();
        metadata.Type.Should().NotBeEmpty();
        metadata.Value.Should().NotBeEmpty();
        metadata.DisplayName.Should().NotBeEmpty();
        metadata.IsActive.Should().BeTrue();
        metadata.SortOrder.Should().Be(0);
    }

    #endregion

    #region IMetadataItem Interface Tests

    [Fact]
    public void MetadataDefinition_ShouldImplementIMetadataItem()
    {
        // Arrange
        var metadata = new MetadataDefinition();

        // Assert
        metadata.Should().BeAssignableTo<IMetadataItem>();
    }

    [Fact]
    public void MetadataDefinition_AsIMetadataItem_ShouldExposeAllProperties()
    {
        // Arrange
        IMetadataItem metadata = new MetadataDefinition
        {
            Id = "test-id",
            Type = "test-type",
            Value = "test-value",
            DisplayName = "Test Display",
            Description = "Test Description",
            IsActive = false,
            SortOrder = 99
        };

        // Assert
        metadata.Id.Should().Be("test-id");
        metadata.Type.Should().Be("test-type");
        metadata.Value.Should().Be("test-value");
        metadata.DisplayName.Should().Be("Test Display");
        metadata.Description.Should().Be("Test Description");
        metadata.IsActive.Should().BeFalse();
        metadata.SortOrder.Should().Be(99);
    }

    #endregion
}
