using CarRentalService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carRentalService;
        private readonly ICarRepository _carRepository;

        public CarController(ICarService carRentalService, ICarRepository carRepository)
        {
            _carRentalService = carRentalService;
            _carRepository = carRepository;
        }

        // Endpoint to get all cars
        [HttpGet("all")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carRepository.GetAllCars();
            return Ok(cars);
        }

        // Endpoint to get a car by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carRepository.GetCarById(id);
            if (car == null)
                return NotFound($"Car with ID {id} not found.");

            return Ok(car);
        }

        // Endpoint to get all available cars
        [HttpGet("available")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAvailableCars()
        {
            var cars = await _carRepository.GetAvailableCars();
            return Ok(cars);
        }

        // Endpoint to add a new car
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCar([FromBody] Car car)
        {
            await _carRepository.AddCar(car);
            return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
        }

        // Endpoint to rent a car
        [HttpPost("rent/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RentCar(int id, int UserId, int days)
        {
            var isAvailable = await _carRentalService.CheckCarAvailability(id);
            if (!isAvailable)
                return BadRequest($"Car with ID {id} is not available for rent.");

            var rentalSuccess = await _carRentalService.RentCar(id, UserId, days);
            if (!rentalSuccess)
                return BadRequest($"Car with ID {id} could not be rented.");

            return Ok($"Car with ID {id} has been rented successfully.");
        }

        // Endpoint to update a car
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] Car updatedCar)
        {
            var car = await _carRepository.GetCarById(id);
            if (car == null)
                return NotFound($"Car with ID {id} not found.");

            car.Make = updatedCar.Make;
            car.Model = updatedCar.Model;
            car.Year = updatedCar.Year;
            car.PricePerDay = updatedCar.PricePerDay;
            car.IsAvailable = updatedCar.IsAvailable;

            await _carRepository.UpdateCar(car);
            return Ok($"Car with ID {id} has been updated.");
        }

        // Endpoint to update car availability
        [HttpPatch("{id}/availability")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCarAvailability(int id, [FromBody] bool isAvailable)
        {
            var car = await _carRepository.GetCarById(id);
            if (car == null)
                return NotFound($"Car with ID {id} not found.");

            await _carRepository.UpdateCarAvailability(id, isAvailable);
            return Ok($"Car with ID {id} availability has been updated to {isAvailable}.");
        }

        // Endpoint to delete a car 
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carRepository.GetCarById(id);
            if (car == null)
                return NotFound($"Car with ID {id} not found.");

            await _carRepository.DeleteCar(id);
            return Ok($"Car with ID {id} has been deleted.");
        }

        // Endpoint to filter cars based on availability, make, and year
        [HttpGet("filter")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> FilterCars([FromQuery] bool? isAvailable, [FromQuery] string make, [FromQuery] int? year)
        {
            var cars = await _carRepository.GetAllCars();

            // Apply filters if provided
            if (isAvailable.HasValue)
            {
                cars = cars.Where(c => c.IsAvailable == isAvailable.Value).ToList();
            }

            if (!string.IsNullOrEmpty(make))
            {
                cars = cars.Where(c => c.Make.Contains(make, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (year.HasValue)
            {
                cars = cars.Where(c => c.Year == year.Value).ToList();
            }

            return Ok(cars);
        }

        // Endpoint to sort cars based on year or price
        [HttpGet("sort")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> SortCars([FromQuery] string sortBy, [FromQuery] string sortOrder)
        {
            var cars = await _carRepository.GetAllCars();

            // Apply sorting based on year and price
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "year":
                        cars = sortOrder == "desc" ? cars.OrderByDescending(c => c.Year).ToList() : cars.OrderBy(c => c.Year).ToList();
                        break;
                    case "price":
                        cars = sortOrder == "desc" ? cars.OrderByDescending(c => c.PricePerDay).ToList() : cars.OrderBy(c => c.PricePerDay).ToList();
                        break;
                    default:
                        return BadRequest("Invalid sorting field.");
                }
            }

            return Ok(cars);
        }
    }
}
