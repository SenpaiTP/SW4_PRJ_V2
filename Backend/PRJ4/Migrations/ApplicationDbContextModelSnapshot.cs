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

            modelBuilder.Entity("PRJ4.Models.Bruger", b =>
                {
                    b.Property<int>("BrugerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrugerId"));

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

                    b.ToTable("Brugers");
                });

            modelBuilder.Entity("PRJ4.Models.Budget", b =>
                {
                    b.Property<int>("BudgetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BudgetId"));

                    b.Property<int>("BrugerId")
                        .HasColumnType("int");

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

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("PRJ4.Models.Fudgifter", b =>
                {
                    b.Property<int>("FudgiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FudgiftId"));

                    b.Property<int>("BrugerId")
                        .HasColumnType("int");

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

                    b.ToTable("Fudgifters");
                });

            modelBuilder.Entity("PRJ4.Models.Kategori", b =>
                {
                    b.Property<int>("KategoriId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KategoriId"));

                    b.Property<string>("Navn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("KategoriId");

                    b.ToTable("Kategorier");
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

                    b.ToTable("LoginModels");
                });

            modelBuilder.Entity("PRJ4.Models.Vudgifter", b =>
                {
                    b.Property<int>("VudgiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VudgiftId"));

                    b.Property<int>("BrugerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Dato")
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

                    b.ToTable("Vudgifters");
                });

            modelBuilder.Entity("PRJ4.Models.Budget", b =>
                {
                    b.HasOne("PRJ4.Models.Bruger", "Bruger")
                        .WithMany("Budgets")
                        .HasForeignKey("BrugerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bruger");
                });

            modelBuilder.Entity("PRJ4.Models.Fudgifter", b =>
                {
                    b.HasOne("PRJ4.Models.Bruger", "Bruger")
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

            modelBuilder.Entity("PRJ4.Models.Vudgifter", b =>
                {
                    b.HasOne("PRJ4.Models.Bruger", "Bruger")
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

            modelBuilder.Entity("PRJ4.Models.Bruger", b =>
                {
                    b.Navigation("Budgets");

                    b.Navigation("Fudgifters");

                    b.Navigation("Vudgifters");
                });
#pragma warning restore 612, 618
        }
    }
}
