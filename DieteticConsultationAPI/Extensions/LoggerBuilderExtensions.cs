using DieteticConsultationAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Runtime.CompilerServices;
using System.Text;

namespace DieteticConsultationAPI.Extensions
{
    public static class LoggerBuilderExtensions
    {
        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Trace);
            return builder;
        }
    }
}

