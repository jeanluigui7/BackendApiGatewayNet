using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMvc(setup =>
            {
                //...mvc setup...
            }).AddFluentValidation();
            //services.AddTransient<IValidator<CreateUserPersonCommand>, CreateUserPersonCommandValidator>();

            return services;
        }
    }
}
