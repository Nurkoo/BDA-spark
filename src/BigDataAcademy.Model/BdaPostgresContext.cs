using System.Text.RegularExpressions;
using BigDataAcademy.Model.Claim;
using BigDataAcademy.Model.Event;
using BigDataAcademy.Model.Exposure;
using BigDataAcademy.Model.Motor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BigDataAcademy.Model;

public class BdaPostgresContext : DbContext
{
    private static readonly Regex Tokenizer = new Regex(@"(?<!^)(?=[A-Z])", RegexOptions.Compiled);

    public BdaPostgresContext(DbContextOptions<BdaPostgresContext> options)
        : base(options)
    {
    }

    public DbSet<IntegrationEvent> Events => this.Set<IntegrationEvent>();

    public DbSet<ClaimHubMotor> ClaimHubMotors => this.Set<ClaimHubMotor>();

    public DbSet<ClaimHubClaim> ClaimHubClaims => this.Set<ClaimHubClaim>();

    public DbSet<ClaimHubExposure> ClaimHubExposures => this.Set<ClaimHubExposure>();

    public DbSet<InsureWaveMotor> InsureWaveMotors => this.Set<InsureWaveMotor>();

    public DbSet<InsureWaveClaim> InsureWaveClaims => this.Set<InsureWaveClaim>();

    public DbSet<InsureWaveExposure> InsureWaveExposures => this.Set<InsureWaveExposure>();

    public DbSet<ClaimZoneMotor> ClaimZoneMotors => this.Set<ClaimZoneMotor>();

    public DbSet<ClaimZoneClaim> ClaimZoneClaims => this.Set<ClaimZoneClaim>();

    public DbSet<ClaimZoneExposure> ClaimZoneExposures => this.Set<ClaimZoneExposure>();

    public DbSet<ClaimProClaim> ClaimProClaims => this.Set<ClaimProClaim>();

    public DbSet<ClaimProMotor> ClaimProMotors => this.Set<ClaimProMotor>();

    public DbSet<ClaimProExposure> ClaimProExposures => this.Set<ClaimProExposure>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tokens = Tokenizer.Split(entityType.ShortName()).Select(c => c.ToLower());
            entityType.SetTableName(string.Join("_", tokens));

            foreach (var property in entityType.GetProperties())
            {
                var propertyTokens = Tokenizer.Split(property.Name).Select(c => c.ToLower());
                property.SetColumnName(string.Join("_", propertyTokens));
            }
        }

        modelBuilder.Entity<IntegrationEvent>().HasKey(o => o.EventId);
        modelBuilder.Entity<IntegrationEvent>().Property(o => o.EventId).HasValueGenerator<SequentialGuidValueGenerator>();

        modelBuilder.Entity<ClaimHubMotor>().HasKey(o => o.MotorId);
        modelBuilder.Entity<ClaimHubMotor>().Property(o => o.MotorId).ValueGeneratedNever();

        modelBuilder.Entity<ClaimHubClaim>().HasKey(o => o.ClaimId);
        modelBuilder.Entity<ClaimHubClaim>().Property(o => o.ClaimId).ValueGeneratedNever();

        modelBuilder.Entity<ClaimHubExposure>().HasKey(o => o.ExposureId);
        modelBuilder.Entity<ClaimHubExposure>().Property(o => o.ExposureId).ValueGeneratedNever();

        modelBuilder.Entity<InsureWaveMotor>().HasKey(o => o.MotorId);
        modelBuilder.Entity<InsureWaveMotor>().Property(o => o.MotorId).ValueGeneratedNever();

        modelBuilder.Entity<InsureWaveClaim>().HasKey(o => o.ClaimId);
        modelBuilder.Entity<InsureWaveClaim>().Property(o => o.ClaimId).ValueGeneratedNever();
        modelBuilder.Entity<InsureWaveClaim>().HasMany<InsureWaveExposure>(o => o.Exposures).WithOne(o => o.Claim).HasForeignKey(o => o.ClaimId);

        modelBuilder.Entity<InsureWaveExposure>().HasKey(o => o.ExposureId);
        modelBuilder.Entity<InsureWaveExposure>().Property(o => o.ExposureId).ValueGeneratedNever();

        modelBuilder.Entity<ClaimZoneMotor>().HasKey(o => o.MotorId);
        modelBuilder.Entity<ClaimZoneMotor>().Property(o => o.MotorId).ValueGeneratedNever();

        modelBuilder.Entity<ClaimZoneClaim>().HasKey(o => o.ClaimId);
        modelBuilder.Entity<ClaimZoneClaim>().Property(o => o.ClaimId).ValueGeneratedNever();
        modelBuilder.Entity<ClaimZoneClaim>().HasMany<ClaimZoneExposure>(o => o.Exposures).WithOne(o => o.Claim).HasForeignKey(o => o.ClaimId);

        modelBuilder.Entity<ClaimZoneExposure>().HasKey(o => o.ExposureId);
        modelBuilder.Entity<ClaimZoneExposure>().Property(o => o.ExposureId).ValueGeneratedNever();

        modelBuilder.Entity<ClaimProMotor>();
        modelBuilder.Entity<ClaimProMotor>().HasKey(o => o.MotorId);
        modelBuilder.Entity<ClaimProMotor>().Property(o => o.MotorId).ValueGeneratedNever();

        modelBuilder.Entity<ClaimProClaim>();
        modelBuilder.Entity<ClaimProClaim>().HasKey(o => o.ClaimId);
        modelBuilder.Entity<ClaimProClaim>().Property(o => o.ClaimId).ValueGeneratedNever();

        modelBuilder.Entity<ClaimProExposure>();
        modelBuilder.Entity<ClaimProExposure>().HasKey(o => o.ExposureId);
        modelBuilder.Entity<ClaimProExposure>().Property(o => o.ExposureId).ValueGeneratedNever();
    }
}
