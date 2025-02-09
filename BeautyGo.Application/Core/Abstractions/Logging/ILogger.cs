﻿using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities.Logging;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Abstractions.Logging;

public interface ILogger
{
    bool IsEnabled(LogLevel level);

    void DeleteLog(Log log);

    void DeleteLogs(IReadOnlyCollection<Log> logs);

    void ClearLog();

    Task<IPagedList<Log>> GetAllLogsAsync(string userEmail = null, DateTime? fromUtc = null, DateTime? toUtc = null,
        string message = "", LogLevel? logLevel = null,
        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Log> GetLogByIdAsync(Guid logId);

    Task<IList<Log>> GetLogByIdsAsync(IReadOnlyList<Guid> logIds);

    Task<Log> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "", User User = null);

    Task InformationAsync(string message, Exception exception = null, User user = null);

    Task WarningAsync(string message, Exception exception = null, User user = null);

    Task ErrorAsync(string message, Exception exception = null, User user = null);
}
