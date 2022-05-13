﻿using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Infrastructure;
using Shop.Application.Infrastructure.Automapper;
using Shop.Application.Infrastructure.TypeFinder;
using AutoMapper;

namespace Shop.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;

            var profiles = typeFinder
                .FindClassOfType<IProfile>()
                .Select(p => (IProfile)Activator.CreateInstance(p))
                .ToList();

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                foreach(var profile in profiles)
                {
                    config.AddProfile(profile.GetType());
                }
            });

            services.AddSingleton(mapperConfiguration.CreateMapper());

            return services;
        }
    }
}
