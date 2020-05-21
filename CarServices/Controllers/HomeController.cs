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

namespace CarServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartsRepository _partsRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;

        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, ICustomerRepository customerRepository, 
            IHttpContextAccessor httpContextAccessor, IPartsRepository partsRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
            _partsRepository = partsRepository;
        }

        public IActionResult Index()
        {
            List<Parts> model = new List<Parts>();
            model = _partsRepository.GetAllParts().Where(p => p.Quantity == 0).ToList();
            string parts = "";
            foreach(var m in model)
            {
                parts += @"\n" + m.Name;
            }
            return View("index", parts);
        }

        public IActionResult Privacy()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value; //id obecnie zalogowanego użytkownika z tabeli AspNetUsers
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
