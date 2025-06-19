using BeautyGo.Domain.Entities.Outbox;
using BeautyGo.Domain.Patterns.Specifications.OutboxMessages;
using BeautyGo.Domain.Repositories;
using BeautyGo.Persistence.Repositories.Bases;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeautyGo.Persistence.Repositories;

internal class OutboxMessagerepository : EFBaseRepository<OutboxMessage>, IOutboxMessageRepository
{
    private readonly IDbConnection _connetion;

    public OutboxMessagerepository(
        BeautyGoContext context,
        IDbConnection connetion) : base(context)
    {
        _connetion = connetion;
    }

    public override async Task<OutboxMessage> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
        SELECT 
            OM.Id, 
            OM.ProcessedOn,
            OM.OccurredOn,
            OM.Content, 
            OM.Type,
            EE.Id AS ErrorId,
            EE.EventId AS ErrorEventId,
            EE.Message AS ErrorMessage
        FROM OutboxMessage OM
        LEFT JOIN Events.EventError EE ON OM.Id = EE.EventId
        WHERE OM.Id = @Id";

        var outboxDictionary = new Dictionary<Guid, OutboxMessage>();

        await _connetion.QueryAsync<OutboxMessage, OutboxMessageError, OutboxMessage>(
            sql,
            (outbox, error) =>
            {
                if (!outboxDictionary.TryGetValue(outbox.Id, out var outboxEntry))
                {
                    outboxEntry = outbox;
                    outboxEntry.Errors = new List<OutboxMessageError>();
                    outboxDictionary.Add(outboxEntry.Id, outboxEntry);
                }

                if (error != null && error.Id != Guid.Empty)
                {
                    outboxEntry.Errors.Add(error);
                }

                return outboxEntry;
            },
            new { Id = id },
            splitOn: "ErrorId"
        );

        return outboxDictionary.Values.FirstOrDefault();
    }

    public async Task<ICollection<OutboxMessage>> GetRecentUnprocessedOutboxMessages(int size, CancellationToken cancellation = default)
    {
        var unProcessedOutboxMessagesSpec = new UnprocessedOutboxMessagesSpecification(size);

        var query = QueryNoTracking();

        query = query.Where(unProcessedOutboxMessagesSpec.ToExpression());

        query = query.OrderByDescending(p => p.CreatedOn);

        return await query.ToListAsync(cancellation);
    }

    public async Task UpdateAsync(OutboxMessage message)
    {
        const string query = @"
            UPDATE OutboxMessage
            SET ProcessedOn = @ProcessedOn, Error = @Error
            WHERE Id = @Id";

        await _connetion.ExecuteAsync(query, message);
    }
}
