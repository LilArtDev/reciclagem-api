using ReciclagemApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Data.Repository
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<MaterialModel>> GetAllAsync(int page, int pageSize);
        Task<MaterialModel> GetByIdAsync(int id);
        Task AddAsync(MaterialModel material);
        Task UpdateAsync(MaterialModel material);
        Task DeleteAsync(int id);
    }
}
