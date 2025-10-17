using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StacktimApi.Model;

namespace StacktimApi.Data;

public partial class StacktimDbContext : DbContext
{
    public StacktimDbContext()
    {
    }

    public StacktimDbContext(DbContextOptions<StacktimDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=StacktimDb;integrated Security=true;TrustServerCertificate=true;");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Players__3214EC272917A464");

            entity.HasIndex(e => e.Email, "UQ__Players__161CF724CF7904FB").IsUnique();

            entity.HasIndex(e => e.Pseudo, "UQ__Players__6087BA7B7AF126FE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Pseudo)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("PSEUDO");
            entity.Property(e => e.Rank)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalScore).HasDefaultValue(0);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teams__3214EC27F76A0C40");

            entity.HasIndex(e => e.Name, "UQ__Teams__737584F65F05D552").IsUnique();

            entity.HasIndex(e => e.Tag, "UQ__Teams__C4516413ED41D026").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tag)
                .HasMaxLength(3)
                .IsUnicode(false);

            entity.HasOne(d => d.Captain).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CaptainId)
                .HasConstraintName("FK_Teams_Players");
        });

        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Player).WithMany()
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("FK_TeamPlayers_Players");

            entity.HasOne(d => d.Team).WithMany()
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_TeamPlayers_Teams");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
