using ReciclagemApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReciclagemApi.Services
{
    public interface IUserService
    {
        Task<UserModel> RegisterUserAsync(UserModel user, string password);
        Task<UserModel> AuthenticateUserAsync(string username, string password);
        Task<IEnumerable<UserModel>> GetAllUsersAsync(int page, int pageSize);
        Task<UserModel> GetUserByIdAsync(int id);
        Task DeleteUserAsync(int id);
    }
}
