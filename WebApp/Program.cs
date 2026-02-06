using DAL;
using Microsoft.EntityFrameworkCore;
using AppDbContext = DAL.AppDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
homeDirectory += Path.DirectorySeparatorChar;

connectionString = connectionString.Replace("<db_file>", $"{homeDirectory}eksamC#.db");

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    await context.SeedKennelsAsync();
    await context.SeedPetsAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();
