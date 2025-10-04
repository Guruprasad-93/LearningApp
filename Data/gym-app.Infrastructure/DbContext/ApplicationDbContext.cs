using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace gym_app.Domain.Entities;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CustomerTrainer> CustomerTrainers { get; set; }

    public virtual DbSet<Dietplan> Dietplans { get; set; }

    public virtual DbSet<GymType> GymTypes { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLoginToken> UserLoginTokens { get; set; }

    public virtual DbSet<UserPhoto> UserPhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__2D971CAC0505D556");

            entity.ToTable("Company");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CompOwnerName).HasMaxLength(100);
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.PhotoPath).HasMaxLength(255);
        });

        modelBuilder.Entity<CustomerTrainer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0748BA1D04");

            entity.ToTable("CustomerTrainer");

            entity.HasOne(d => d.Company).WithMany(p => p.CustomerTrainers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerT__Compa__18EBB532");

            entity.HasOne(d => d.Trainer).WithMany(p => p.CustomerTrainers)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("FK__CustomerT__Train__17036CC0");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerTrainers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__CustomerT__UserI__160F4887");
        });

        modelBuilder.Entity<Dietplan>(entity =>
        {
            entity.HasKey(e => e.DietplanId).HasName("PK__Dietplan__2DC96D3F18D4E256");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);

            entity.HasOne(d => d.Company).WithMany(p => p.Dietplans)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dietplans__Compa__1332DBDC");

            entity.HasOne(d => d.User).WithMany(p => p.Dietplans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Dietplans__UserI__114A936A");
        });

        modelBuilder.Entity<GymType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__GymType__516F03B560A20C0D");

            entity.ToTable("GymType");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.TypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A786792188DA4B");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Compa__25518C17");

            entity.HasOne(d => d.User).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Membershi__UserI__22751F6C");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF87C7E608314");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Review).HasMaxLength(500);

            entity.HasOne(d => d.Company).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ratings__Company__1F98B2C1");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Ratings__UserId__1CBC4616");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AC1B18172");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.RoleCode).HasMaxLength(100);
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.TrainerId).HasName("PK__Trainers__366A1A7C3C78BA78");

            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Speciality).HasMaxLength(100);

            entity.HasOne(d => d.Company).WithMany(p => p.Trainers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Trainers__Compan__7E37BEF6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C87EE5187");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Company).WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__CompanyId__693CA210");
        });

        modelBuilder.Entity<UserLoginToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__UserLogi__658FEEEA105F0BDF");

            entity.ToTable("UserLoginToken");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Expires).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RefreshToken).HasMaxLength(255);
            entity.Property(e => e.Revoked).HasColumnType("datetime");
            entity.Property(e => e.SessionId).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Company).WithMany(p => p.UserLoginTokens)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserLogin__Compa__46B27FE2");

            entity.HasOne(d => d.User).WithMany(p => p.UserLoginTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserLogin__UserI__42E1EEFE");
        });

        modelBuilder.Entity<UserPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserPhot__3214EC070F8E9EB0");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PhotoPath).HasMaxLength(255);

            entity.HasOne(d => d.Company).WithMany(p => p.UserPhotos)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPhoto__Compa__2A164134");

            entity.HasOne(d => d.User).WithMany(p => p.UserPhotos)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserPhoto__UserI__282DF8C2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
