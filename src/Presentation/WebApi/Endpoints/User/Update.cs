using System.Net;
using System.Security.Claims;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.User;

public class Update(IUserRepository userRepository,
    IUserService userService) : Endpoint<UpdateUserRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Put("/user");
        EnableAntiforgery();
    }

    public override async Task HandleAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserRequestDtoValidator(userRepository);
        await validator.ValidateAndThrowAsync(updateUserRequestDto, cancellationToken);

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
        var user = await userService.GetAsync(new UserId(Guid.Parse(userId)));

        var updateUserRequest = UpdateUserRequestMapper.Map(user!, updateUserRequestDto);

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = UserProfileResponseMapper.Map(updatedUser);

        await SendAsync(response, statusCode: (int)HttpStatusCode.Created, cancellation: cancellationToken);
    }
}