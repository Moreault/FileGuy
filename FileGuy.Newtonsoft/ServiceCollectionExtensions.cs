namespace FileGuy.Newtonsoft;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Injects all services required to use FileGuy.Newtonsoft. Warning : Only use if you're not using ToolBX's AutoInject or AssemblyInitializer packages.
    /// </summary>
    public static IServiceCollection AddFileGuyJson(this IServiceCollection services)
    {
        return services.AddFileGuy()
            .AddSingleton<IJsonConverterFetcher, JsonConverterFetcher>()
            .AddSingleton<IFileSerializer, FileSerializer>();
    }
}