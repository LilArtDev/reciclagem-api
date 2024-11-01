using ReciclagemApi.Data.Repository;
using ReciclagemApi.Models;
using ReciclagemApi.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclagemApi.Data.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly DatabaseContext _context;

        public MaterialRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaterialModel>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Materials
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MaterialModel> GetByIdAsync(int id)
        {
            return await _context.Materials.FindAsync(id);
        }

        public async Task AddAsync(MaterialModel material)
        {
            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MaterialModel material)
        {
            _context.Materials.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var material = await GetByIdAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();
            }
        }
    }
}
