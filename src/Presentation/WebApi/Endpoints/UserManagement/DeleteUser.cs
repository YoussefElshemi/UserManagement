using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.WebApi.Models.UserManagement;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class DeleteUsers(IUserService userService,
    IUserRepository userRepository
) : Endpoint<GetUserRequestDto>
{
    public override void Configure()
    {
        Delete(ApiUrls.UserManagement.DeleteUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();
    }

    public override async Task HandleAsync(GetUserRequestDto getUserRequestDto, CancellationToken cancellationToken)
    {
        var validator = new DeleteUserRequestDtoValidator(userRepository);
        await validator.ValidateAndThrowAsync(getUserRequestDto, cancellationToken);

        var user = HttpContext.Items["User"] as Core.Models.User;

        await userService.DeleteAsync(new UserId(getUserRequestDto.UserId), user!.Username);

        await SendNoContentAsync(cancellation: cancellationToken);
    }
}