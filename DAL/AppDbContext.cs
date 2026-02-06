using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Owner> Owners { get; set; } = null!;
    public DbSet<Pet> Pets { get; set; } = null!;
    public DbSet<Kennel> Kennels { get; set; } = null!;
    public DbSet<Stay> Stays { get; set; } = null!;
    public DbSet<Medication> Medications { get; set; } = null!;
    public DbSet<MedicationLog> MedicationLogs { get; set; } = null!;
    public DbSet<FeedingSchedule> FeedingSchedules { get; set; } = null!;
    public DbSet<Incompatibility> Incompatibilities { get; set; } = null!;
    public DbSet<PhotoUpdate> PhotoUpdates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Remove cascade delete
        foreach (var relationship in modelBuilder.Model
                     .GetEntityTypes()
                     .Where(e => !e.IsOwned())
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // Configure Owner
        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasIndex(e => e.Email);
        });

        // Configure Pet
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasIndex(e => e.Name);
            entity.HasOne(p => p.Owner)
                  .WithMany(o => o.Pets)
                  .HasForeignKey(p => p.OwnerId);
            entity.HasOne(p => p.Kennel)
                  .WithMany()
                  .HasForeignKey(p => p.KennelId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Kennel
        modelBuilder.Entity<Kennel>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();

            entity.Property(e => e.AdjacentKennelIds)
                  .HasConversion(
                      v => string.Join(',', v),
                      v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(Guid.Parse).ToList())
                  .Metadata.SetValueComparer(
                      new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<Guid>>(
                          (a, b) => a != null && b != null && a.SequenceEqual(b),
                          v => v.Aggregate(0, (acc, x) => HashCode.Combine(acc, x.GetHashCode())),
                          v => v.ToList()));

            // Explicitly configure the CurrentPet relationship
            entity.HasOne(k => k.CurrentPet)
                  .WithMany()
                  .HasForeignKey(k => k.CurrentPetId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Stay
        modelBuilder.Entity<Stay>(entity =>
        {
            entity.HasOne(s => s.Pet)
                  .WithMany(p => p.Stays)
                  .HasForeignKey(s => s.PetId);
            entity.HasOne(s => s.Kennel)
                  .WithMany(k => k.Stays)
                  .HasForeignKey(s => s.KennelId);
        });

        // Configure Medication
        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasOne(m => m.Pet)
                  .WithMany(p => p.Medications)
                  .HasForeignKey(m => m.PetId);
        });

        // Configure MedicationLog
        modelBuilder.Entity<MedicationLog>(entity =>
        {
            entity.HasOne(m => m.Medication)
                  .WithMany(m => m.Logs)
                  .HasForeignKey(m => m.MedicationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure FeedingSchedule
        modelBuilder.Entity<FeedingSchedule>(entity =>
        {
            entity.HasOne(f => f.Pet)
                  .WithMany(p => p.FeedingSchedules)
                  .HasForeignKey(f => f.PetId);
        });

        // Configure Incompatibility
        modelBuilder.Entity<Incompatibility>(entity =>
        {
            entity.HasOne(i => i.PetA)
                  .WithMany()
                  .HasForeignKey(i => i.PetAId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(i => i.PetB)
                  .WithMany()
                  .HasForeignKey(i => i.PetBId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PhotoUpdate
        modelBuilder.Entity<PhotoUpdate>(entity =>
        {
            entity.HasOne(p => p.Pet)
                  .WithMany(p => p.PhotoUpdates)
                  .HasForeignKey(p => p.PetId);
        });

        // Tier 1: no seeding (keeps migrations deterministic and repeatable)
    }

    public async Task SeedKennelsAsync()
    {
        if (await Kennels.AnyAsync())
            return;

        var kennels = new List<Kennel>();
        int kennelNumber = 1;

        // 5 Small kennels (5 cat)
        for (int i = 0; i < 5; i++)
        {
            kennels.Add(new Kennel
            {
                Name = $"{kennelNumber++}",
                Size = KennelSize.Small,
                Zone = KennelZone.Cat,
                Status = KennelStatus.Available
            });
        }

        // 5 Small kennels (5 dog)
        for (int i = 0; i < 5; i++)
        {
            kennels.Add(new Kennel
            {
                Name = $"{kennelNumber++}",
                Size = KennelSize.Small,
                Zone = KennelZone.Dog,
                Status = KennelStatus.Available
            });
        }

        // 7 Medium kennels (7 cat)
        for (int i = 0; i < 7; i++)
        {
            kennels.Add(new Kennel
            {
                Name = $"{kennelNumber++}",
                Size = KennelSize.Medium,
                Zone = KennelZone.Cat,
                Status = KennelStatus.Available
            });
        }

        // 8 Medium kennels (8 dog)
        for (int i = 0; i < 8; i++)
        {
            kennels.Add(new Kennel
            {
                Name = $"{kennelNumber++}",
                Size = KennelSize.Medium,
                Zone = KennelZone.Dog,
                Status = KennelStatus.Available
            });
        }

        // 1 Large kennel (1 cat)
        for (int i = 0; i < 1; i++)
        {
            kennels.Add(new Kennel
            {
                Name = $"{kennelNumber++}",
                Size = KennelSize.Large,
                Zone = KennelZone.Cat,
                Status = KennelStatus.Available
            });
        }

        // 4 Large kennels (4 dog)
        for (int i = 0; i < 4; i++)
        {
            kennels.Add(new Kennel
            {
                Name = $"{kennelNumber++}",
                Size = KennelSize.Large,
                Zone = KennelZone.Dog,
                Status = KennelStatus.Available
            });
        }

        Kennels.AddRange(kennels);
        await SaveChangesAsync();
    }

    public async Task SeedPetsAsync()
    {
        if (await Pets.AnyAsync())
            return;

        // First ensure we have owners
        if (!await Owners.AnyAsync())
        {
            var owners = new List<Owner>
            {
                new Owner { Name = "John Smith", Email = "john@email.com" },
                new Owner { Name = "Jane Doe", Email = "jane@email.com" }
            };
            Owners.AddRange(owners);
            await SaveChangesAsync();
        }

        var owner1 = await Owners.FirstAsync(o => o.Email == "john@email.com");
        var owner2 = await Owners.FirstAsync(o => o.Email == "jane@email.com");

        var pets = new List<Pet>
        {
            new Pet
            {
                Name = "Buddy",
                Species = PetSpecies.Dog,
                Size = PetSize.Medium,
                OwnerId = owner1.Id,
                KennelId = null,
                Notes = "Friendly dog"
            },
            new Pet
            {
                Name = "Whiskers",
                Species = PetSpecies.Cat,
                Size = PetSize.Small,
                OwnerId = owner2.Id,
                KennelId = null,
                Notes = "Loves to play"
            }
        };

        Pets.AddRange(pets);
        await SaveChangesAsync();
    }

}
