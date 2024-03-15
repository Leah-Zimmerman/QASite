using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QASite.Data
{
    public class StackOverflowContextFactory : IDesignTimeDbContextFactory<StackOverflowContext>
    {
        public StackOverflowContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}StackOverflow.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new StackOverflowContext(config.GetConnectionString("ConStr"));
        }
    }
}