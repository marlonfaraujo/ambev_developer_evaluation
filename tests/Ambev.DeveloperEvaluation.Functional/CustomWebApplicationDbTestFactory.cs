using Ambev.DeveloperEvaluation.NoSql;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Functional
{
    public class CustomWebApplicationDbTestFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<DefaultContext>(options =>
                {
                    options.UseNpgsql("Server=localhost:5432;Database=DbTest;User Id=developer;Password=ev@luAt10n");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
                context.Database.EnsureCreated();
                //context.Database.Migrate();


                services.AddSingleton<MongoDbContext>(sp =>
                    new MongoDbContext(
                        "mongodb://developer:ev%40luAt10n@localhost:27017/?authSource=admin",
                        "test_developer_evaluation"
                    ));
            });
        }
    }
}
