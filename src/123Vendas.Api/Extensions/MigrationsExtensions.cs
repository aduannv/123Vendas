using _123Vendas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace _123Vendas.Api.Extensions
{
    public static class MigrationExtensions 
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SalesDbContext>();

            int retries = 5;
            int delay = 2000;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    dbContext.Database.Migrate();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Try {i + 1} failed: {ex.Message}");
                    Thread.Sleep(delay);
                }
            }

            throw new Exception("Failed to connect to database, please stop and run again.");
        }
    }
}
