using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarServices.Models;
using CarServices.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarServices.Controllers
{
    public class PartsController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PartsController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPartsRepository _partsRepository;

        public PartsController(ILogger<PartsController> logger, ICarRepository carRepository, ICustomerRepository customerRepository,
            IHttpContextAccessor httpContextAccessor, IPartsRepository partsRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
            _customerRepository = customerRepository;
            _httpContextAccessor = httpContextAccessor;
            _partsRepository = partsRepository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AvailiableParts()
        {
            var carParts = _partsRepository.GetAllParts();
            var viewModel = new PartsAvailiablePartsViewModel()
            {
                PartsList = carParts
            };
            return View(viewModel);
        }
        public IActionResult OrderingParts()
        {
            return View();
        }
        public IActionResult ReportingMissingParts()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddParts()
        {
            AddPartsViewModel addPartsViewModel = new AddPartsViewModel();
            addPartsViewModel.PartsList = _partsRepository.GetAllParts().ToList();

            return View(addPartsViewModel);
        }

        [HttpPost]
        public IActionResult AddParts(AddPartsViewModel addPartsViewModel)
        {
            if (ModelState.IsValid)
            {
                Parts updatedParts = _partsRepository.GetParts(addPartsViewModel.choosenPartsId);
                updatedParts.Quantity += addPartsViewModel.addedQuantity;
                _partsRepository.Update(updatedParts);
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Your Message');", true);
                return RedirectToAction("index", "home");
            }
            else
            {
                addPartsViewModel.PartsList = _partsRepository.GetAllParts().ToList();
                return View(addPartsViewModel);
            }
        }
    }
}
