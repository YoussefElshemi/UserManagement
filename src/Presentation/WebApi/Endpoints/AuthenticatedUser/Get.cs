using System.Net;
using Core.Models;
using FastEndpoints;
using Presentation.Constants;
using Presentation.WebApi.Models.AuthenticatedUser;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.AuthenticatedUser;

public class Get(IMapper mapper) : EndpointWithoutRequest<UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiRoutes.User.Get);
        EnableAntiforgery();

        Description(b => b
            .Produces<UserProfileResponseDto>()
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.User.Get;
            s.Description = SwaggerSummaries.User.Get;
        });

        Options(x => x.WithTags(SwaggerTags.User));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var authenticatedUser = HttpContext.Items["User"] as User;

        var response = mapper.Map<UserProfileResponseDto>(authenticatedUser!);

        await SendAsync(response, cancellation: cancellationToken);
    }
}