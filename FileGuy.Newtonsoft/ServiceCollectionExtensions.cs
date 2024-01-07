namespace ToolBX.FileGuy.Newtonsoft;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Injects all services required to use FileGuy.Newtonsoft. Warning : Only use if you're not using ToolBX's AutoInject or AssemblyInitializer packages.
    /// </summary>
    public static IServiceCollection AddFileGuyJson(this IServiceCollection services, AutoInjectOptions? options = null)
    {
        return services.AddFileGuy()
            .AddAutoInjectServices(Assembly.GetExecutingAssembly(), options);
    }
}