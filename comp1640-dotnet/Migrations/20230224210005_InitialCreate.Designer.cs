﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using comp1640_dotnet.Data;

#nullable disable

namespace comp1640_dotnet.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230224210005_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("comp1640_dotnet.Models.AcademicYear", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ClosureDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FinalClosureDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("AcademicYears");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdeaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsAnonymous")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Department", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Document", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocumentUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdeaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Idea", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AcademicYearId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAnonymous")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AcademicYearId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("UserId");

                    b.ToTable("Ideas");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Profile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Reaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdeaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId");

                    b.HasIndex("UserId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.RefreshToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Comment", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.Idea", "Idea")
                        .WithMany("Comments")
                        .HasForeignKey("IdeaId");

                    b.HasOne("comp1640_dotnet.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");

                    b.Navigation("Idea");

                    b.Navigation("User");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Document", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.Idea", "Idea")
                        .WithMany("Documents")
                        .HasForeignKey("IdeaId");

                    b.Navigation("Idea");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Idea", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.AcademicYear", "AcademicYear")
                        .WithMany("Ideas")
                        .HasForeignKey("AcademicYearId");

                    b.HasOne("comp1640_dotnet.Models.Category", "Category")
                        .WithMany("Ideas")
                        .HasForeignKey("CategoryId");

                    b.HasOne("comp1640_dotnet.Models.Department", "Department")
                        .WithMany("Ideas")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("comp1640_dotnet.Models.User", "User")
                        .WithMany("Ideas")
                        .HasForeignKey("UserId");

                    b.Navigation("AcademicYear");

                    b.Navigation("Category");

                    b.Navigation("Department");

                    b.Navigation("User");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Profile", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("comp1640_dotnet.Models.Profile", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Reaction", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.Idea", "Idea")
                        .WithMany("Reactions")
                        .HasForeignKey("IdeaId");

                    b.HasOne("comp1640_dotnet.Models.User", "User")
                        .WithMany("Reactions")
                        .HasForeignKey("UserId");

                    b.Navigation("Idea");

                    b.Navigation("User");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.RefreshToken", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("comp1640_dotnet.Models.RefreshToken", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.User", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.UserRole", b =>
                {
                    b.HasOne("comp1640_dotnet.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("comp1640_dotnet.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.AcademicYear", b =>
                {
                    b.Navigation("Ideas");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Category", b =>
                {
                    b.Navigation("Ideas");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Department", b =>
                {
                    b.Navigation("Ideas");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.Idea", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Documents");

                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("comp1640_dotnet.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Ideas");

                    b.Navigation("Profile");

                    b.Navigation("Reactions");

                    b.Navigation("RefreshToken");
                });
#pragma warning restore 612, 618
        }
    }
}
