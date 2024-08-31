using System.Net;
using Core.Enums;
using Core.Interfaces.Services;
using FastEndpoints;
using Presentation.Constants;
using Presentation.WebApi.Common.Models;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Emails.Endpoints;

public class GetEmailsSortableFields(
    IEmailService emailService) : EndpointWithoutRequest<QueryFieldsResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiRoutes.Emails.GetEmailsSortableFields);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Produces<QueryFieldsResponseDto>()
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s => s.Summary = SwaggerSummaries.Emails.GetEmailsSortableFields);

        Options(x => x.WithTags(SwaggerTags.Emails));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var queryFieldsResponseDto = new QueryFieldsResponseDto
        {
            Fields = emailService.GetSortableFields().Keys.ToArray()
        };

        await SendAsync(queryFieldsResponseDto, cancellation: cancellationToken);
    }
}