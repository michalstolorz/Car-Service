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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CarServices.Controllers
{
    public class OfficeController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICarBrandRepository _carBrandRepository;
        private readonly ICarModelRepository _carModelRepository;
        private readonly ILocalDataRepository _localDataRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IRepairTypeRepository _repairTypeRepository;
        private readonly IRepairRepository _repairRepository;
        private readonly IUsedRepairTypeRepository _usedRepairTypeRepository;

        public OfficeController(ICustomerRepository customerRepository, ICarRepository carRepository,
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository, ILocalDataRepository localDataRepository,
            IEmployeesRepository employeesRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IRepairTypeRepository repairTypeRepository, IRepairRepository repairRepository, IHttpContextAccessor httpContextAccessor,
            IUsedRepairTypeRepository usedRepairTypeRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _customerRepository = customerRepository;
            _carRepository = carRepository;
            _carBrandRepository = carBrandRepository;
            _carModelRepository = carModelRepository;
            _localDataRepository = localDataRepository;
            _employeesRepository = employeesRepository;
            _repairTypeRepository = repairTypeRepository;
            _repairRepository = repairRepository;
            _usedRepairTypeRepository = usedRepairTypeRepository;
        }

        //dodawanie/przegląd klientow, dodawanie aut, zamawianie czesci, udzielanie rabatu, dodawanie nowej naprawy

        [HttpGet]
        public IActionResult ListCustomers()
        {
            IEnumerable<Customer> listCustomers = _customerRepository.GetAllCustomer().ToList();
            return View(listCustomers);
        }

        [HttpGet]
        public async Task<IActionResult> ListEmployees()
        {
            IEnumerable<Employees> listEmployees = _employeesRepository.GetAllEmployees().ToList();
            List<ListEmployeesViewModel> model = new List<ListEmployeesViewModel>();
            foreach (var e in listEmployees)
            {
                ListEmployeesViewModel listEmployeesViewModel = new ListEmployeesViewModel();
                listEmployeesViewModel.Id = e.Id;
                listEmployeesViewModel.Name = e.Name;
                listEmployeesViewModel.Surname = e.Surname;
                var user = await _userManager.FindByIdAsync(e.UserId);
                var roles = await _userManager.GetRolesAsync(user);
                listEmployeesViewModel.Rolename = roles.FirstOrDefault();
                model.Add(listEmployeesViewModel);
            }
            return View(model);
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
            //model.CarBrands = new List<CarBrand>();
            model.CarBrands = _carBrandRepository.GetAllCarBrand().ToList();
            //model.CustomersList = new List<Customer>();
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
            //model.CarBrands = new List<CarBrand>();
            model.CarBrands = _carBrandRepository.GetAllCarBrand().ToList();
            //model.CustomersList = new List<Customer>();
            model.CustomersList = _customerRepository.GetAllCustomer().ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddRepair()
        {
            AddRepairViewModel addRepairViewModel = new AddRepairViewModel();
            addRepairViewModel.RepairTypeList = _repairTypeRepository.GetAllRepairType().ToList(); 
            addRepairViewModel.CarList = new List<SelectListItem>();
            var carList = _carRepository.GetAllCar().ToList();
            foreach (var c in carList)
            {
                CarModel carModel = _carModelRepository.GetCarModel(c.ModelId);
                c.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                c.CarModel.CarBrand = carBrand;
                addRepairViewModel.CarList.Add(new SelectListItem { Text = c.CarModel.CarBrand.Name.ToString() + " " + c.CarModel.Name.ToString() + " " + c.ProductionYear.ToString() + " " + c.VIN.ToString(), Value = c.Id.ToString() });
            }
            return View(addRepairViewModel);
        }

        [HttpPost]
        public IActionResult AddRepair(AddRepairViewModel addRepairViewModel)
        {
            if (ModelState.IsValid)
            {
                Repair repair = new Repair()
                {
                    CarId = addRepairViewModel.ChoosenCarId,
                    Status = "Waiting for assignment"
                };
                _repairRepository.Add(repair);
                List<Repair> repairs = _repairRepository.GetAllRepair().ToList();
                UsedRepairType usedRepairType = new UsedRepairType()
                {
                    RepairId = repairs.LastOrDefault().Id,
                    RepairTypeId = addRepairViewModel.ChoosenTypeId
                };
                _usedRepairTypeRepository.Add(usedRepairType);
                return RedirectToAction("index", "home");
            }
            addRepairViewModel.RepairTypeList = _repairTypeRepository.GetAllRepairType().ToList();
            addRepairViewModel.CarList = new List<SelectListItem>();
            var carList = _carRepository.GetAllCar().ToList();
            foreach (var c in carList)
            {
                CarModel carModel = _carModelRepository.GetCarModel(c.ModelId);
                c.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                c.CarModel.CarBrand = carBrand;
                addRepairViewModel.CarList.Add(new SelectListItem { Text = c.CarModel.CarBrand.Name.ToString() + " " + c.CarModel.Name.ToString() + " " + c.ProductionYear.ToString() + " " + c.VIN.ToString(), Value = c.Id.ToString() });
            }
            return View(addRepairViewModel);
        }

        [HttpGet]
        public IActionResult SetDiscount()
        {
            SetDiscountViewModel model = new SetDiscountViewModel();
            //model.CustomerList = new List<Customer>();
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

        [HttpGet]
        public IActionResult AddRepairType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRepairType(RepairType model)
        {
            if (ModelState.IsValid)
            {
                _repairTypeRepository.Add(model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}