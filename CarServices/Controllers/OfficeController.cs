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
using System.Net.Mail;

namespace CarServices.Controllers
{
    //[Authorize(Roles = "Officeworker, Admin")]
    public class OfficeController : Controller
    {
        const int waitingForAssigmentStatusId = 7;
        const int repairInProgressStatusId = 8;
        const int forClientDecisionStatusId = 9;
        const int completeStatusId = 10;
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
        private readonly IRepairStatusRepository _repairStatusRepository;
        private readonly IRepairRepository _repairRepository;
        private readonly IPartsRepository _partsRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IUsedPartsRepository _usedPartsRepository;
        private readonly IUsedRepairTypeRepository _usedRepairTypeRepository;

        public OfficeController(ICustomerRepository customerRepository, ICarRepository carRepository,
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository, ILocalDataRepository localDataRepository,
            IEmployeesRepository employeesRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IRepairTypeRepository repairTypeRepository, IRepairRepository repairRepository, IHttpContextAccessor httpContextAccessor,
            IPartsRepository partsRepository, IOrderRepository orderRepository, IOrderDetailsRepository orderDetailsRepository,
            IUsedPartsRepository usedPartsRepository, IUsedRepairTypeRepository usedRepairTypeRepository, IRepairStatusRepository repairStatusRepository)
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
            _repairStatusRepository = repairStatusRepository;
            _repairRepository = repairRepository;
            _partsRepository = partsRepository;
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _usedPartsRepository = usedPartsRepository;
            _usedRepairTypeRepository = usedRepairTypeRepository;
        }

        [HttpGet]
        public IActionResult ListCustomers()
        {
            IEnumerable<Customer> listCustomers = _customerRepository.GetAllCustomer().ToList();
            return View(listCustomers);
        }

        [HttpGet]
        public IActionResult ListCars()
        {
            IEnumerable<Car> listCars = _carRepository.GetAllCar().ToList();
            foreach(var l in listCars)
            {
                CarModel carModel = _carModelRepository.GetCarModel(l.ModelId);
                l.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                l.CarModel.CarBrand = carBrand;
                Customer customer = _customerRepository.GetCustomer(l.CustomerId);
                l.Customer = customer;
            }
            return View(listCars);
        }

        [HttpGet]
        public IActionResult ListAllRepairs()
        {
            List<Repair> listRepairs = _repairRepository.GetAllRepair().OrderBy(r => r.StatusId).ToList();
            foreach (var l in listRepairs)
            {
                Car car = _carRepository.GetCar(l.CarId);
                Customer customer = _customerRepository.GetCustomer(car.CustomerId);
                l.Car.Customer = customer;
                CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
                l.Car.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                l.Car.CarModel.CarBrand = carBrand;
                RepairStatus repairStatus = _repairStatusRepository.GetRepairStatus(l.StatusId ?? repairInProgressStatusId);
                l.RepairStatus = repairStatus;
                if (l.EmployeesId == null)
                    l.EmployeesId = 0;
                else
                {
                    Employees employee = _employeesRepository.GetEmployees(l.EmployeesId ?? 1);
                    l.Employees = employee;
                }
            }
            List<UsedParts> listUsedParts = _usedPartsRepository.GetAllUsedParts().ToList();
            foreach (var u in listUsedParts)
            {
                Parts part = _partsRepository.GetParts(u.PartId);
                u.Part = part;
            }
            List<UsedRepairType> listUsedRepairTypes = _usedRepairTypeRepository.GetAllUsedRepairType().ToList();
            foreach (var l in listUsedRepairTypes)
            {
                RepairType repairType = _repairTypeRepository.GetRepairType(l.RepairTypeId);
                l.RepairType = repairType;
            }
            ListAllRepairsViewModel model = new ListAllRepairsViewModel()
            {
                Repairs = listRepairs,
                UsedParts = listUsedParts,
                UsedRepairTypes = listUsedRepairTypes
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRepairsInProgress()
        {
            //listRepairs.RemoveAll(l => l.Status != "Repair in progress");
            //model.repairs.RemoveAll(l => l.Status != "Repair in progress");

            //List<Repair> listRepairs = _repairRepository.GetAllRepair().Where(r => (r.StatusId == repairInProgressStatusId) || (r.StatusId == forClientDecisionStatusId)).ToList();
            List<Repair> listRepairs = _repairRepository.GetAllRepair().Where(r => (r.StatusId != waitingForAssigmentStatusId) && (r.StatusId != completeStatusId)).ToList();
            List<UsedRepairType> listUsedRepairTypes = _usedRepairTypeRepository.GetAllUsedRepairType().ToList();
            foreach (var l in listRepairs)
            {
                Car car = _carRepository.GetCar(l.CarId);
                Customer customer = _customerRepository.GetCustomer(car.CustomerId);
                l.Car.Customer = customer;
                CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
                l.Car.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                l.Car.CarModel.CarBrand = carBrand;
                RepairStatus repairStatus = _repairStatusRepository.GetRepairStatus(l.StatusId ?? repairInProgressStatusId);
                l.RepairStatus = repairStatus;
                if (l.EmployeesId == null)
                    l.EmployeesId = 0;
                else
                {
                    Employees employee = _employeesRepository.GetEmployees(l.EmployeesId ?? 1);
                    l.Employees = employee;
                }
            }
            foreach (var l in listUsedRepairTypes)
            {
                RepairType repairType = _repairTypeRepository.GetRepairType(l.RepairTypeId);
                //repairType.Name += "\n";
                l.RepairType = repairType;
            }
            //var repairListWaitingForAssignFirst = listRepairs.OrderBy(l => l.EmployeesId);
            //listRepairs.RemoveAll(l => l.Status != "Repair in progress");
            ListRepairAssignViewModel model = new ListRepairAssignViewModel()
            {
                Repairs = listRepairs,
                UsedRepairTypes = listUsedRepairTypes
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult CustomerCarRepair(int Id)
        {
            Repair repair = _repairRepository.GetRepair(Id);
            Car car = _carRepository.GetCar(repair.CarId);
            repair.Car = car;
            CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
            repair.Car.CarModel = carModel;
            CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
            repair.Car.CarModel.CarBrand = carBrand;
            Customer customer = _customerRepository.GetCustomer(car.CustomerId);

            List<UsedRepairType> listUsedRepairTypes = _usedRepairTypeRepository.GetAllUsedRepairType().Where(l => l.RepairId == repair.Id).ToList();
            foreach (var l in listUsedRepairTypes)
            {
                RepairType repairType = _repairTypeRepository.GetRepairType(l.RepairTypeId);
                l.RepairType = repairType;
            }
            CustomerCarRepairViewModel customerCarRepairViewModel = new CustomerCarRepairViewModel()
            {
                Repair = repair,
                Customer = customer,
                UsedRepairTypes = listUsedRepairTypes,
                RepairId = Id
            };
            return View(customerCarRepairViewModel);
        }

        [HttpPost]
        public IActionResult CustomerCarRepairCustomerAgree(CustomerCarRepairViewModel customerCarRepairViewModel)
        {
            Repair repair = _repairRepository.GetRepair(customerCarRepairViewModel.RepairId);
            repair.StatusId = repairInProgressStatusId;
            repair.Description += "Customer agree for additional repairs. ";
            _repairRepository.Update(repair);
            return RedirectToAction("ListRepairsInProgress", "Office");
        }

        [HttpPost]
        public IActionResult CustomerCarRepairCustomerDisagree(CustomerCarRepairViewModel customerCarRepairViewModel)
        {
            if (ModelState.IsValid)
            {
                Repair repair = _repairRepository.GetRepair(customerCarRepairViewModel.RepairId);
                repair.StatusId = repairInProgressStatusId;
                repair.Description += customerCarRepairViewModel.Description + ". ";
                _repairRepository.Update(repair);
                return RedirectToAction("ListRepairsInProgress", "Office");
            }
            Repair repairForModel = _repairRepository.GetRepair(customerCarRepairViewModel.RepairId);
            Car car = _carRepository.GetCar(repairForModel.CarId);
            repairForModel.Car = car;
            CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
            repairForModel.Car.CarModel = carModel;
            CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
            repairForModel.Car.CarModel.CarBrand = carBrand;
            Customer customer = _customerRepository.GetCustomer(car.CustomerId);

            List<UsedRepairType> listUsedRepairTypes = _usedRepairTypeRepository.GetAllUsedRepairType().Where(l => l.RepairId == repairForModel.Id).ToList();
            foreach (var l in listUsedRepairTypes)
            {
                RepairType repairType = _repairTypeRepository.GetRepairType(l.RepairTypeId);
                l.RepairType = repairType;
            }
            customerCarRepairViewModel.Repair = repairForModel;
            customerCarRepairViewModel.Customer = customer;
            customerCarRepairViewModel.UsedRepairTypes = listUsedRepairTypes;
            return View("CustomerCarRepair", customerCarRepairViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ListEmployees()
        {
            IEnumerable<Employees> listEmployees = _employeesRepository.GetAllEmployees().ToList();
            List<ListEmployeesViewModel> model = new List<ListEmployeesViewModel>();
            foreach (var e in listEmployees)
            {
                ListEmployeesViewModel listEmployeesViewModel = new ListEmployeesViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname
                };
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
            AddCarViewModel model = new AddCarViewModel
            {
                CarBrands = _carBrandRepository.GetAllCarBrand().ToList()
            };
            List<Customer> customerList = _customerRepository.GetAllCustomer().ToList();
            model.CustomersList = new List<SelectListItem>();
            foreach (var c in customerList)
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
            AddRepairViewModel model = new AddRepairViewModel
            {
                RepairTypeList = _repairTypeRepository.GetAllRepairType().ToList(),
                CarList = new List<SelectListItem>()
            };
            List<Car> carList = _carRepository.GetAllCar().ToList();
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
                const int repairWaitingForAssignmentStatusId = 7;
                Repair repair = new Repair()
                {
                    CarId = model.ChoosenCarId,
                    StatusId = repairWaitingForAssignmentStatusId
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
                    Customer customer = _customerRepository.GetCustomer(model.Id);
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
            CreateOrderViewModel model = new CreateOrderViewModel
            {
                AllPartsList = _partsRepository.GetAllParts().ToList(),
                PartsToOrderList = _localDataRepository.GetOrderDetails()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderViewModel createOrderViewModel)
        {
            string user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Employees employees = _employeesRepository.GetEmployeesByUserId(user);
            Order order = new Order()
            {
                //order.Employees = employees;
                EmployeesId = employees.Id,
                OrderTime = DateTime.Now,
                Status = "In progress"
            };
            _orderRepository.Add(order);
            List<OrderDetails> orderDetails = _localDataRepository.GetOrderDetails();
            foreach (OrderDetails o in orderDetails)
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
                OrderDetails orderDetail = new OrderDetails
                {
                    Part = _partsRepository.GetParts(createOrderViewModel.ChoosenPartId),
                    PartId = createOrderViewModel.ChoosenPartId,
                    Quantity = createOrderViewModel.AddedQuantity
                };
                _localDataRepository.AddOrderDetail(orderDetail);
                return RedirectToAction("createorder", "office");
            }
            createOrderViewModel.AllPartsList = _partsRepository.GetAllParts().ToList();
            createOrderViewModel.PartsToOrderList = _localDataRepository.GetOrderDetails();
            return View("CreateOrder", createOrderViewModel);
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
                List<RepairType> RepairTypes = _repairTypeRepository.GetAllRepairType().Where(r => r.Name == model.Name).ToList();
                if (RepairTypes.Count == 0)
                {
                    _repairTypeRepository.Add(model);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Repair type already exist");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddCarBrand()
        {
            List<CarBrand> listCarBrands = _carBrandRepository.GetAllCarBrand().ToList();
            AddCarBrandViewModel model = new AddCarBrandViewModel()
            { CarBrands = listCarBrands };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCarBrand(AddCarBrandViewModel model)
        {
            if (ModelState.IsValid)
            {
                CarBrand carBrand = new CarBrand()
                { Name = model.NewCarBrand };
                _carBrandRepository.Add(carBrand);
            }
            List<CarBrand> listCarBrands = _carBrandRepository.GetAllCarBrand().ToList();
            model.CarBrands = listCarBrands;
            return View(model);
        }

        [HttpGet]
        public IActionResult AddCarModel(int Id)
        {
            List<CarModel> listCarModels = _carModelRepository.GetAllCarModel().Where(c => c.BrandId == Id).ToList();
            AddCarModelViewModel model = new AddCarModelViewModel()
            {
                CarModels = listCarModels,
                CarBrandId = Id
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCarModel(AddCarModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                CarModel carModel = new CarModel()
                {
                    Name = model.NewCarModel,
                    BrandId = model.CarBrandId
                };
                _carModelRepository.Add(carModel);
            }
            List<CarModel> listCarModels = _carModelRepository.GetAllCarModel().Where(c => c.BrandId == model.CarBrandId).ToList();
            model.CarModels = listCarModels;
            return View(model);
        }
    }
}