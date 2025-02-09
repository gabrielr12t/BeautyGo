using BeautyGo.Domain.Entities.Logging;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Persistence.Repositories;

internal class LogRepository : BaseRepository<Log>, ILogRepository
{
    public LogRepository(BeautyGoContext context) : base(context)
    {
    }
}
