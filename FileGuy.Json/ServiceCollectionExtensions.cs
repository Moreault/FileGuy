﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ToolBX.FileGuy.Json;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Injects all services required to use FileGuy.Json. Warning : Only use if you're not using ToolBX's AutoInject or AssemblyInitializer packages.
    /// </summary>
    public static IServiceCollection AddFileGuyJson(this IServiceCollection services, AutoInjectOptions? options = null)
    {
        return services.AddFileGuy().AddAutoInjectServices(Assembly.GetExecutingAssembly(), options);
    }
}