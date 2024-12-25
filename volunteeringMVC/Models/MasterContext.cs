using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace volunteeringMVC.Models;

public partial class MasterContext : DbContext
{
    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<Register> Registers { get; set; }

    public virtual DbSet<VolunteeringPost> VolunteeringPosts { get; set; }

    public virtual DbSet<VolunteeringUser> VolunteeringUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    => optionsBuilder.UseSqlServer(

               WebApplication.CreateBuilder().Configuration.GetConnectionString("DB1")

);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complain__3213E83F9D9EFADB");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ComplaintType).HasMaxLength(100);

            entity.HasOne(d => d.Post).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Complaints_Post");
        });

        modelBuilder.Entity<Register>(entity =>
        {
            entity.ToTable("register");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FamilyName)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("familyName");
            entity.Property(e => e.FirstName)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<VolunteeringPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Voluntee__3214EC070F7686B9");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.OrgName).HasMaxLength(100);
            entity.Property(e => e.PostAdminEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<VolunteeringUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PostId });

            entity.Property(e => e.VolunteeredAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.VolunteeringUsers)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VolunteeringUsers_Posts");

            entity.HasOne(d => d.User).WithMany(p => p.VolunteeringUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VolunteeringUsers_Register");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
