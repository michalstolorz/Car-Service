using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Employees
    {
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsHired { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public virtual Employees Employees { get; set; }
    }
}
