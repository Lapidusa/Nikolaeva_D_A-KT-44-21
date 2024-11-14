using project.Interfaces.StudentsInterfaces;
namespace project.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services) { 
            services.AddScoped<IStudentService, StudentService>();
            return services;
        }
    }
}
