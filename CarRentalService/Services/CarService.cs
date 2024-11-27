using CarRentalService.Data;
public interface ICarService
{
    Task<bool> RentCar(int carId, int userId, int days);
    Task<bool> CheckCarAvailability(int carId);
}
namespace CarRentalService.Services
{
    public class CarService : ICarService
    { 

        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public CarService(ICarRepository carRepository, IEmailService emailService, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<bool> RentCar(int carId, int userId, int days)
        {

            var car = await _carRepository.GetCarById(carId);
            var user = await _userRepository.GetUserById(userId);
            if (car == null)
            {
                throw new ArgumentNullException(nameof(car), "Car not found.");
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User not found.");
            }
            car.IsAvailable = false;
            await _carRepository.UpdateCarAvailability(carId, false);

            // Send email notification
            await _emailService.SendRentalConfirmationEmailAsync(car,user,days);

            return true;
        }



        public async Task<bool> CheckCarAvailability(int carId)
        {
            var car = await _carRepository.GetCarById(carId);
            return car?.IsAvailable ?? false;
        }
    }
}
