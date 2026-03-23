using System.Net;
using System.Net.Http.Json;
using Cookify.Api.Dtos;

namespace Cookify.Api.Tests.AuthTests;

public class RegisterTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOk()
    {
        // Arrange
        var dto = new RegisterUserDto("testuser", "test@example.com", "Test123!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<MessageResponse>();
        Assert.Equal("Account created successfully", body?.Message);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterUserDto("user1", "duplicate@example.com", "Test123!");
        await _client.PostAsJsonAsync("/api/auth/register", dto);

        var dto2 = new RegisterUserDto("user2", "duplicate@example.com", "Test123!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto2);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithDuplicateUsername_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterUserDto("duplicateuser", "unique1@example.com", "Test123!");
        await _client.PostAsJsonAsync("/api/auth/register", dto);

        var dto2 = new RegisterUserDto("duplicateuser", "unique2@example.com", "Test123!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto2);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithWeakPassword_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterUserDto("weakpwduser", "weakpwd@example.com", "123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorsResponse>();
        Assert.NotNull(body?.Errors);
        Assert.NotEmpty(body.Errors);
    }

    [Fact]
    public async Task Register_WithMissingEmail_ReturnsBadRequest()
    {
        // Arrange
        var payload = new { Username = "noEmailUser", Password = "Test123!" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", payload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var dto = new RegisterUserDto("invalidemail", "not-an-email", "Test123!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithEmptyBody_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private record MessageResponse(string Message);
    private record ErrorsResponse(string[] Errors);
}