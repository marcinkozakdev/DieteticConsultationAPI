using DieteticConsultationAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Runtime.CompilerServices;
using System.Text;

namespace DieteticConsultationAPI
{
    public static class LoggerBuilderExtension
    {
        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            return builder;
        }
    }
}

