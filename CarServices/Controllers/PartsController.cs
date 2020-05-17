﻿using System;
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
            var viewModel = new PartsAvailiablePartsViewModel() {
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
    }
}
