namespace ToolBX.FileGuy;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Injects all services required to use FileGuy. Warning : Only use if you're not using ToolBX's AutoInject or AssemblyInitializer packages.
    /// </summary>
    public static IServiceCollection AddFileGuy(this IServiceCollection services)
    {
        return services.AddSingleton<IFileLoader, FileLoader>()
            .AddSingleton<IFileSaver, FileSaver>()
            .AddSingleton<IStreamCompressor, StreamCompressor>();
    }
}