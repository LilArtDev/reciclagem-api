using ReciclagemApi.Models;
using ReciclagemApi.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<IEnumerable<MaterialModel>> GetAllMaterialsAsync(int page, int pageSize)
        {
            return await _materialRepository.GetAllAsync(page, pageSize);
        }

        public async Task<MaterialModel> GetMaterialByIdAsync(int id)
        {
            return await _materialRepository.GetByIdAsync(id);
        }

        public async Task AddMaterialAsync(MaterialModel material)
        {
            await _materialRepository.AddAsync(material);
        }

        public async Task UpdateMaterialAsync(MaterialModel material)
        {
            await _materialRepository.UpdateAsync(material);
        }

        public async Task DeleteMaterialAsync(int id)
        {
            await _materialRepository.DeleteAsync(id);
        }
    }
}
