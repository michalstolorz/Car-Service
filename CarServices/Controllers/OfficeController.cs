using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarServices.Models;
using CarServices.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarServices.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarServices.Controllers
{
    public class OfficeController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarModelRepository _carModelRepository;
        private readonly ILocalDataRepository _localDataRepository;

        public OfficeController(ICustomerRepository customerRepository, ICarRepository carRepository,
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository, ILocalDataRepository localDataRepository)
        {
            _customerRepository = customerRepository;
            _carRepository = carRepository;
            _carBrandRepository = carBrandRepository;
            _carModelRepository = carModelRepository;
            _localDataRepository = localDataRepository;
        }

        //dodawanie/przegląd klientow, dodawanie aut, zamawianie czesci, udzielanie rabatu, dodawanie nowej naprawy

        [HttpGet]
        public IActionResult ListCustomers()
        {
            IEnumerable<Customer> listOfCustomers = _customerRepository.GetAllCustomer().ToList();
            return View(listOfCustomers);
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Discount = 0;
                _customerRepository.Add(customer);
                return RedirectToAction("index", "home");
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult AddCar()
        {
            AddCarViewModel model = new AddCarViewModel();
            model.CarBrands = new List<CarBrand>();
            model.CarBrands = _carBrandRepository.GetAllCarBrand().ToList();
            model.CustomersList = new List<Customer>();
            model.CustomersList = _customerRepository.GetAllCustomer().ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetModelByBrandId(int brandId)
        {
            List<CarModel> carModels = new List<CarModel>();
            carModels = _carModelRepository.GetAllCarModel().Where(m => m.BrandId == brandId).ToList();
            SelectList model = new SelectList(carModels, "Id", "Name", 0);
            return Json(model);
        }

        [HttpPost]
        public void FillModelId(int modelId)
        {
            _localDataRepository.SetModelId(modelId);
        }

        [HttpPost]
        public IActionResult AddCar(AddCarViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ChoosenModelId = _localDataRepository.GetModelId();
                Car car = new Car
                {
                    VIN = model.VIN.ToUpper(),
                    ProductionYear = model.ProductionYear,
                    ModelId = model.ChoosenModelId,
                    CustomerId = model.ChoosenCustomerId
                };
                _carRepository.Add(car);
                return RedirectToAction("index", "home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult SetDiscount()
        {
            SetDiscountViewModel model = new SetDiscountViewModel();
            model.CustomerList = new List<Customer>();
            model.CustomerList = _customerRepository.GetAllCustomer().ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult SetDiscount(SetDiscountViewModel setDiscountViewModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();
                customer = _customerRepository.GetCustomer(setDiscountViewModel.Id);
                customer.Discount = setDiscountViewModel.Discount;
                _customerRepository.Update(customer);
                return RedirectToAction("listcustomers", "office", _customerRepository.GetAllCustomer().ToList());
            }
            setDiscountViewModel.CustomerList = _customerRepository.GetAllCustomer().ToList();
            return View(setDiscountViewModel);
        }
    }
}