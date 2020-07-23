using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarServices.Models;
using CarServices.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Interactive;
using Microsoft.AspNetCore.Hosting;
using s2industries.ZUGFeRD;
using System.Xml;
using System.Text;
using System.Collections;
using CarServices.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CarServices.Controllers
{
    //[Authorize(Roles = "Boss, Admin")]
    public class BossController : Controller
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
        private readonly IUsedRepairTypeRepository _usedRepairTypeRepository;
        private readonly IWebHostEnvironment _environment;

        public BossController(ICustomerRepository customerRepository, ICarRepository carRepository,
            ICarBrandRepository carBrandRepository, ICarModelRepository carModelRepository, ILocalDataRepository localDataRepository,
            IEmployeesRepository employeesRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IRepairTypeRepository repairTypeRepository, IRepairRepository repairRepository, IHttpContextAccessor httpContextAccessor,
            IPartsRepository partsRepository, IUsedPartsRepository usedPartsRepository, IUsedRepairTypeRepository usedRepairTypeRepository,
            IWebHostEnvironment environment)
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
            _usedRepairTypeRepository = usedRepairTypeRepository;
            _environment = environment;
        }


        [HttpGet]
        public IActionResult CreateInvoice()
        {
            const int completeStatusId = 10;
            List<Repair> listRepairs = _repairRepository.GetAllRepair().Where(l => (l.Cost != null) && (l.StatusId != completeStatusId)).ToList();
            List<UsedRepairType> listUsedRepairTypes = _usedRepairTypeRepository.GetAllUsedRepairType().ToList();
            foreach (var l in listRepairs)
            {
                Car car = _carRepository.GetCar(l.CarId);
                l.Car = car;
                CarModel carModel = _carModelRepository.GetCarModel(car.ModelId);
                l.Car.CarModel = carModel;
                CarBrand carBrand = _carBrandRepository.GetCarBrand(carModel.BrandId);
                l.Car.CarModel.CarBrand = carBrand;
                Customer customer = _customerRepository.GetCustomer(car.CustomerId);
                l.Car.Customer = customer;
            }
            foreach (var l in listUsedRepairTypes)
            {
                RepairType repairType = _repairTypeRepository.GetRepairType(l.RepairTypeId);
                repairType.Name += "\n";
                l.RepairType = repairType;
            }
            CreateInvoiceViewModel model = new CreateInvoiceViewModel()
            {
                repairs = listRepairs,
                usedRepairTypes = listUsedRepairTypes
            };
            return View(model);
        }

        public IActionResult CreateInvoicePDF(int Id)
        {
            //Create PDF with PDF/A-3b conformance.
            PdfDocument document = new PdfDocument(PdfConformanceLevel.Pdf_A3B);
            //Set ZUGFeRD profile.
            document.ZugferdConformanceLevel = ZugferdConformanceLevel.Basic;

            //Create border color.
            PdfColor borderColor = new PdfColor(Color.FromArgb(255, 142, 170, 219));
            PdfBrush lightBlueBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 91, 126, 215)));

            PdfBrush darkBlueBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 65, 104, 209)));

            PdfBrush whiteBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 255, 255, 255)));
            PdfPen borderPen = new PdfPen(borderColor, 1f);

            string path = _environment.WebRootPath + "/arial.ttf";
            Stream fontStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

            //Create TrueType font.
            PdfTrueTypeFont headerFont = new PdfTrueTypeFont(fontStream, 30, PdfFontStyle.Regular);
            PdfTrueTypeFont arialRegularFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Regular);
            PdfTrueTypeFont arialBoldFont = new PdfTrueTypeFont(fontStream, 11, PdfFontStyle.Regular);


            const float margin = 30;
            const float lineSpace = 7;
            const float headerHeight = 90;

            //Add page to the PDF.
            PdfPage page = document.Pages.Add();

            PdfGraphics graphics = page.Graphics;

            //Get the page width and height.
            float pageWidth = page.GetClientSize().Width;
            float pageHeight = page.GetClientSize().Height;
            //Draw page border
            graphics.DrawRectangle(borderPen, new RectangleF(0, 0, pageWidth, pageHeight));

            //Fill the header with light Brush.
            graphics.DrawRectangle(lightBlueBrush, new RectangleF(0, 0, pageWidth, headerHeight));

            RectangleF headerAmountBounds = new RectangleF(400, 0, pageWidth - 400, headerHeight);

            graphics.DrawString("INVOICE", headerFont, whiteBrush, new PointF(margin, headerAmountBounds.Height / 3));

            graphics.DrawRectangle(darkBlueBrush, headerAmountBounds);

            graphics.DrawString("Amount", arialRegularFont, whiteBrush, headerAmountBounds, new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));

            Repair repair = _repairRepository.GetRepair(Id);
            Car car = _carRepository.GetCar(repair.CarId);
            repair.Car = car;
            Customer customer = _customerRepository.GetCustomer(repair.Car.CustomerId);
            repair.Car.Customer = customer;
            List<UsedRepairType> usedRepairTypesList = _usedRepairTypeRepository.GetAllUsedRepairType().Where(u => u.RepairId == Id).ToList();
            List<UsedParts> usedPartsList = _usedPartsRepository.GetAllUsedParts().Where(u => u.RepairId == Id).ToList();
            foreach (var u in usedRepairTypesList)
            {
                u.Repair = repair;
                u.RepairType = _repairTypeRepository.GetRepairType(u.RepairTypeId);
            }
            foreach (var u in usedPartsList)
            {
                u.Repair = repair;
                u.Part = _partsRepository.GetParts(u.PartId);
            }

            PdfTextElement textElement = new PdfTextElement("Invoice Number: " + repair.Id, arialRegularFont);

            PdfLayoutResult layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - margin, 120));

            textElement.Text = "Date : " + DateTime.Now.ToString("dddd dd, MMMM yyyy");
            textElement.Draw(page, new PointF(layoutResult.Bounds.X, layoutResult.Bounds.Bottom + lineSpace));

            textElement.Text = "Bill To:";
            layoutResult = textElement.Draw(page, new PointF(margin, 120));

            textElement.Text = customer.Name + " " + customer.Surname;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = customer.Email;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = customer.TelephoneNumber;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));

            PdfGrid grid = new PdfGrid();

            List<CreateInvoicePDFViewModel> model = new List<CreateInvoicePDFViewModel>();
            int i = 1;
            const double TaxNetValueValue = 0.77;
            const double VATValue = 0.23;
            double summaryPartsPrice = 0;
            foreach (var u in usedPartsList)
            {
                double TaxValueForList1 = (double)(u.Part.PartPrice * u.Quantity * VATValue);
                double NetPriceForList1 = u.Part.PartPrice * TaxNetValueValue;
                double NetValueForList1 = u.Part.PartPrice * u.Quantity * TaxNetValueValue;
                TaxValueForList1 = Math.Round(TaxValueForList1, 2);
                NetPriceForList1 = Math.Round(NetPriceForList1, 2);
                NetValueForList1 = Math.Round(NetValueForList1, 2);
                model.Add(new CreateInvoicePDFViewModel
                {
                    Id = i,
                    Name = u.Part.Name,
                    Quantity = u.Quantity,
                    NetPrice = NetPriceForList1,
                    NetValue = NetValueForList1,
                    Tax = (VATValue * 100).ToString() + "%",
                    TaxValue = TaxValueForList1,
                    SummaryPrice = u.Part.PartPrice * u.Quantity
                });


                summaryPartsPrice += u.Part.PartPrice * u.Quantity;
                i++;
            }
            string allRepairNames = "";
            foreach (var u in usedRepairTypesList)
                allRepairNames += u.RepairType.Name + "\n";

            double TaxValueForList2 = (double)(repair.Cost * VATValue);
            double NetPriceForList2 = (double)(repair.Cost * TaxNetValueValue);
            double NetValueForList2 = (double)(repair.Cost * TaxNetValueValue);
            TaxValueForList2 = Math.Round(TaxValueForList2, 2);
            NetPriceForList2 = Math.Round(NetPriceForList2, 2);
            NetValueForList2 = Math.Round(NetValueForList2, 2);
            model.Add(new CreateInvoicePDFViewModel
            {
                Id = i,
                Name = allRepairNames,
                Quantity = 1,
                NetPrice = NetPriceForList2,
                NetValue = NetValueForList2,
                Tax = (VATValue * 100).ToString() + "%",
                TaxValue = TaxValueForList2,
                SummaryPrice = (double)repair.Cost - summaryPartsPrice
            });

            grid.DataSource = model;

            grid.Columns[1].Width = 150;
            grid.Style.Font = arialRegularFont;
            grid.Style.CellPadding.All = 5;

            grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.ListTable4Accent5);

            layoutResult = grid.Draw(page, new PointF(0, layoutResult.Bounds.Bottom + 40));

            textElement.Text = "Discount: ";
            textElement.Font = arialBoldFont;
            layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - 40, layoutResult.Bounds.Bottom + lineSpace));

            textElement.Text = (customer.Discount).ToString() + "%";
            layoutResult = textElement.Draw(page, new PointF(layoutResult.Bounds.Right + 4, layoutResult.Bounds.Y));

            textElement.Text = "Grand Total: ";
            textElement.Font = arialBoldFont;
            layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - 40, layoutResult.Bounds.Bottom + lineSpace));

            float totalAmount = (float)((repair.Cost) * ((100 - customer.Discount) / 100));

            textElement.Text = totalAmount.ToString() + "PLN";
            layoutResult = textElement.Draw(page, new PointF(layoutResult.Bounds.Right + 4, layoutResult.Bounds.Y));

            graphics.DrawString(totalAmount.ToString() + "PLN", arialBoldFont, whiteBrush, new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));


            borderPen.DashStyle = PdfDashStyle.Custom;
            borderPen.DashPattern = new float[] { 3, 3 };

            PdfLine line = new PdfLine(borderPen, new PointF(0, 0), new PointF(pageWidth, 0));
            layoutResult = line.Draw(page, new PointF(0, pageHeight - 100));

            textElement.Text = "Car Services Adam Paluch";
            textElement.Font = arialRegularFont;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + (lineSpace * 3)));
            textElement.Text = "ul. Krakowska 19/2\n" +
                "02-20 Warsaw\n" +
                "NIP: 8127749027";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = "Any Questions? support@adventure-works.com";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));

            FileStream fileStream = new FileStream("Invoice" + repair.Id + ".pdf", FileMode.CreateNew, FileAccess.ReadWrite);
            document.Save(fileStream);
            document.Close(true);

            const int completeStatusId = 10;
            repair.StatusId = completeStatusId;
            repair.DateOfCompletion = DateTime.Now;
            _repairRepository.Update(repair);

            return RedirectToAction("index", "home");
        }



        [HttpGet]
        public IActionResult CreateReport()
        {
            CreateReportViewModel model = new CreateReportViewModel();
            return View(model);
        }



        [HttpPost]
        public IActionResult CreateReport(CreateReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Repair> repairs = _repairRepository.GetAllRepair().ToList();

                foreach (var repair in repairs)
                {
                    if (repair.DateOfCompletion != null)
                        if (repair.DateOfCompletion > DateTime.Now.AddDays(-model.ChoosenNumberOfDays))
                        {
                            CreateReportViewModel.RepairReportEntity repairEntity = new CreateReportViewModel.RepairReportEntity();
                            repairEntity.Repair = repair;
                            repairEntity.Employee = _employeesRepository.GetEmployees(repair.EmployeesId);
                            List<UsedRepairType> allUsedRepairTypes = _usedRepairTypeRepository.GetAllUsedRepairType().ToList();
                            List<UsedRepairType> usedRepairTypes = new List<UsedRepairType>();
                            foreach (var item in allUsedRepairTypes)
                            {
                                if (item.RepairId == repair.Id)
                                    usedRepairTypes.Add(item);
                            }
                            foreach (var item in usedRepairTypes)
                            {
                                item.RepairType = _repairTypeRepository.GetRepairType(item.RepairTypeId);
                            }
                            repairEntity.RepairType = usedRepairTypes;
                            List<UsedParts> allUsedParts = _usedPartsRepository.GetAllUsedParts().ToList();
                            List<UsedParts> usedParts = new List<UsedParts>();
                            foreach (var item in allUsedParts)
                            {
                                if (item.RepairId == repair.Id)
                                    usedParts.Add(item);
                            }
                            foreach (var item in allUsedParts)
                            {
                                item.Part = _partsRepository.GetParts(item.PartId);
                            }
                            repairEntity.UsedPartsList = usedParts;

                            model.RepairList.Add(repairEntity);
                        }
                }
                return View(model);
            }
            return View(model);
        }

    }
}
