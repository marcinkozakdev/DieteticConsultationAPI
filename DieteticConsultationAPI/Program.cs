using DieteticConsultationAPI;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Middleware;
using DieteticConsultationAPI.Services;
//using DieteticConsultationAPI.Services;
using NLog.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//NLog: Setup NLog for Dependency injection
//builder.Logging.ClearProviders();
//builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container.
// ConfigureServices
builder.Services.AddControllers();
builder.Services.AddDbContext<DieteticConsultationDbContext>();
builder.Services.AddScoped<DieteticConsultationSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDieticianService, DieticianService>();
builder.Services.AddScoped<IDietService, DietService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RequestTimeMiddleware>();

builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure - ustawienie middleware dla projektu
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<DieteticConsultationSeeder>();
seeder.Seed();
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c=>
c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dietetic Consulatation API"));


app.UseAuthorization();

app.MapControllers();

app.Run();
