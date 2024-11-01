using ReciclagemApi.Models;
using ReciclagemApi.Services;
using ReciclagemApi.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Services
{
    public class RecyclingReportService : IRecyclingReportService
    {
        private readonly IRecyclingReportRepository _reportRepository;

        public RecyclingReportService(IRecyclingReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<RecyclingReportModel>> GetUserReportsAsync(int userId, int page, int pageSize)
        {
            return await _reportRepository.GetUserReportsAsync(userId, page, pageSize);
        }

        public async Task AddReportAsync(int userId, string material, int quantity)
        {
            var report = new RecyclingReportModel
            {
                UserId = userId,
                Material = material,
                Quantity = quantity,
                Date = DateTime.UtcNow
            };
            await _reportRepository.AddAsync(report);
        }
    }
}
