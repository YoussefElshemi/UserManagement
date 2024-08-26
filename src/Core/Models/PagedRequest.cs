using Core.Enums;
using Core.ValueObjects;

namespace Core.Models;

public record PagedRequest
{
    public required PageNumber PageNumber { get; init; }
    public required PageSize PageSize { get; init; }
    public required SortBy? SortBy { get; init; }
    public required SortOrder? SortOrder { get; init; }
    public required SearchField? SearchField { get; init; }
    public required SearchTerm? SearchTerm { get; init; }
}