namespace ToolBX.FileGuy;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Injects all services required to use FileGuy. 
    /// </summary>
    public static IServiceCollection AddFileGuy(this IServiceCollection services, AutoInjectOptions? options = null)
    {
        services.AddOptions<FileSaveOptions>("DefaultFileSaving");
        return services.AddAutoInjectServices(Assembly.GetExecutingAssembly(), options);
    }
}