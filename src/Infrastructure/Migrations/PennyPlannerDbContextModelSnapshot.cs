﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(PennyPlannerDbContext))]
    partial class PennyPlannerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Entities.EmailMessageOutboxEntity", b =>
                {
                    b.Property<Guid>("EmailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailBody")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailSubject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsProcessed")
                        .HasColumnType("boolean");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EmailId");

                    b.ToTable("EmailOutbox", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Entities.LoginEntity", b =>
                {
                    b.Property<Guid>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RevokedBy")
                        .HasColumnType("text");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginId");

                    b.HasIndex("UserId");

                    b.ToTable("Logins", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Entities.OneTimePasscodeEntity", b =>
                {
                    b.Property<Guid>("OneTimePasscodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("Passcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("OneTimePasscodeId");

                    b.HasIndex("UserId");

                    b.ToTable("OneTimePasscodes", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Entities.PasswordResetEntity", b =>
                {
                    b.Property<Guid>("PasswordResetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("ResetToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("PasswordResetId");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordResets", (string)null);
                });

            modelBuilder.Entity("Infrastructure.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsTwoFactorAuthenticationEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserRoleId")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.HasIndex("UserRoleId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("f71d20e7-8880-4202-ad50-9ee88ac4e7d1"),
                            CreatedAt = new DateTime(2024, 9, 4, 19, 15, 44, 324, DateTimeKind.Utc).AddTicks(7272),
                            CreatedBy = "System",
                            EmailAddress = "admin@admin.com",
                            IsDeleted = false,
                            IsTwoFactorAuthenticationEnabled = false,
                            PasswordHash = "",
                            PasswordSalt = "",
                            RowVersion = 0u,
                            UpdatedAt = new DateTime(2024, 9, 4, 19, 15, 44, 324, DateTimeKind.Utc).AddTicks(7274),
                            UpdatedBy = "System",
                            UserRoleId = 2,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Infrastructure.Entities.UserRoleEntity", b =>
                {
                    b.Property<int>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserRoleId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserRoleId");

                    b.ToTable("UserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserRoleId = 1,
                            CreatedAt = new DateTime(2024, 9, 4, 19, 15, 44, 324, DateTimeKind.Utc).AddTicks(3168),
                            CreatedBy = "System",
                            Name = "User",
                            RowVersion = 0u,
                            UpdatedAt = new DateTime(2024, 9, 4, 19, 15, 44, 324, DateTimeKind.Utc).AddTicks(3502),
                            UpdatedBy = "System"
                        },
                        new
                        {
                            UserRoleId = 2,
                            CreatedAt = new DateTime(2024, 9, 4, 19, 15, 44, 324, DateTimeKind.Utc).AddTicks(3941),
                            CreatedBy = "System",
                            Name = "Admin",
                            RowVersion = 0u,
                            UpdatedAt = new DateTime(2024, 9, 4, 19, 15, 44, 324, DateTimeKind.Utc).AddTicks(3942),
                            UpdatedBy = "System"
                        });
                });

            modelBuilder.Entity("Infrastructure.Entities.LoginEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.UserEntity", "UserEntity")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserEntity");
                });

            modelBuilder.Entity("Infrastructure.Entities.OneTimePasscodeEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.UserEntity", "UserEntity")
                        .WithMany("OneTimePasscodes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserEntity");
                });

            modelBuilder.Entity("Infrastructure.Entities.PasswordResetEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.UserEntity", "UserEntity")
                        .WithMany("PasswordResets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserEntity");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserEntity", b =>
                {
                    b.HasOne("Infrastructure.Entities.UserRoleEntity", "UserRoleEntity")
                        .WithMany("Users")
                        .HasForeignKey("UserRoleId")
                        .IsRequired();

                    b.Navigation("UserRoleEntity");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserEntity", b =>
                {
                    b.Navigation("Logins");

                    b.Navigation("OneTimePasscodes");

                    b.Navigation("PasswordResets");
                });

            modelBuilder.Entity("Infrastructure.Entities.UserRoleEntity", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
