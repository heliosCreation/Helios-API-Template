using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Template.API;
using Template.Application.Features.Account;
using Template.Application.Features.Account.Command.Authenticate;
using Template.Application.Features.Categories;
using Template.Application.Features.Categories.Commands.Create;
using Template.Application.Responses;
using Template.Identity;
using Template.Persistence;
using static Template.API.Contract.ApiRoutes;

namespace Api.IntegrationTest.Base
{
    public class IntegrationTestBase : IDisposable
    {
        protected readonly HttpClient TestClient;
        private IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        protected IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()

                .WithWebHostBuilder(builder =>
                {
                    //Change the json file used by the integration tests
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        _configuration = new ConfigurationBuilder()
                         .AddJsonFile("integrationsettings.json")
                         .Build();

                        config.AddConfiguration(_configuration);
                    });

                    builder.ConfigureServices(services =>
                    {
                        //Remove the Data DbContext service from the original startup
                        services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                        //Add our test data db
                        var dataConnectionString = _configuration.GetConnectionString("IntegrationData").Replace("{name}", Guid.NewGuid().ToString());
                        services.AddDbContext<AppDbContext>(
                           opt => opt.UseSqlServer(dataConnectionString,
                           b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

                        services.RemoveAll(typeof(DbContextOptions<AppIdentityDbContext>));
                        var identityConnectionString = _configuration.GetConnectionString("IntegrationIdentity").Replace("{name}", Guid.NewGuid().ToString());
                        services.AddDbContext<AppIdentityDbContext>(
                           opt => opt.UseSqlServer(identityConnectionString,
                           b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName)));
                    });
                });



            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
            TestClient.BaseAddress = new Uri("https://localhost:5001");

            using var serviceScope = _serviceProvider.CreateScope();

            var dataContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            var identityContext = serviceScope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

            dataContext.Database.Migrate();
            identityContext.Database.Migrate();

            dataContext.Database.EnsureCreated();
            identityContext.Database.EnsureCreated();
        }


        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task<ApiResponse<CategoryVm>> CreateCategoryAsync(CreateCategoryCommand command)
        {
            var response = await TestClient.PostAsJsonAsync(Category.Create, command);
            return await response.Content.ReadAsAsync<ApiResponse<CategoryVm>>();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(Account.Authenticate, new AuthenticateCommand
            {
                Email = "john@gmail.com",
                Password = "Pwd12345!"
            });

            var content = await response.Content.ReadAsAsync<ApiResponse<AuthenticationResponse>>();
            return content.Data.Token;
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();

            var dataContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            var identityContext = serviceScope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

            dataContext.Database.EnsureDeleted();
            identityContext.Database.EnsureDeleted();
        }
    }
}
