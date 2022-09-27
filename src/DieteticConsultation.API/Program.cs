using DieteticConsultationAPI;
using FluentValidation.AspNetCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddCustomLogging();
builder.Host.UseNLog();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddAutorizationServices();
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContextServices(builder.Configuration);
builder.Services.AddCustomServices();
builder.Services.AddMiddlewareCustomServices();
builder.Services.AddValidatorCustomServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();
app.UseSeeder();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseMiddleware();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dietetic Consulatation API"));
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers();});
app.Run();

