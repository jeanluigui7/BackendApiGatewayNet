using Common.GoogleCaptcha;
using Common.JwtToken;
using Common.Mail;
using Common.Utilitary;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTokenService(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IUtilService, UtilService>();
            services.AddTransient<IGoogleCaptchaService, GoogleCaptchaService>();
            return services;
        }
    }
}
