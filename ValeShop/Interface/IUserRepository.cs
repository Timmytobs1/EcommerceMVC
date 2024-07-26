using ValeShop.Models.Entities;
using ValeShop.Models;

namespace ValeShop.Interface
{
    public interface IUserRepository
    {

        public Task<List<User>> GetAllUsers();
        public Task<User?> GetById(Guid id);
        public Task<User> CreateUser(UserViewModel userViewModel);
        public Task<User?> UpdateUser(Guid id, UserViewModel userViewModel);
        public Task<User?> DeleteUser(Guid id);
        public Task<User?> Login(LoginViewModel login);
    }
}
