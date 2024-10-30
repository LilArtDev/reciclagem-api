using ReciclagemApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Services
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialModel>> GetAllMaterialsAsync(int page, int pageSize);
        Task<MaterialModel> GetMaterialByIdAsync(int id);
        Task AddMaterialAsync(MaterialModel material);
        Task UpdateMaterialAsync(MaterialModel material);
        Task DeleteMaterialAsync(int id);
    }
}
