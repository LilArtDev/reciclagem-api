using ReciclagemApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Services
{
    public interface IRecyclingReportService
    {
        Task<IEnumerable<RecyclingReportModel>> GetUserReportsAsync(int userId, int page, int pageSize);
        Task AddReportAsync(int userId, string material, int quantity);
    }
}
