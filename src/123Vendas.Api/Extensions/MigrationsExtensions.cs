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
            dbContext.Database.Migrate();
        }
    }
}
