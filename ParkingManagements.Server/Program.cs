using AutoMapper.Execution;
using Lidhja.Administrata.Server.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Data;
using ParkingManagement.server.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Parking Management",
        Version = "v1",
        Description = "API for managing parking lots"
    });
});

// Register Identity services
builder.Services
    .AddIdentityApiEndpoints<User>(o => o.SignIn.RequireConfirmedAccount = false)
    .AddSignInManager<LASignInManager>()
    .AddUserManager<UserManager<User>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Use Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking Management API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
