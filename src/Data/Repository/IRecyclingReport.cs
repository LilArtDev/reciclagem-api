using ReciclagemApi.Models;

namespace ReciclagemApi.Data.Repository
{
    public interface IRecyclingReportRepository
    {
        Task<IEnumerable<RecyclingReportModel>> GetUserReportsAsync(int userId, int page, int pageSize);
        Task AddAsync(RecyclingReportModel report);
    }
}
