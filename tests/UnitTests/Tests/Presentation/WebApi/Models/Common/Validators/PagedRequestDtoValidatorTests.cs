using Core.Enums;
using Core.Interfaces.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Models.Common.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Common;

namespace UnitTests.Tests.Presentation.WebApi.Models.Common.Validators;

public class PagedRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IPagedRepository<object>> _mockPageRepository = new();
    private readonly PagedRequestDtoValidator<object> _validator;

    public PagedRequestDtoValidatorTests()
    {
        _mockPageRepository
            .Setup(x => x.GetCountAsync())
            .ReturnsAsync(10);

        _mockPageRepository
            .Setup(x => x.GetSortableFields())
            .Returns(["Id"]);

        _validator = new PagedRequestDtoValidator<object>(_mockPageRepository.Object);
    }

    [Fact]
    public async Task ValidateAsync_NegativePageNumber_ReturnsError()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture) with
        {
            PageNumber = -1,
            SortBy = null
        };

        // Act
        var result = await _validator.TestValidateAsync(pagedRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorCode("GreaterThanOrEqualValidator");
    }

    [Fact]
    public async Task ValidateAsync_PageNumberMoreThanPossibleNumberOfPages_ReturnsError()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture) with
        {
            PageSize = 10,
            PageNumber = 2,
            SortBy = null
        };

        // Act
        var result = await _validator.TestValidateAsync(pagedRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorCode("LessThanOrEqualValidator");
    }

    [Fact]
    public async Task ValidateAsync_NegativePageSize_ReturnsError()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture) with
        {
            PageSize = -1,
            SortBy = null
        };

        // Act
        var result = await _validator.TestValidateAsync(pagedRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorCode("GreaterThanOrEqualValidator");
    }

    [Fact]
    public async Task ValidateAsync_PageSizeGreaterThanMaxPageSize_ReturnsError()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture) with
        {
            PageSize = PagedRequestDtoValidator<object>.MaxPageSize + 1,
            SortBy = null
        };

        // Act
        var result = await _validator.TestValidateAsync(pagedRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorCode("LessThanOrEqualValidator");
    }

    [Fact]
    public async Task ValidateAsync_InvalidSortOrder_ReturnsError()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture) with
        {
            PageNumber = 1,
            PageSize = PagedRequestDtoValidator<object>.MaxPageSize,
            SortOrder = "invalid",
            SortBy = null,
        };

        // Act
        var result = await _validator.TestValidateAsync(pagedRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SortOrder)
            .WithErrorMessage(PagedRequestDtoValidator<object>.InvalidSortOrderErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_InvalidSortBy_ReturnsError()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture) with
        {
            PageNumber = 1,
            PageSize = PagedRequestDtoValidator<object>.MaxPageSize,
            SortBy = "invalid",
        };

        // Act
        var result = await _validator.TestValidateAsync(pagedRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SortBy)
            .WithErrorMessage(PagedRequestDtoValidator<object>.InvalidSortByErrorMessage(["Id"]));
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture) with
        {
            PageNumber = 1,
            PageSize = PagedRequestDtoValidator<object>.MaxPageSize,
            SortBy = "Id"
        };

        // Act
        var result = await _validator.TestValidateAsync(pagedRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}