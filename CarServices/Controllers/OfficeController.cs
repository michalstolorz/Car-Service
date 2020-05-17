using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarServices.Models;
using CarServices.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServices.Controllers
{
    public class OfficeController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarModelRepository _carModelRepository;

        public OfficeController(ICustomerRepository customerRepository, ICarRepository carRepository, 
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository)
        {
            _customerRepository = customerRepository;
            _carRepository = carRepository;
            _carBrandRepository = carBrandRepository;
            _carModelRepository = carModelRepository;
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
            if(ModelState.IsValid)
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
        public IActionResult AddCar(AddCarViewModel model)
        {
            if (ModelState.IsValid)
            {
                Car car = new Car
                {
                    ProductionYear = model.ProductionYear,
                    VIN = model.VIN,
                };
                _carRepository.Add(car);
                return RedirectToAction("index", "home");
            }
            return View();
        }
    }
}