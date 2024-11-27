using CarRentalService.Data;
using CarRentalService.Models;
using Microsoft.EntityFrameworkCore;

public interface ICarRepository
{
    Task<IEnumerable<Car>> GetAllCars();
    Task AddCar(Car car);
    Task<Car> GetCarById(int id);
    Task<IEnumerable<Car>> GetAvailableCars();
    Task UpdateCar(Car car);
    Task UpdateCarAvailability(int carId, bool isAvailable);

    Task DeleteCar(int id);
}

public class CarRepository : ICarRepository
{
    private readonly AppDbContext _context;
    public CarRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Car>> GetAllCars() =>
        await _context.Cars.ToListAsync();

    public async Task AddCar(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }

    public async Task<Car> GetCarById(int id) => await _context.Cars.FindAsync(id);

    public async Task<IEnumerable<Car>> GetAvailableCars() =>
        await _context.Cars.Where(c => c.IsAvailable).ToListAsync();

    public async Task UpdateCar(Car car)
    {
        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCarAvailability(int carId, bool isAvailable)
    {
        var car = await _context.Cars.FindAsync(carId);
        if (car != null)
        {
            car.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCar(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Car with ID {id} not found.");
        }
    }
}
