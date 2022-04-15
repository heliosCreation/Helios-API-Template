using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Contracts.Infrastructure;
using Template.Application.Model.Mail;
using Template.Infrastructure.FileExport;
using Template.Infrastructure.Mail;

namespace Template.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICsvExporterService, CsvExporterService>();
            return services;
        }
    }
}
