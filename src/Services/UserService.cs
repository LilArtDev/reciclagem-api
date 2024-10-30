using ReciclagemApi.Models;
using ReciclagemApi.Data.Repository;

namespace ReciclagemApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserModel> RegisterUserAsync(UserModel user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<UserModel> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;
            return user;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync(int page, int pageSize)
        {
            return await _userRepository.GetAllAsync(page, pageSize);
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
