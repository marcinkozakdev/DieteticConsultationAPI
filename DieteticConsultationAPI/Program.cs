using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DieteticConsultationDbContext>();
    context.Database.Migrate();
}
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



