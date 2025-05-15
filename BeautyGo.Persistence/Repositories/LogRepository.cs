using BeautyGo.Domain.Entities.Logging;
using BeautyGo.Domain.Repositories;
using BeautyGo.Persistence.Repositories.Bases;

namespace BeautyGo.Persistence.Repositories;

internal class LogRepository : EFBaseRepository<Log>, ILogRepository
{
    public LogRepository(BeautyGoContext context) : base(context)
    {
    }
}
