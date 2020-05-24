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
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, ICustomerRepository customerRepository, 
            IHttpContextAccessor httpContextAccessor, IPartsRepository partsRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _carRepository = carRepository;
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
            _partsRepository = partsRepository;
            _userManager = userManager;
            _roleManager = roleManager;
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
