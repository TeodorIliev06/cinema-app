﻿namespace CinemaApp.Web.Infrastructure.Extensions
{
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    using Data.Models;
    using Data.Repositories;
    using Services.Data.Contracts;
    using Data.Repositories.Contracts;

    public static class ServiceCollectionExtensions
    {
        public static void RegisterRepositories(this IServiceCollection services, Assembly modelsAssembly)
        {
            Type[] typesToExclude = { typeof(ApplicationUser) };
            Type[] modelsTypes = modelsAssembly
                .GetTypes()
                .Where(t => 
                    !t.IsAbstract && !t.IsInterface &&
                    !typeof(Attribute).IsAssignableFrom(t))
                .ToArray();

            foreach (Type type in modelsTypes)
            {
                if (!typesToExclude.Contains(type))
                {
                    Type repositoryInterfaceType = typeof(IRepository<,>);
                    Type repositoryInstanceType = typeof(BaseRepository<,>);

                    PropertyInfo? idPropInfo = type
                        .GetProperties()
                        .SingleOrDefault(p => p.Name.ToLower() == "id");

                    Type[] constructArgs = new Type[2];
                    constructArgs[0] = type;

                    if (idPropInfo == null)
                    {
                        constructArgs[1] = typeof(object);
                    }
                    else
                    {
                        constructArgs[1] = idPropInfo.PropertyType;
                    }

                    repositoryInterfaceType = repositoryInterfaceType.MakeGenericType(constructArgs);
                    repositoryInstanceType = repositoryInstanceType.MakeGenericType(constructArgs);

                    services.AddScoped(repositoryInterfaceType, repositoryInstanceType);
                }
            }
        }

        public static void RegisterUserDefinedServices(this IServiceCollection services, Assembly servicesAssembly)
        {
            Type[] serviceInterfaceTypes = servicesAssembly
                .GetTypes()
                .Where(t => t.IsInterface)
                .ToArray();
            Type[] serviceTypes = servicesAssembly
                .GetTypes()
                .Where(t =>
                    !t.IsInterface && !t.IsAbstract &&
                    t.Name.ToLower().EndsWith("service"))
                .ToArray();

            foreach (var serviceInterfaceType in serviceInterfaceTypes)
            {
                Type? serviceType = serviceTypes
                    .SingleOrDefault(t => 
                        String.Equals("I" + t.Name, serviceInterfaceType.Name, StringComparison.OrdinalIgnoreCase));

                if (serviceType == null)
                {
                    throw new NullReferenceException($"Service type could not be obtained {serviceInterfaceType.Name}");
                }

                services.AddScoped(serviceInterfaceType, serviceType);
            }
        }

        public static void RegisterUserDefinedServicesWebApi(this IServiceCollection services, Assembly serviceAssembly)
        {
            Type[] serviceInterfaceTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.IsInterface)
                .ToArray();
            Type[] serviceTypes = serviceAssembly
                .GetTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract &&
                            t.Name.ToLower().EndsWith("service"))
                .ToArray();
            foreach (Type serviceInterfaceType in serviceInterfaceTypes)
            {
                if (serviceInterfaceType.Name != nameof(IUserService))
                {
                    Type? serviceType = serviceTypes
                        .SingleOrDefault(t => "i" + t.Name.ToLower() == serviceInterfaceType.Name.ToLower());
                    if (serviceType == null)
                    {
                        throw new NullReferenceException(
                            $"Service type could not be obtained for the service {serviceInterfaceType.Name}");
                    }
                    services.AddScoped(serviceInterfaceType, serviceType);
                }
            }
        }
    }
}
