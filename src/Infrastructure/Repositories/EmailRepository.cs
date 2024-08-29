using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace Infrastructure.Repositories;

public class EmailRepository(
    PennyPlannerDbContext context,
    IMapper mapper) : IEmailRepository
{
    public async Task CreateAsync(EmailMessage emailMessage)
    {
        var emailMessageEntity = mapper.Map<EmailMessageOutboxEntity>(emailMessage);
        context.Emails.Add(emailMessageEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmailMessage emailMessage)
    {
        var emailMessageToUpdate = mapper.Map<EmailMessageOutboxEntity>(emailMessage);
        var emailMessageEntity = await context.Emails.FirstAsync(x => x.EmailId == emailMessageToUpdate.EmailId);

        emailMessageEntity.IsProcessed = emailMessageToUpdate.IsProcessed;
        emailMessageEntity.UpdatedBy = emailMessageToUpdate.UpdatedBy;
        emailMessageEntity.UpdatedAt = emailMessageToUpdate.UpdatedAt;

        context.Emails.Update(emailMessageEntity);
        await context.SaveChangesAsync();
    }
}