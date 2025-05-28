using WhatsQ.Repositories;
using WhatsQ.Providers;

public static class ConfigureHelper
{
    public static void RegisterServices(IServiceCollection services)
    {
       

        // Patient Status
        services.AddScoped<IPatientStatusRepository, PatientStatusRepository>();
        services.AddScoped<IPatientStatusProvider, PatientStatusProvider>();

        // Add more modules here if needed
    }
}
