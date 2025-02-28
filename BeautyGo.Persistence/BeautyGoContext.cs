using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace BeautyGo.Persistence;

public sealed class BeautyGoContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionStringSettings _connectionSettings;

    #region Ctor

    public BeautyGoContext(
        DbContextOptions<BeautyGoContext> options,
        AppSettings appSettings,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        _connectionSettings = appSettings.Get<ConnectionStringSettings>();
    }

    #endregion

    #region Overrides 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString(_connectionSettings.SettingsKey));
        }
    }

    #endregion
}
