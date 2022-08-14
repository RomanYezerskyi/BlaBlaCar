using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Services;
using BlaBlaCar.DAL;
using BlaBlaCar.DAL.Data;
using BlaBlaCar.DAL.Interfaces;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.BL;
using BlaBlaCar.BL.Services.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using BlaBlaCar.BL.Services.BookedTripServices;
using BlaBlaCar.BL.Services.TripServices;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//    {
//        options.Password.RequireDigit = false;
//        options.Password.RequireLowercase = false;
//        options.Password.RequireNonAlphanumeric = false;
//        options.Password.RequireUppercase = false;
//        options.Password.RequiredLength = 4;
//    }).AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<AuthorizationService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IBookedTripsService, BookedTripsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarSeatsService, CarSeatsService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IFileService, FileService>();

//builder.Services.AddAuthentication("Bearer")
//    .AddIdentityServerAuthentication("Bearer", options =>
//    {
//        options.ApiName = "myApi";
//        options.Authority = "https://localhost:44355";
//        options.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Jwt;
//    });
//builder.Services.AddAuthorization(o =>
//{
//    o.AddPolicy("All", b => { b.RequireScope("read"); });
//});



//builder.Services

//    .AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = false;
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidIssuer = "https://localhost:5001",
//            ValidAudience = "https://localhost:6001",
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
//            ClockSkew = TimeSpan.Zero
//        };
//    });
builder.Services.AddAuthentication(opt => {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:5001",
            ValidAudience = "https://localhost:6001",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//app.UseCors(builder => builder.WithOrigins("https://localhost:44321")
//    .AllowAnyHeader()
//    .AllowAnyMethod());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"DriverDocuments")),
    RequestPath = new PathString("/DriverDocuments")
});

app.UseAuthentication(); 
app.UseAuthorization();
app.UseCors("EnableCORS");

app.MapControllers();//.RequireAuthorization("ApiScope"); ;

app.Run();
