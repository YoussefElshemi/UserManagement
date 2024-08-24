namespace Presentation.WebApi.Models.User;

public record UserProfileResponseDto
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required string EmailAddress { get; init; }
    public required string UserRole { get; init; }
}