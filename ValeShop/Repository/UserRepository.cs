using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models;
using ValeShop.Models.Entities;

namespace ValeShop.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<User> CreateUser(UserViewModel userViewModel)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(userViewModel.Password);
            var userModel = new User
            {
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                UserName = userViewModel.UserName,
                Email = userViewModel.Email, 
                Password = password,
                ConfirmPassword = userViewModel.ConfirmPassword,
              
            };
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> DeleteUser(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> Login(LoginViewModel login)
        {
            var templogin = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email && x.Password == login.Password);
            return templogin;
        }

        public async Task<User?> UpdateUser(Guid id, UserViewModel userViewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return null;
            }

            user.FirstName = userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.Email = userViewModel.Email;
            user.UserName = userViewModel.UserName;
     

            await _context.SaveChangesAsync();
            return user;
        }
    }
}
