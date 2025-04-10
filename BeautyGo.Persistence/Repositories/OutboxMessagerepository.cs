﻿using BeautyGo.Domain.Entities;
using BeautyGo.Domain.Patterns.Specifications.OutboxMessages;
using BeautyGo.Domain.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeautyGo.Persistence.Repositories;

internal class OutboxMessagerepository : BaseRepository<OutboxMessage>, IOutboxMessageRepository
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
        const string query = "SELECT Id, ProcessedOn, OccurredOn, Content, Type, Error FROM OutboxMessage WHERE Id = @Id";
        return await _connetion.QueryFirstOrDefaultAsync<OutboxMessage>(query, new { Id = id });
    }

    public async Task<ICollection<OutboxMessage>> GetRecentUnprocessedOutboxMessages(int size)
    {
        var unProcessedOutboxMessagesSpec = new UnprocessedOutboxMessagesSpecification(5);

        var query = Query();

        query = query.Where(unProcessedOutboxMessagesSpec.ToExpression());

        query = query.OrderByDescending(p => p.CreatedOn);

        return await query.ToListAsync();
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
