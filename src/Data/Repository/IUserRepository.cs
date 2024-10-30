using ReciclagemApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Data.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetAllAsync(int page, int pageSize);
        Task<UserModel> GetByIdAsync(int id);
        Task<UserModel> GetByUsernameAsync(string username);
        Task AddAsync(UserModel user);
        Task UpdateAsync(UserModel user);
        Task DeleteAsync(int id);
    }
}
