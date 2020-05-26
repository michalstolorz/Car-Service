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
        private readonly IPartsRepository _partsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IUsedRepairTypeRepository _usedRepairTypeRepository;

        

        public OfficeController(ICustomerRepository customerRepository, ICarRepository carRepository,
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository, ILocalDataRepository localDataRepository,
            IEmployeesRepository employeesRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IRepairTypeRepository repairTypeRepository, IRepairRepository repairRepository, IHttpContextAccessor httpContextAccessor, 
            IPartsRepository partsRepository, IOrderRepository orderRepository, IOrderDetailsRepository orderDetailsRepository,
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
            _partsRepository = partsRepository;
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _usedRepairTypeRepository = usedRepairTypeRepository;
        }

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
            model.CarBrands = _carBrandRepository.GetAllCarBrand().ToList();
            List<Customer> customerList = _customerRepository.GetAllCustomer().ToList(); 
            model.CustomersList = new List<SelectListItem>();
            foreach(var c in customerList)
                model.CustomersList.Add(new SelectListItem { Text = c.Name + " " + c.Surname, Value = c.Id.ToString() });

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
            model.CarBrands = _carBrandRepository.GetAllCarBrand().ToList();
            List<Customer> customerList = _customerRepository.GetAllCustomer().ToList();
            model.CustomersList = new List<SelectListItem>();
            foreach (var c in customerList)
                model.CustomersList.Add(new SelectListItem { Text = c.Name + " " + c.Surname, Value = c.Id.ToString() });

            return View(model);
        }

        [HttpGet]
        public IActionResult AddRepair()
        {
            AddRepairViewModel model = new AddRepairViewModel();
            model.RepairTypeList = _repairTypeRepository.GetAllRepairType().ToList();
            model.CarList = new List<SelectListItem>();
            var carList = _carRepository.GetAllCar().ToList();
            foreach (var c in carList)
            {
                CarModel carModel = _carModelRepository.GetCarModel(c.ModelId);
                c.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                c.CarModel.CarBrand = carBrand;
                model.CarList.Add(new SelectListItem { Text = c.CarModel.CarBrand.Name.ToString() + " " + c.CarModel.Name.ToString() + " " + c.ProductionYear.ToString() + " " + c.VIN.ToString(), Value = c.Id.ToString() });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AddRepair(AddRepairViewModel model)
        {
            if (ModelState.IsValid)
            {
                Repair repair = new Repair()
                {
                    CarId = model.ChoosenCarId,
                    Status = "Waiting for assignment"
                };
                _repairRepository.Add(repair);
                List<Repair> repairs = _repairRepository.GetAllRepair().ToList();
                UsedRepairType usedRepairType = new UsedRepairType()
                {
                    RepairId = repairs.LastOrDefault().Id,
                    RepairTypeId = model.ChoosenTypeId
                };
                _usedRepairTypeRepository.Add(usedRepairType);
                return RedirectToAction("index", "home");
            }
            model.RepairTypeList = _repairTypeRepository.GetAllRepairType().ToList();
            model.CarList = new List<SelectListItem>();
            var carList = _carRepository.GetAllCar().ToList();
            foreach (var c in carList)
            {
                CarModel carModel = _carModelRepository.GetCarModel(c.ModelId);
                c.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                c.CarModel.CarBrand = carBrand;
                model.CarList.Add(new SelectListItem { Text = c.CarModel.CarBrand.Name.ToString() + " " + c.CarModel.Name.ToString() + " " + c.ProductionYear.ToString() + " " + c.VIN.ToString(), Value = c.Id.ToString() });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SetDiscount()
        {
            SetDiscountViewModel model = new SetDiscountViewModel();
            List<Customer> customerList = _customerRepository.GetAllCustomer().ToList();
            model.CustomersList = new List<SelectListItem>();
            foreach (var c in customerList)
                model.CustomersList.Add(new SelectListItem { Text = c.Name + " " + c.Surname, Value = c.Id.ToString() });


            return View(model);
        }

        [HttpPost]
        public IActionResult SetDiscount(SetDiscountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Discount >= 0 && model.Discount <= 100)
                {
                    Customer customer = new Customer();
                    customer = _customerRepository.GetCustomer(model.Id);
                    customer.Discount = model.Discount;
                    _customerRepository.Update(customer);
                    return RedirectToAction("listcustomers", "office", _customerRepository.GetAllCustomer().ToList());
                }
                ModelState.AddModelError(string.Empty, "Wrong value of discount. It sholud be between 0 and 100");
            }
            List<Customer> customerList = _customerRepository.GetAllCustomer().ToList();
            model.CustomersList = new List<SelectListItem>();
            foreach (var c in customerList)
                model.CustomersList.Add(new SelectListItem { Text = c.Name + " " + c.Surname, Value = c.Id.ToString() });

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            CreateOrderViewModel model = new CreateOrderViewModel();
            model.AllPartsList = _partsRepository.GetAllParts().ToList();
            model.PartsToOrderList = _localDataRepository.GetOrderDetails();
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderViewModel createOrderViewModel)
        {
            string user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Employees employees = _employeesRepository.GetEmployeesByUserId(user);
            Order order = new Order();
            //order.Employees = employees;
            order.EmployeesId = employees.Id;
            order.OrderTime = DateTime.Now;
            order.Status = "In progress";
            _orderRepository.Add(order);
            List<OrderDetails> orderDetails = _localDataRepository.GetOrderDetails();
            foreach(OrderDetails o in orderDetails)
            {
                //o.Order = order;
                o.OrderId = order.Id;
                o.Part = null;
                _orderDetailsRepository.Add(o);
            }
            _localDataRepository.ClearOrderDetails();
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public IActionResult AddToOrder(CreateOrderViewModel createOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                List<OrderDetails> orderDetails = _localDataRepository.GetOrderDetails();
 
                foreach (var o in orderDetails)
                {
                    if (o.PartId == createOrderViewModel.ChoosenPartId)
                    {
                        o.Quantity += createOrderViewModel.AddedQuantity;
                        return RedirectToAction("createorder", "office");
                    }
                }
                 OrderDetails orderDetail = new OrderDetails();
                 orderDetail.Part = _partsRepository.GetParts(createOrderViewModel.ChoosenPartId);
                 orderDetail.PartId = createOrderViewModel.ChoosenPartId;
                 orderDetail.Quantity = createOrderViewModel.AddedQuantity;
                 _localDataRepository.AddOrderDetail(orderDetail);
                return RedirectToAction("createorder", "office");
            }
            createOrderViewModel.AllPartsList = _partsRepository.GetAllParts().ToList();
            createOrderViewModel.PartsToOrderList = _localDataRepository.GetOrderDetails();
            return View("CreateOrder",createOrderViewModel);
        }

        
        [HttpGet]
        public IActionResult RemovePartFromOrder(int i)
        {
            _localDataRepository.DeleteOrderDetail(i);
            return RedirectToAction("createorder", "office");
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
                List<RepairType> repairTypes = _repairTypeRepository.GetAllRepairType().ToList();
                if (repairTypes.FirstOrDefault().Equals(null))
                {
                    _repairTypeRepository.Add(model);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Repair type already exist");
            }
            return View(model);
        }
    }
}