using System.Net;
using System.Net.Http.Headers;
using AutoFixture;
using BehaviouralTests.TestHelpers;
using Core.Enums;
using Core.ValueObjects;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Infrastructure.Entities;
using Presentation.WebApi.Common.Models.Responses;
using Presentation.WebApi.Emails.Endpoints;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using Xunit;
using IMapper = AutoMapper.IMapper;

namespace BehaviouralTests.Tests.Endpoints.EmailTests;

[Collection("Sequential")]
public class GetEmailsSearchableFieldsTests : TestBase<TestFixture>
{
    private readonly IFixture _fixture = AutoFixtureHelper.Create();
    private readonly IMapper _mapper = AutoMapperHelper.Create();
    private readonly IServiceProvider _serviceProvider;
    private readonly TestFixture _testFixture;

    public GetEmailsSearchableFieldsTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
        _serviceProvider = testFixture.Services;
        testFixture.Client.DefaultRequestHeaders.Clear();
    }

    [Fact]
    public async Task GetEmailsSearchableFields_NotLoggedIn_ReturnsUnauthorized()
    {
        // Act
        var (httpResponseMessage, _) = await _testFixture.Client.GETAsync<GetEmailsSearchableFields, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetEmailsSearchableFields_ExpiredAccessToken_ReturnsUnauthorized()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture);
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, -10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.GETAsync<GetEmailsSearchableFields, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetEmailsSearchableFields_UserNoLongerExists_ReturnsUnauthorized()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            IsDeleted = new IsDeleted(true)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.GETAsync<GetEmailsSearchableFields, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetEmailsSearchableFields_UserNotAdmin_ReturnsForbidden()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.User,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, _) = await _testFixture.Client.GETAsync<GetEmailsSearchableFields, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetEmailsSearchableFields_GivenValidRequest_ReturnsOk()
    {
        // Arrange
        var user = FakeUser.CreateValid(_fixture) with
        {
            UserRole = UserRole.Admin,
            IsDeleted = new IsDeleted(false)
        };
        var userEntity = _mapper.Map<UserEntity>(user);
        var accessToken = AuthenticationHelper.CreateAccessToken(user, 10);
        await DatabaseSeeder.InsertUser(_serviceProvider, userEntity);

        // Act
        _testFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var (httpResponseMessage, queryFieldsResponse) = await _testFixture.Client.GETAsync<GetEmailsSearchableFields, QueryFieldsResponseDto>();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        queryFieldsResponse.Fields.Should().NotBeEmpty();
    }

    protected override async Task SetupAsync()
    {
       await _testFixture.ResetDatabaseAsync();
    }

    protected override async Task TearDownAsync()
    {
        await _testFixture.ResetDatabaseAsync();
    }
}