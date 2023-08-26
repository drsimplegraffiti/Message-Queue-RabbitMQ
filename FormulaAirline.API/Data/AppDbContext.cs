using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormulaAirline.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FormulaAirline.API.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Booking> Bookings { get; set; } = null!;
    }

}