﻿using Planner.Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Planner.Application.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    { }

    public virtual DbSet<User> Users { set; get; } = null!;
    public virtual DbSet<Role> Roles { set; get; } = null!;
    public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
    public virtual DbSet<Contractor> Contractors { get; set; } = null!;
    public virtual DbSet<Goal> Goals { get; set; } = null!;
    public virtual DbSet<GoalElapsedTimePart> GoalElapsedTimeParts { get; set; } = null!;
    public virtual DbSet<JobsNotes> JobsNotes { get; set; } = null!;
    public virtual DbSet<JobsNote> Jobs { get; set; } = null!;
    public virtual DbSet<UserSettings> UserSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // it should be placed here, otherwise it will rewrite the following settings!
        base.OnModelCreating(builder);

        // Custom application mappings
        builder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(450).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Password).IsRequired();
        });

        builder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(450).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.RoleId);
            entity.Property(e => e.UserId);
            entity.Property(e => e.RoleId);
            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasForeignKey(d => d.RoleId);
            entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId);
        });

        builder.Entity<Role>().HasData(
            new Role { Id = 1, Name = CustomRoles.User },
            new Role { Id = 2, Name = CustomRoles.Admin }
        );

        builder.Entity<Goal>(entity =>
        {
	        entity.Navigation(x => x.Contractor).AutoInclude();
	        entity.Navigation(x => x.User).AutoInclude();
        });

        builder.Entity<UserSettings>(entity =>
        {
	        entity.Navigation(x => x.User).AutoInclude();
        });


        builder.Entity<JobsNotes>(entity =>
        {
	        entity.Navigation(x => x.User).AutoInclude();
        });
	}
}
