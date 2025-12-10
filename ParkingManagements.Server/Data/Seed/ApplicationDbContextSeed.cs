using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingManagements.Data;
using ParkingManagements.Data.Entities.Enums;
using ParkingManagements.server.Data.Entities;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        await context.Database.MigrateAsync();

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await roleManager.RoleExistsAsync("Attendant"))
            await roleManager.CreateAsync(new IdentityRole("Attendant"));

        if (!await roleManager.RoleExistsAsync("Viewer"))
            await roleManager.CreateAsync(new IdentityRole("Viewer"));

        // Admin
        var adminUser = await userManager.FindByEmailAsync("admin@parking.com");
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = "admin@parking.com",
                Email = "admin@parking.com",
                EmailConfirmed = true,
                Role = UserRole.Admin
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (!result.Succeeded)
                throw new Exception($"Failed to create Admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            await userManager.AddToRoleAsync(adminUser, "Admin");

        // Attendant
        var attendantUser = await userManager.FindByEmailAsync("attendant@parking.com");
        if (attendantUser == null)
        {
            attendantUser = new User
            {
                UserName = "attendant@parking.com",
                Email = "attendant@parking.com",
                EmailConfirmed = true,
                Role = UserRole.Attendant
            };

            var result = await userManager.CreateAsync(attendantUser, "Attendant123!");
            if (!result.Succeeded)
                throw new Exception($"Failed to create Attendant user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        if (!await userManager.IsInRoleAsync(attendantUser, "Attendant"))
            await userManager.AddToRoleAsync(attendantUser, "Attendant");

        // Seed Parking Lot
        if (!context.ParkingLots.Any())
        {
            var lot = new ParkingLot
            {
                Id = Guid.NewGuid(),
                Name = "Central Parking Lot",
                Address = "123 Main Street",
                Timezone = "UTC",
            };

            // Generate 30–50 spots
            var spots = new List<ParkingSpot>();
            for (int i = 1; i <= 40; i++)
            {
                SpotType type = i <= 10 ? SpotType.Regular :
                                i <= 25 ? SpotType.EV : SpotType.Disabled;

                spots.Add(new ParkingSpot
                {
                    Id = Guid.NewGuid(),
                    LotId = lot.Id,
                    SpotCode = $"S{i:00}",
                    Type = type,
                    Status = SpotStatus.Free
                });
            }
            lot.ParkingSpots = spots;

            // Default Tariff
            lot.Tariffs.Add(new Tariff
            {
                Id = Guid.NewGuid(),
                LotId = lot.Id,
                RatePerHour = 2.5m,
                BillingPeriodMinutes = 60,
                GracePeriodMinutes = 15,
                DailyMaximum = 20m,
                LostTicketFee = 50m,
                EffectiveFrom = DateTime.UtcNow
            });

            await context.ParkingLots.AddAsync(lot);
            await context.SaveChangesAsync();
        }
    }
}
