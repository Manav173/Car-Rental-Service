using CarRentalService.Data;
using CarRentalService.Models;
using Microsoft.EntityFrameworkCore; 
using System.Threading.Tasks;

public interface IUserRepository
{
    Task AddUser(User user);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(int id);

    Task<bool> DeleteUser(int id);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
      
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User> GetUserById(int id)
    {
        // Validate input id (optional)
        if (id <= 0)
        {
            throw new ArgumentException("Invalid user ID.", nameof(id));
        }

        // Fetch the user by ID and handle the null case
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            // Log or throw a custom exception, based on your needs
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        return user;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false; // User not found
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true; // User successfully deleted
    }

}

