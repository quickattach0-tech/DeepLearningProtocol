using Microsoft.EntityFrameworkCore;
using System;

namespace DeepLearningProtocol
{
    /// <summary>
    /// ProtocolDbContext manages the database connection and command definitions.
    /// Provides Entity Framework integration for persistent command storage.
    /// </summary>
    public class ProtocolDbContext : DbContext
    {
        /// <summary>DbSet for CommandDefinitions table</summary>
        public DbSet<CommandDefinition> CommandDefinitions { get; set; }

    /// <summary>DbSet for TranslationRules table</summary>
    public DbSet<TranslationRule> TranslationRules { get; set; }

    /// <summary>DbSet for TranslatedTexts table</summary>
    public DbSet<TranslatedText> TranslatedTexts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use LocalDB for local development, configurable for production
            var connectionString = Environment.GetEnvironmentVariable("DLP_CONNECTION_STRING") 
                ?? @"Server=(localdb)\mssqllocaldb;Database=DeepLearningProtocol;Trusted_Connection=true;";
            
            optionsBuilder.UseSqlServer(connectionString);
        }

        /// <summary>Configures model relationships and constraints</summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure CommandDefinition entity
            modelBuilder.Entity<CommandDefinition>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CommandName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CommandPattern)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .HasDefaultValue("Protocol");

                entity.Property(e => e.Parameters)
                    .HasMaxLength(1000)
                    .HasDefaultValue("{}");

                entity.Property(e => e.LastExecutionResult)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Index for fast command lookup
                entity.HasIndex(e => e.CommandName)
                    .IsUnique();

                // Index for category filtering
                entity.HasIndex(e => e.Category);

                // Index for enabled commands
                entity.HasIndex(e => e.IsEnabled);
            });

            // Configure TranslationRule entity
            modelBuilder.Entity<TranslationRule>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SourceText)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SpanishTranslation)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ArabicTranslation)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FrenchTranslation)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .HasDefaultValue("Custom");

                entity.Property(e => e.Priority)
                    .HasDefaultValue(5);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Index for active rules
                entity.HasIndex(e => e.IsActive);

                // Index for priority (rules checked by priority)
                entity.HasIndex(e => e.Priority).IsDescending();
            });

            // Configure TranslatedText entity
            modelBuilder.Entity<TranslatedText>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SourceText)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.SpanishTranslation)
                    .HasMaxLength(2000);

                entity.Property(e => e.ArabicTranslation)
                    .HasMaxLength(2000);

                entity.Property(e => e.FrenchTranslation)
                    .HasMaxLength(2000);

                entity.Property(e => e.Notes)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Index for verified translations
                entity.HasIndex(e => e.IsManuallyVerified);

                // Index for quality score
                entity.HasIndex(e => e.QualityScore);
            });
        }
    }
}
