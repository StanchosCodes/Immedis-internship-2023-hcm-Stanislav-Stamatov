using System;
using System.Collections.Generic;
using HumanCapitalManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanCapitalManagement.Data;

public partial class HumanCapitalManagementContext : DbContext
{
    public HumanCapitalManagementContext()
    {
    }

    public HumanCapitalManagementContext(DbContextOptions<HumanCapitalManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Town> Towns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-LMGD5FU\\SQLEXPRESS;Database=HumanCapitalManagement;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC076C6CECA6");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(30);

            entity.HasOne(d => d.Manager).WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Departments_Employees");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0757BF0981");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(60);
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.JobTitle).HasMaxLength(60);
            entity.Property(e => e.LastName).HasMaxLength(30);
            entity.Property(e => e.MiddleName).HasMaxLength(30);
            entity.Property(e => e.PasswordHash).HasMaxLength(1);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Departments");

            entity.HasOne(d => d.Manager).WithMany(p => p.Employees)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Employees");

            entity.HasOne(d => d.Town).WithMany(p => p.Employees)
                .HasForeignKey(d => d.TownId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__TownI__3D5E1FD2");

            entity.HasMany(d => d.Projects).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeesProject",
                    r => r.HasOne<Project>().WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Employees__Proje__440B1D61"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Employees__Emplo__4316F928"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "ProjectId").HasName("PK__Employee__6DB1E4FEA3BB51A8");
                        j.ToTable("EmployeesProjects");
                    });

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsersRole__RoleI__4AB81AF0"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsersRole__UserI__49C3F6B7"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UsersRol__AF2760AD21D56ACD");
                        j.ToTable("UsersRoles");
                    });
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC079675A7D0");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasMaxLength(80);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07B18B65DC");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<Town>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Towns__3214EC0760909871");

            entity.Property(e => e.Name).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
