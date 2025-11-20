using AutoMapper.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data.Entities;
using ParkingManagements.server.Data.Entities;

namespace ParkingManagements.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor) :
            base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<ParkingLot> ParkingLots { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }
        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ParkingLot>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.ParkingSpots)
                      .WithOne(s => s.ParkingLot)
                      .HasForeignKey(s => s.LotId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Tariffs)
                      .WithOne(t => t.ParkingLot)
                      .HasForeignKey(t => t.LotId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Tickets)
                      .WithOne(t => t.ParkingLot)
                      .HasForeignKey(t => t.LotId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ParkingSpot>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Tickets)
                      .WithOne(t => t.ParkingSpot)
                      .HasForeignKey(t => t.SpotId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Tariff>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Tickets)
                      .WithOne(t => t.Vehicle)
                      .HasForeignKey(t => t.VehicleId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Payments)
                      .WithOne(p => p.Ticket)
                      .HasForeignKey(p => p.TicketId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
