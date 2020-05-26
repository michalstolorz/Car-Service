using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class AppDbContext : IdentityDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Car { get; set; }
        public DbSet<CarBrand> CarBrand { get; set; }
        public DbSet<CarModel> CarModel { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Parts> Parts { get; set; }
        public DbSet<Repair> Repair { get; set; }
        public DbSet<RepairType> RepairType { get; set; }
        public DbSet<UsedParts> UsedParts { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<UsedRepairType> UsedRepairType { get; set; }
    }
}
