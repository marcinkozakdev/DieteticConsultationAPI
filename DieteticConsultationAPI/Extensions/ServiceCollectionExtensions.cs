using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Models.Validators;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Runtime.CompilerServices;
using System.Text;
using DieteticConsultationAPI.Middleware;
using Microsoft.AspNetCore.Identity;
using DieteticConsultationAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using DieteticConsultationAPI.Repositories.Abstractions;
using DieteticConsultationAPI.Repositories;
using DieteticConsultationAPI.Services.Interfaces;

namespace DieteticConsultationAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DieteticConsultationDbContext>
                  (options => options.UseSqlServer(configuration.GetConnectionString("DieteticConsultationDbConnection")));

            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationSettings = new AuthenticationSettings();
            configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = authenticationSettings.JwtIssuer,
                        ValidAudience = authenticationSettings.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                    };
                });

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<DieteticConsultationSeeder>();
            services.AddScoped<IDieticianService, DieticianService>();
            services.AddScoped<IDietService, DietService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IDieticianRepository, DieticianRepository>();
            services.AddScoped<IDietRepository, DietRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();

            return services;
        }

        public static IServiceCollection AddMiddlewareCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();

            return services;
        }

        public static IServiceCollection AddValidatorCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<PatientQuery>, PatientQueryValidatior>();

            return services;
        }

        public static IServiceCollection AddAutorizationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();

            return services;
        }




    }
}

