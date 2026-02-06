using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            homeDirectory = homeDirectory + System.IO.Path.DirectorySeparatorChar;

            var connectionString = $"Data Source={homeDirectory}eksamC#.db";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite(connectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}