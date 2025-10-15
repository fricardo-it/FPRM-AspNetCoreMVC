using Microsoft.EntityFrameworkCore;
using FPRMAspNetCoreMVC.Models;

namespace FPRMAspNetCoreMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Apartment> Apartment { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Building> Building { get; set; }

    }
}
