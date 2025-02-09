using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities.Logging;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrasctructure.Services.Logging;

public class DefaultLogger : ILogger
{
    #region Fields

    private readonly ILogRepository _repository;
    private readonly IWebHelper _webHelper;

    #endregion

    #region Ctor

    public DefaultLogger(
         IWebHelper webHelper,
         ILogRepository repository)
    {
        _webHelper = webHelper;
        _repository = repository;
    }

    #endregion

    #region Methods

    public virtual bool IsEnabled(LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => false,
            _ => true,
        };
    }

    public virtual void DeleteLog(Log log)
    {
        if (log == null)
            throw new ArgumentNullException(nameof(log));

        _repository.Remove(log);
    }

    public virtual void DeleteLogs(IReadOnlyCollection<Log> logs)
    {
        _repository.Remove(logs);
    }

    public virtual void ClearLog()
    {
        _repository.Truncate();
    }

    public virtual async Task<IPagedList<Log>> GetAllLogsAsync(string userEmail = null, DateTime? fromUtc = null, DateTime? toUtc = null,
        string message = "", LogLevel? logLevel = null,
        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        List<Specification<Log>> specifications = new List<Specification<Log>>();

        //var logs = await _repository.GetAllPagedAsync(query =>
        //{
        //    //if (!string.IsNullOrEmpty(userEmail))
        //    //{
        //    //    query = query.Join(_userRepository.Table(), x => x.UserId, y => y.Id,
        //    //            (x, y) => new { Log = x, User = y })
        //    //        .Where(z => z.User.Email == userEmail)
        //    //        .Select(z => z.Log);
        //    //}

        //    if (fromUtc.HasValue)
        //        query = query.Where(l => fromUtc.Value <= l.CreatedOn);

        //    if (toUtc.HasValue)
        //        query = query.Where(l => toUtc.Value >= l.CreatedOn);

        //    if (logLevel.HasValue)
        //    {
        //        var logLevelId = (int)logLevel.Value;
        //        query = query.Where(l => logLevelId == l.LogLevelId);
        //    }

        //    if (!string.IsNullOrEmpty(message))
        //        query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));

        //    query = query.OrderByDescending(l => l.CreatedOn);

        //    return query;
        //}, pageIndex, pageSize);

        //return logs;
        
        return null;

    }

    public virtual async Task<Log> GetLogByIdAsync(Guid logId)
    {
        return await _repository.GetByIdAsync(logId);
    }

    public virtual async Task<IList<Log>> GetLogByIdsAsync(IReadOnlyList<Guid> logIds)
    {
        return await _repository.GetByIdAsync(logIds);
    }

    public virtual async Task<Log> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "", User user = null)
    {
        var log = new Log
        {
            LogLevel = logLevel,
            ShortMessage = shortMessage,
            FullMessage = fullMessage,
            IpAddress = await _webHelper.GetCurrentIpAddressAsync(),
            UserId = user?.Id,
            PageUrl = _webHelper.GetThisPageUrl(true),
            ReferrerUrl = _webHelper.GetUrlReferrer(),
        };

        await _repository.InsertAsync(log);

        return log;
    }

    public virtual async Task InformationAsync(string message, Exception exception = null, User user = null)
    {
        //don't log thread abort exception
        if (exception is ThreadAbortException)
            return;

        if (IsEnabled(LogLevel.Information))
            await InsertLogAsync(LogLevel.Information, message, exception?.ToString() ?? string.Empty, user);
    }

    public virtual async Task WarningAsync(string message, Exception exception = null, User user = null)
    {
        //don't log thread abort exception
        if (exception is ThreadAbortException)
            return;

        if (IsEnabled(LogLevel.Warning))
            await InsertLogAsync(LogLevel.Warning, message, exception?.ToString() ?? string.Empty, user);
    }

    public virtual async Task ErrorAsync(string message, Exception exception = null, User user = null)
    {
        //don't log thread abort exception
        if (exception is System.Threading.ThreadAbortException)
            return;

        if (IsEnabled(LogLevel.Error))
            await InsertLogAsync(LogLevel.Error, message, exception?.ToString() ?? string.Empty, user);
    }

    #endregion
}
