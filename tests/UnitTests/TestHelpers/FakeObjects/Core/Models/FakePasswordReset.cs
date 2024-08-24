using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakePasswordReset
{
    public static PasswordReset CreateValid(IFixture fixture)
    {
        return new PasswordReset
        {
            PasswordResetId = FakePasswordResetId.CreateValid(fixture),
            UserId = FakeUserId.CreateValid(fixture),
            User = FakeUser.CreateValid(fixture),
            ResetToken = FakePasswordResetToken.CreateValid(fixture),
            IsUsed = FakeIsUsed.CreateValid(fixture),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}