using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DieteticConsultationAPI
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSeeder (this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DieteticConsultationSeeder>();
            seeder.Seed();

            return app;
        }

        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            return app;
        }
    }
}
