using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarServices.Models;
using CarServices.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CarServices.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartsRepository _partsRepository;
        private readonly IRepairRepository _repairRepository;
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarModelRepository _carModelRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, ICustomerRepository customerRepository,
            IHttpContextAccessor httpContextAccessor, IPartsRepository partsRepository, IRepairRepository repairRepository,
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _carRepository = carRepository;
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
            _partsRepository = partsRepository;
            _repairRepository = repairRepository;
            _carBrandRepository = carBrandRepository;
            _carModelRepository = carModelRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<string> model = new List<string>();
            List<Parts> parts = _partsRepository.GetAllParts().Where(p => p.Quantity == 0).ToList();
            string modelString = "";
            foreach (var m in parts)
            {
                modelString += @"\n" + m.Name;
            }
            model.Add(modelString);
            modelString = "";
            List<Repair> repairs = _repairRepository.GetAllRepair().Where(r => r.StatusId == 9).ToList();
            repairs = repairs.GroupBy(r => r.CarId).Select(g => g.First()).ToList();
            foreach (var m in repairs)
            {
                Car car = _carRepository.GetCar(m.CarId);
                CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
                car.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                car.CarModel.CarBrand = carBrand;
                modelString += @"\n" + car.CarModel.CarBrand.Name + " " + car.CarModel.Name + " " + car.VIN;
            }
            model.Add(modelString);
            modelString = "";
            List<Repair> repairs2 = _repairRepository.GetAllRepair().Where(r => r.StatusId == 12).ToList();
            repairs2 = repairs2.GroupBy(r => r.CarId).Select(g => g.First()).ToList();
            foreach (var m in repairs2)
            {
                Car car = _carRepository.GetCar(m.CarId);
                CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
                car.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                car.CarModel.CarBrand = carBrand;
                modelString += @"\n" + car.CarModel.CarBrand.Name + " " + car.CarModel.Name + " " + car.VIN;
            }
            model.Add(modelString);
            return View("index", model);
        }

        public async Task<IActionResult> Privacy()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; //id obecnie zalogowanego użytkownika z tabeli AspNetUsers
            var user = await _userManager.FindByIdAsync(username);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.FirstOrDefault() != "Admin")
                return Unauthorized();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
