using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace Infrastructure.Repositories;

public class EmailRepository(
    UserManagementDbContext context,
    IMapper mapper) : PagedRepository<EmailMessageOutboxEntity>(context, mapper), IEmailRepository
{
    private readonly IMapper _mapper = mapper;

    public Task<bool> ExistsAsync(Guid emailId)
    {
        return context.Emails.AnyAsync(x => x.EmailId == emailId);
    }

    public async Task<EmailMessage> GetAsync(Guid emailId)
    {
        var emailMessageEntity = await context.Emails
            .FirstAsync(x => x.EmailId == emailId);

        return _mapper.Map<EmailMessage>(emailMessageEntity);
    }

    public async Task CreateAsync(EmailMessage emailMessage)
    {
        var emailMessageEntity = _mapper.Map<EmailMessageOutboxEntity>(emailMessage);
        context.Emails.Add(emailMessageEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmailMessage emailMessage)
    {
        var emailMessageToUpdate = _mapper.Map<EmailMessageOutboxEntity>(emailMessage);
        var emailMessageEntity = await context.Emails.FirstAsync(x => x.EmailId == emailMessageToUpdate.EmailId);

        emailMessageEntity.IsProcessed = emailMessageToUpdate.IsProcessed;
        emailMessageEntity.UpdatedBy = emailMessageToUpdate.UpdatedBy;
        emailMessageEntity.UpdatedAt = emailMessageToUpdate.UpdatedAt;

        context.Emails.Update(emailMessageEntity);
        await context.SaveChangesAsync();
    }

    public override IDictionary<string, string> GetSortableFields()
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(EmailMessage.EmailAddress), nameof(EmailMessageOutboxEntity.EmailAddress) },
            { nameof(EmailMessage.CreatedAt), nameof(EmailMessageOutboxEntity.CreatedAt) },
            { nameof(EmailMessage.UpdatedAt), nameof(EmailMessageOutboxEntity.UpdatedAt) }
        };
    }

    public override IDictionary<string, string> GetSearchableFields()
    {
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(EmailMessage.EmailAddress), nameof(EmailMessageOutboxEntity.EmailAddress) },
            { nameof(EmailMessage.CreatedBy), nameof(EmailMessageOutboxEntity.CreatedBy) },
            { nameof(EmailMessage.UpdatedBy), nameof(EmailMessageOutboxEntity.UpdatedBy) }
        };
    }

    public async Task<IEnumerable<EmailMessage>> GetAwaitingEmailsAsync()
    {
        var emailMessageEntities = await context.Emails
            .Where(x => x.IsProcessed == false)
            .ToListAsync();

        return emailMessageEntities.Select(_mapper.Map<EmailMessage>);
    }
}