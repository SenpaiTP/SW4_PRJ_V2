﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRJ4.Data;

#nullable disable

namespace PRJ4.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.ApiUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.Bruger", b =>
                {
                    b.Property<string>("BrugerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Efternavn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fornavn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BrugerId");

                    b.ToTable("Brugers", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.Budget", b =>
                {
                    b.Property<int>("BudgetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BudgetId"));

                    b.Property<string>("BrugerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BrugerId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BudgetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("BudgetSlut")
                        .HasColumnType("date");

                    b.Property<DateOnly>("BudgetStart")
                        .HasColumnType("date");

                    b.Property<int>("SavingsGoal")
                        .HasColumnType("int");

                    b.HasKey("BudgetId");

                    b.HasIndex("BrugerId");

                    b.HasIndex("BrugerId1");

                    b.ToTable("Budgets", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.Findtægt", b =>
                {
                    b.Property<int>("FindtægtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FindtægtId"));

                    b.Property<string>("BrugerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Dato")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Indtægt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("KategoriId")
                        .HasColumnType("int");

                    b.Property<string>("Tekst")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FindtægtId");

                    b.HasIndex("BrugerId");

                    b.HasIndex("KategoriId");

                    b.ToTable("Findtægter", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.Fudgifter", b =>
                {
                    b.Property<int>("FudgiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FudgiftId"));

                    b.Property<string>("BrugerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Dato")
                        .HasColumnType("datetime2");

                    b.Property<int>("KategoriId")
                        .HasColumnType("int");

                    b.Property<decimal>("Pris")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Tekst")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FudgiftId");

                    b.HasIndex("BrugerId");

                    b.HasIndex("KategoriId");

                    b.ToTable("Fudgifters", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.Kategori", b =>
                {
                    b.Property<int>("KategoriId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KategoriId"));

                    b.Property<string>("KategoriNavn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KategoriId");

                    b.ToTable("Kategorier", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.KategoryLimit", b =>
                {
                    b.Property<int>("KategoryLimitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KategoryLimitId"));

                    b.Property<string>("BrugerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("KategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Limit")
                        .HasColumnType("int");

                    b.HasKey("KategoryLimitId");

                    b.HasIndex("BrugerId");

                    b.HasIndex("KategoryId")
                        .IsUnique();

                    b.ToTable("KategoryLimits", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.LoginModel", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LoginId");

                    b.ToTable("LoginModels", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.Saving", b =>
                {
                    b.Property<int>("SavingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SavingId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("SavingId");

                    b.HasIndex("BudgetId");

                    b.ToTable("Savings");
                });

            modelBuilder.Entity("PRJ4.Models.Vindtægt", b =>
                {
                    b.Property<int>("VindtægtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VindtægtId"));

                    b.Property<string>("BrugerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Dato")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Indtægt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("KategoriId")
                        .HasColumnType("int");

                    b.Property<string>("Tekst")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VindtægtId");

                    b.HasIndex("BrugerId");

                    b.HasIndex("KategoriId");

                    b.ToTable("Vindtægter", (string)null);
                });

            modelBuilder.Entity("PRJ4.Models.Vudgifter", b =>
                {
                    b.Property<int>("VudgiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VudgiftId"));

                    b.Property<string>("BrugerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Dato")
                        .HasColumnType("datetime2");

                    b.Property<int?>("KategoriId")
                        .HasColumnType("int");

                    b.Property<decimal>("Pris")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Tekst")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VudgiftId");

                    b.HasIndex("BrugerId");

                    b.HasIndex("KategoriId");

                    b.ToTable("Vudgifters", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRJ4.Models.ApiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PRJ4.Models.Budget", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", "Bruger")
                        .WithMany("Budgets")
                        .HasForeignKey("BrugerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRJ4.Models.Bruger", null)
                        .WithMany("Budgets")
                        .HasForeignKey("BrugerId1");

                    b.Navigation("Bruger");
                });

            modelBuilder.Entity("PRJ4.Models.Findtægt", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", "Bruger")
                        .WithMany("Findtægter")
                        .HasForeignKey("BrugerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRJ4.Models.Kategori", "Kategori")
                        .WithMany()
                        .HasForeignKey("KategoriId");

                    b.Navigation("Bruger");

                    b.Navigation("Kategori");
                });

            modelBuilder.Entity("PRJ4.Models.Fudgifter", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", "Bruger")
                        .WithMany("Fudgifters")
                        .HasForeignKey("BrugerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRJ4.Models.Kategori", "Kategori")
                        .WithMany()
                        .HasForeignKey("KategoriId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bruger");

                    b.Navigation("Kategori");
                });

            modelBuilder.Entity("PRJ4.Models.KategoryLimit", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", "Bruger")
                        .WithMany()
                        .HasForeignKey("BrugerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRJ4.Models.Kategori", "Kategory")
                        .WithOne("KategoryLimit")
                        .HasForeignKey("PRJ4.Models.KategoryLimit", "KategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bruger");

                    b.Navigation("Kategory");
                });

            modelBuilder.Entity("PRJ4.Models.Saving", b =>
                {
                    b.HasOne("PRJ4.Models.Budget", "Budget")
                        .WithMany("Savings")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("PRJ4.Models.Vindtægt", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", "Bruger")
                        .WithMany("Vindtægter")
                        .HasForeignKey("BrugerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRJ4.Models.Kategori", "Kategori")
                        .WithMany()
                        .HasForeignKey("KategoriId");

                    b.Navigation("Bruger");

                    b.Navigation("Kategori");
                });

            modelBuilder.Entity("PRJ4.Models.Vudgifter", b =>
                {
                    b.HasOne("PRJ4.Models.ApiUser", "Bruger")
                        .WithMany("Vudgifters")
                        .HasForeignKey("BrugerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRJ4.Models.Kategori", "Kategori")
                        .WithMany()
                        .HasForeignKey("KategoriId");

                    b.Navigation("Bruger");

                    b.Navigation("Kategori");
                });

            modelBuilder.Entity("PRJ4.Models.ApiUser", b =>
                {
                    b.Navigation("Budgets");

                    b.Navigation("Findtægter");

                    b.Navigation("Fudgifters");

                    b.Navigation("Vindtægter");

                    b.Navigation("Vudgifters");
                });

            modelBuilder.Entity("PRJ4.Models.Bruger", b =>
                {
                    b.Navigation("Budgets");
                });

            modelBuilder.Entity("PRJ4.Models.Budget", b =>
                {
                    b.Navigation("Savings");
                });

            modelBuilder.Entity("PRJ4.Models.Kategori", b =>
                {
                    b.Navigation("KategoryLimit");
                });
#pragma warning restore 612, 618
        }
    }
}
