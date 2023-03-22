using RPG.Game.Engine.ViewModels;

namespace BlazorRPG
{
    public static class AppServiceConfig
    {
        public static void ConfigureAppServices(IServiceCollection services)
        {
            // add app-specific/custom services here...
            services.AddSingleton<GameSession>();
        }

        public static void InitializeAppServices(IServiceProvider serviceProvider)
        {
            // add service initialization here...
        }
    }
}
