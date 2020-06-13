using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarServices.Models;
using CarServices.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarServices.Controllers
{
    [AllowAnonymous]
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
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
        public IActionResult SupplyParts()
        {
            SupplyPartsViewModel supplyPartsViewModel = new SupplyPartsViewModel();
            supplyPartsViewModel.PartsList = _partsRepository.GetAllParts().ToList();

            return View(supplyPartsViewModel);
        }

        [HttpPost]
        public IActionResult SupplyParts(SupplyPartsViewModel supplyPartsViewModel)
        {
            if (ModelState.IsValid)
            {
                Parts updatedParts = _partsRepository.GetParts(supplyPartsViewModel.ChoosenPartsId);
                updatedParts.Quantity += supplyPartsViewModel.AddedQuantity;
                _partsRepository.Update(updatedParts);
                return RedirectToAction("AvailiableParts", "Parts");
            }
            supplyPartsViewModel.PartsList = _partsRepository.GetAllParts().ToList();
            return View(supplyPartsViewModel);
        }

        [HttpGet]
        public IActionResult AddParts()
        {
            AddPartsViewModel addPartsViewModel = new AddPartsViewModel();
            return View(addPartsViewModel);
        }

        [HttpPost]
        public IActionResult AddParts(AddPartsViewModel addPartsViewModel)
        {

            if (ModelState.IsValid)
            {
                if (addPartsViewModel.PartPrice < 0.0)
                {
                    ModelState.AddModelError(string.Empty, "Price of the part cannot be negative");
                    return View(addPartsViewModel);
                }

                //List<Parts> PartsList = _partsRepository.GetAllParts().ToList();
                //foreach (var element in PartsList)
                //{
                //    if (element.Name.Equals(addPartsViewModel.Name))
                //    {
                //        ModelState.AddModelError(string.Empty, "Parts with this name already exists ");
                //        return View(addPartsViewModel);
                //    }
                //}

                List<Parts> PartsList = _partsRepository.GetAllParts().Where(p => p.Name == addPartsViewModel.Name).ToList();
                if (PartsList.Count == 0)
                {
                    Parts parts = new Parts
                    {
                        Name = addPartsViewModel.Name,
                        Quantity = addPartsViewModel.Quantity,
                        PartPrice = addPartsViewModel.PartPrice
                    };
                    _partsRepository.Add(parts);
                    return RedirectToAction("AvailiableParts", "Parts");
                }
                ModelState.AddModelError(string.Empty, "Parts with this name already exists ");
            }
            return View(addPartsViewModel);
        }
    }
}
