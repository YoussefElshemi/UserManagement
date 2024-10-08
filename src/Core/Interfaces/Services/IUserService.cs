using Core.Models;
using Core.ValueObjects;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<User> CreateAsync(CreateUserRequest createUserRequest);
    Task<User> UpdateAsync(User user);
    Task<PagedResponse<User>> GetAllAsync(PagedRequest pagedRequest);
    Task<int> GetCountAsync(PagedRequest pagedRequest);
    IDictionary<string, string> GetSortableFields();
    IDictionary<string, string> GetSearchableFields();
    IDictionary<string, Dictionary<string, string>> GetMappableValues();
    Task<bool> ExistsAsync(UserId userId);
    Task<bool> ExistsAsync(Username username);
    Task<bool> ExistsAsync(EmailAddress emailAddress);
    Task<User> GetAsync(UserId userId);
    Task<User> GetAsync(Username username);
    Task<User> GetAsync(EmailAddress emailAddress);
    Task DeleteAsync(UserId userId, Username deletedBy);
}