using CarRentalService.Models;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    Task<bool> RegisterUser(User user);
    Task<string> AuthenticateUser(string email, string password);
}

namespace CarRentalService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<bool> RegisterUser(User user)
        {
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null) return false;
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _userRepository.AddUser(user);
            return true;
        }

        public async Task<string> AuthenticateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed) return null;
            return _jwtService.GenerateToken(user.Name, user.Role);
        }
    }
}
