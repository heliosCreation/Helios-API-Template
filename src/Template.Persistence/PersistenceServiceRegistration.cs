using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Contrats.Persistence;
using Template.Persistence.Repositories;

namespace Template.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                opt => opt.UseSqlServer(configuration.GetConnectionString("DataConnectionString"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
            );

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            return services;

        }
    }
}
