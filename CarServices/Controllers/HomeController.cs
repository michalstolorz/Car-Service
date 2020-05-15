using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarServices.Models;

namespace CarServices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;

        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
            _customerRepository = customerRepository;
        }

        public IActionResult Index()
        {
            int a = 1;
            string b = "dupa";
            var model = _carRepository.GetCar(a);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
