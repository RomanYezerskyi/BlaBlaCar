using System.Configuration;
using System.Text;
using BlaBlaCar.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlaBlaCar.BL.Interfaces;
using BlaBlaCar.BL.Services;
using BlaBlaCar.DAL;
using BlaBlaCar.DAL.Data;
using BlaBlaCar.DAL.Interfaces;
using BlaBlaCar.DAL.Entities;
using BlaBlaCar.BL;
using BlaBlaCar.BL.ExceptionHandler;
using BlaBlaCar.BL.Hubs;
using BlaBlaCar.BL.Logger;
using BlaBlaCar.BL.Services.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using BlaBlaCar.BL.Services.BookedTripServices;
using BlaBlaCar.BL.Services.ChatServices;
using BlaBlaCar.BL.Services.MapsServices;
using BlaBlaCar.BL.Services.NotificationServices;
using BlaBlaCar.BL.Services.TripServices;
using EmailService.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using NLog;
using EmailService.Services;

var builder = WebApplication.CreateBuilder(args);


LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                   sqlOption =>
                       sqlOption.UseNetTopologySuite()));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));
builder.Services.AddHangfireServer();

builder.Services.Configure<HostSettings>(builder.Configuration.GetSection("ApiHostSettings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});




builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IBookedTripsService, BookedTripsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarSeatsService, CarSeatsService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatHubService, ChatHubService>();
builder.Services.AddScoped<IMapService, MapsService>();
builder.Services.AddSingleton<ILog, NLogger>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
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
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(jwtSettings.WebClientUrl);
    });
});


builder.Services.AddSignalR();

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
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseCors("EnableCORS");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.MapHub<NotificationHub>("/notify");
app.MapHub<ChatHub>("/chatHub");
app.UseHangfireDashboard();
//BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});//.RequireAuthorization("ApiScope");

app.Run();
