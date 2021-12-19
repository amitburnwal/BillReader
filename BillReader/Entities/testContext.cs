using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BillReader.Entities
{
    public partial class testContext : DbContext
    {
        public testContext()
        {
        }

        public testContext(DbContextOptions<testContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MeterReading> MeterReadings { get; set; }
        public virtual DbSet<TestAccount> TestAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                //optionsBuilder.UseSqlServer("Data Source=DESKTOP-57S1AP9\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            

            modelBuilder.Entity<MeterReading>(entity =>
            {
                entity.ToTable("MeterReading");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasMaxLength(8);

                entity.Property(e => e.MeterReadDt)
                    .HasColumnType("datetime")
                    .HasColumnName("MeterReadDT");

                entity.Property(e => e.MeterReadValue).HasMaxLength(5);
            });

            modelBuilder.Entity<TestAccount>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Test_Accounts");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
