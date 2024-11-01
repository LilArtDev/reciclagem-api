using ReciclagemApi.Data.Repository;
using ReciclagemApi.Models;
using ReciclagemApi.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclagemApi.Data.Repository
{
    public class RecyclingReportRepository : IRecyclingReportRepository
    {
        private readonly DatabaseContext _context;

        public RecyclingReportRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecyclingReportModel>> GetUserReportsAsync(int userId, int page, int pageSize)
        {
            return await _context.Reports
                .Where(r => r.UserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(RecyclingReportModel report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }
    }
}
