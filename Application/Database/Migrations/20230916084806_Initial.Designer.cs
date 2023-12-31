﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Planner.Application.Database;

#nullable disable

namespace Planner.Application.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230916084806_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Planner.Application.Models.Contractor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_contractors");

                    b.ToTable("contractors", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.Goal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("comment");

                    b.Property<int?>("ContractorId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("contractor_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<int>("CurrentElapsedTimePartId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("current_elapsed_time_part_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_goals");

                    b.HasIndex("ContractorId")
                        .HasDatabaseName("ix_goals_contractor_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_goals_user_id");

                    b.ToTable("goals", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.GoalElapsedTimePart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<bool>("Collapsed")
                        .HasColumnType("INTEGER")
                        .HasColumnName("collapsed");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<TimeSpan>("ElapsedTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("elapsed_time");

                    b.Property<int>("GoalId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("goal_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_goal_elapsed_time_parts");

                    b.HasIndex("GoalId")
                        .HasDatabaseName("ix_goal_elapsed_time_parts_goal_id");

                    b.ToTable("goal_elapsed_time_parts", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.JobsNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("comment");

                    b.Property<bool>("Completed")
                        .HasColumnType("INTEGER")
                        .HasColumnName("completed");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<int?>("JobsNotesId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("jobs_notes_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("TEXT")
                        .HasColumnName("time");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_jobs");

                    b.HasIndex("JobsNotesId")
                        .HasDatabaseName("ix_jobs_jobs_notes_id");

                    b.ToTable("jobs", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.JobsNotes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT")
                        .HasColumnName("date");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_jobs_notes");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_jobs_notes_user_id");

                    b.ToTable("jobs_notes", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_roles_name");

                    b.ToTable("roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "User"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Planner.Application.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<DateTime?>("EmailVerifiedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("email_verified_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.Property<string>("RememberToken")
                        .HasColumnType("TEXT")
                        .HasColumnName("remember_token");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_user_roles_role_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_roles_user_id");

                    b.ToTable("user_roles", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_at");

                    b.Property<string>("DetailedTimeFormatter")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("detailed_time_formatter");

                    b.Property<string>("TimeFormatter")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("time_formatter");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_at");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_settings");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_settings_user_id");

                    b.ToTable("user_settings", (string)null);
                });

            modelBuilder.Entity("Planner.Application.Models.Goal", b =>
                {
                    b.HasOne("Planner.Application.Models.Contractor", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorId")
                        .HasConstraintName("fk_goals_contractors_contractor_id");

                    b.HasOne("Planner.Application.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_goals_users_user_id");

                    b.Navigation("Contractor");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Planner.Application.Models.GoalElapsedTimePart", b =>
                {
                    b.HasOne("Planner.Application.Models.Goal", "Goal")
                        .WithMany("ElapsedTimeParts")
                        .HasForeignKey("GoalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_goal_elapsed_time_parts_goals_goal_id");

                    b.Navigation("Goal");
                });

            modelBuilder.Entity("Planner.Application.Models.JobsNote", b =>
                {
                    b.HasOne("Planner.Application.Models.JobsNotes", null)
                        .WithMany("Notes")
                        .HasForeignKey("JobsNotesId")
                        .HasConstraintName("fk_jobs_jobs_notes_jobs_notes_id");
                });

            modelBuilder.Entity("Planner.Application.Models.JobsNotes", b =>
                {
                    b.HasOne("Planner.Application.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_jobs_notes_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Planner.Application.Models.UserRole", b =>
                {
                    b.HasOne("Planner.Application.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_roles_role_id");

                    b.HasOne("Planner.Application.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_user_id");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Planner.Application.Models.UserSettings", b =>
                {
                    b.HasOne("Planner.Application.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_settings_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Planner.Application.Models.Goal", b =>
                {
                    b.Navigation("ElapsedTimeParts");
                });

            modelBuilder.Entity("Planner.Application.Models.JobsNotes", b =>
                {
                    b.Navigation("Notes");
                });

            modelBuilder.Entity("Planner.Application.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Planner.Application.Models.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
