using AuthBase.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthBase.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the application's dependencies 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterApplicationDenpendecies(this IServiceCollection services)
        {                  
            services.Scan(scan => scan
                    .FromAssemblyOf<Service>()
                    .AddClasses(c => c.InNamespaces("Services"))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );


            return services;
        }
    }
}
