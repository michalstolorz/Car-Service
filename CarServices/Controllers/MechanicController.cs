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
    public class MechanicController : Controller
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
        private readonly IUsedPartsRepository _usedPartsRepository;

        public MechanicController(ICustomerRepository customerRepository, ICarRepository carRepository,
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository, ILocalDataRepository localDataRepository,
            IEmployeesRepository employeesRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IRepairTypeRepository repairTypeRepository, IRepairRepository repairRepository, IHttpContextAccessor httpContextAccessor,
            IPartsRepository partsRepository, IUsedPartsRepository usedPartsRepository)
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
            _usedPartsRepository = usedPartsRepository;
        }

        [HttpGet]
        //[Authorize(Policy = "Admin")]
        public IActionResult ListRepairAssign()
        {
            List<Repair> model = _repairRepository.GetAllRepair().ToList();
            foreach (var m in model)
            {
                Car car = _carRepository.GetCar(m.CarId);
                CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
                m.Car.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                m.Car.CarModel.CarBrand = carBrand;
                RepairType repairType = _repairTypeRepository.GetRepairType(m.TypeId ?? 1);
                m.RepairType = repairType;
                if (m.EmployeesId == null)
                    m.EmployeesId = 0;
                else
                {
                    Employees employee = _employeesRepository.GetEmployees(m.EmployeesId ?? 1);
                    m.Employees = employee;
                }
            }
            return View(model);
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin, Mechanic")]
        public IActionResult AssignRepair(int id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Employees employees = _employeesRepository.GetAllEmployees().Where(u => u.UserId == userId).FirstOrDefault();

            Repair repair = _repairRepository.GetRepair(id);
            repair.Status = "Assign to " + employees.Name + " " + employees.Surname;
            repair.EmployeesId = employees.Id;

            _repairRepository.Update(repair);
            return RedirectToAction("ListRepairAssign", "Mechanic");
        }

        [HttpGet]
        public IActionResult ListRepairAssignedToMechanic()
        {
            string user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Employees employees = _employeesRepository.GetEmployeesByUserId(user);
            List<Repair> model = _repairRepository.GetAllRepair().Where(r => r.EmployeesId == employees.Id).ToList();
            foreach (var m in model)
            {
                Car car = _carRepository.GetCar(m.CarId);
                CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
                m.Car.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                m.Car.CarModel.CarBrand = carBrand;
                RepairType repairType = _repairTypeRepository.GetRepairType(m.TypeId ?? 1);
                m.RepairType = repairType;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddPartToRepair(int id)
        {
            List<UsedParts> usedPartslist = _usedPartsRepository.GetAllUsedParts().Where(up => up.RepairId == id).ToList();
            foreach (var u in usedPartslist)
            {
                Parts part = _partsRepository.GetParts(u.PartId);
                u.Part = part;
            }
            AddPartToRepairViewModel model = new AddPartToRepairViewModel()
            {
                AvailableParts = _partsRepository.GetAllParts().Where(p => p.Quantity > 0).ToList(),
                UsedParts = usedPartslist,
                RepairId = id
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetQuantityByPartId(int partId)
        {
            //List<CarModel> carModels = new List<CarModel>();
            //carModels = _carModelRepository.GetAllCarModel().Where(m => m.BrandId == brandId).ToList();
            //SelectList model = new SelectList(carModels, "Id", "Name", 0);

            //List<int> qlist = new List<int>();
            //qlist.Add(_partsRepository.GetParts(partId).Quantity);
            //SelectList model = new SelectList( qlist, "Id", "Name", 0);

            List<int> qlist = new List<int>();
            qlist.Add(_partsRepository.GetParts(partId).Quantity);
            return Json(qlist);
        }

        [HttpPost]
        public IActionResult AddPartToRepair(AddPartToRepairViewModel model)
        {
            Parts partsCheck = _partsRepository.GetParts(model.ChoosenPartId);
            if (ModelState.IsValid)
            {
                if (partsCheck.Quantity >= model.UsedPartQuantity)
                {
                    UsedParts usedParts = new UsedParts()
                    {
                        RepairId = model.RepairId,
                        PartId = model.ChoosenPartId,
                        Quantity = model.UsedPartQuantity
                    };
                    _usedPartsRepository.Add(usedParts);
                    Parts parts = _partsRepository.GetParts(model.ChoosenPartId);
                    parts.Quantity -= model.UsedPartQuantity;
                    _partsRepository.Update(parts);
                    return RedirectToAction("AddPartToRepair", "Mechanic", model.RepairId);
                }
                ModelState.AddModelError(string.Empty, "Not enough parts in the warehouse");
            }
            List<UsedParts> usedPartslist = _usedPartsRepository.GetAllUsedParts().Where(up => up.RepairId == model.RepairId).ToList();
            foreach (var m in usedPartslist)
            {
                Parts part = _partsRepository.GetParts(m.PartId);
                m.Part = part;
            }
            model.AvailableParts = _partsRepository.GetAllParts().Where(p => p.Quantity > 0).ToList();
            model.UsedParts = usedPartslist;
            return View(model);
        }

        [HttpGet]
        public IActionResult SetRepairCost(int id)
        {
            List<UsedParts> usedPartslist = _usedPartsRepository.GetAllUsedParts().Where(up => up.RepairId == id).ToList();
            double summaryPriceForUsedParts = 0;
            foreach (var u in usedPartslist)
            {
                Parts part = _partsRepository.GetParts(u.PartId);
                u.Part = part;
                summaryPriceForUsedParts += u.Part.PartPrice * u.Quantity;
            }
            SetRepairCostViewModel model = new SetRepairCostViewModel()
            {
                UsedParts = usedPartslist,
                RepairId = id,
                CostForParts = summaryPriceForUsedParts
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SetRepairCost(SetRepairCostViewModel model)
        {
            if (ModelState.IsValid)
            {
                Repair repair = _repairRepository.GetRepair(model.RepairId);
                repair.Cost = model.CostForParts + model.CostForWork;
                _repairRepository.Update(repair);
            }
            List<UsedParts> usedPartslist = _usedPartsRepository.GetAllUsedParts().Where(up => up.RepairId == model.RepairId).ToList();
            foreach (var u in usedPartslist)
            {
                Parts part = _partsRepository.GetParts(u.PartId);
                u.Part = part;
            }
            model.UsedParts = usedPartslist;
            return View(model);
        }
    }
}
