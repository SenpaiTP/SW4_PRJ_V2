using PRJ4.DTOs;

namespace PRJ4.Services
{
    public interface ILogQueryService
    {
        Task<IEnumerable<LogDto>> GetAllLogsAsync();
        Task<IEnumerable<LogDto>> GetLogsByLevelAsync(string level);
        Task<IEnumerable<LogDto>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<LogDto>> GetLogsByRequest(string request);
    }
}