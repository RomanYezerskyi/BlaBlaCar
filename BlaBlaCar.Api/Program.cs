using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Services;
using BlaBlaCar.DAL;
using BlaBlaCar.DAL.Data;
using BlaBlaCar.DAL.Interfaces;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.BL;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<ITripSeatsService, TripSeatsService>();
builder.Services.AddScoped<IUserTripsService, UserTripsService>();
builder.Services.AddScoped<IBookedSeatsService, BookedSeatsService>();
builder.Services.AddScoped<IBookedTripsService, BookedTripsService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7168/";
        options.Audience = "BlaBlaCar.API";
        options.RequireHttpsMetadata = false;
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
