using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AgentCommunicationUtility
{
    class Program
    {
        private static IServiceProvider serviceProvider;

        static async Task Main(string[] args)
        {
            // Registering types into client container
            ConfigureServices();
            // Run client application
            var exitCode = await serviceProvider.GetRequiredService<ClientApplication>().Run(args);
            // Dispose
            DisposeServices();
            await Task.Yield();
            Environment.Exit(exitCode);
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddScoped<IClientApplicationViewModel, ClientApplicationViewModel>()
                .AddSingleton<ClientApplication>()
                ;
            serviceProvider = services.BuildServiceProvider();
            serviceProvider.CreateScope();
        }

        private static void DisposeServices()
        {
            if (serviceProvider != null && serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
