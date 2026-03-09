using System.Net;
using System.Net.Http.Json;
using Cookify.Api.Dtos;

namespace Cookify.Api.Tests.AuthTests;

public class LoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public LoginTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task RegisterUserAsync(string username, string email, string password)
    {
        var dto = new RegisterUserDto(username, email, password);
        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Login_WithValidEmail_ReturnsOkAndToken()
    {
        // Arrange
        await RegisterUserAsync("loginuser1", "loginuser1@example.com", "Test123!");
        var loginDto = new LoginUserDto("loginuser1@example.com", "Test123!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(body?.AccessToken);
        Assert.NotEmpty(body.AccessToken);
    }

    [Fact]
    public async Task Login_WithValidUsername_ReturnsOkAndToken()
    {
        // Arrange
        await RegisterUserAsync("loginuser2", "loginuser2@example.com", "Test123!");
        var loginDto = new LoginUserDto("loginuser2", "Test123!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(body?.AccessToken);
        Assert.NotEmpty(body.AccessToken);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        // Arrange
        await RegisterUserAsync("loginuser3", "loginuser3@example.com", "Test123!");
        var loginDto = new LoginUserDto("loginuser3@example.com", "WrongPassword!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<MessageResponse>();
        Assert.Equal("Invalid login or password", body?.Message);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginUserDto("nonexistent@example.com", "Test123!");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<MessageResponse>();
        Assert.Equal("Invalid login or password", body?.Message);
    }

    [Fact]
    public async Task Login_WithEmptyBody_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithMissingPassword_ReturnsBadRequest()
    {
        // Arrange
        var payload = new { UserLogin = "someuser@example.com" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private record TokenResponse(string AccessToken);
    private record MessageResponse(string Message);
}
