using Infrastructure.AppEnvironment;
using Infrastructure.Entities.Pills;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApplicationDatabase.Context;

/// <summary>
/// Контекст базы данных
/// </summary>
public class ApplicationContext : DbContext
{
    /// <summary>
    /// Таблица с информацией о лекарствах
    /// </summary>
    public DbSet<Pill> Pills { get; set; } = null!;

    /// <summary>
    /// Настройки приложения
    /// </summary>
    private readonly AppSettings _appSettings;

    public ApplicationContext(IOptions<AppSettings> appSettingsOptions)
    {
        _appSettings = appSettingsOptions.Value;
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_appSettings.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        PillModelBuilder(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void PillModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pill>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Link)
                .IsRequired();

            entity.Property(e => e.Description)
                .IsRequired();
        });
    }
}
