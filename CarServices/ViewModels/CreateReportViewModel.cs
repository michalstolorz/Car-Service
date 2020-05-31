using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class CreateReportViewModel
    {
        public class RepairReportEntity
        {
            public Repair Repair { get; set; }
            public List<UsedRepairType> RepairType { get; set; }
            public Employees Employee { get; set; }
            public List<UsedParts> UsedPartsList { get; set; }
        }
        public List<RepairReportEntity> RepairList { get; set; }
    
        [Required]
        [Display(Name = "NumberOfDays")]
        [Range(1, 2147483647, ErrorMessage = "Number of days should be between 1 and 2 147 483 647")]
        public int ChoosenNumberOfDays { get; set; }

        public CreateReportViewModel()
        {
            RepairList = new List<RepairReportEntity>();
        }

    }
}
