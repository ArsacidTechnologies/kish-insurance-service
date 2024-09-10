﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using kish_insurance_service;

#nullable disable

namespace kish_insurance_service.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240910080244_UpdateCoverageTypeId")]
    partial class UpdateCoverageTypeId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("kish_insurance_service.Models.Coverage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Capital")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("CoverageTypeId")
                        .HasColumnType("int");

                    b.Property<int>("InsuranceRequestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CoverageTypeId");

                    b.HasIndex("InsuranceRequestId");

                    b.ToTable("Coverages");
                });

            modelBuilder.Entity("kish_insurance_service.Models.CoverageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("MaxCapital")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("MinCapital")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("PremiumRate")
                        .HasColumnType("decimal(5, 4)");

                    b.HasKey("Id");

                    b.ToTable("CoverageTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MaxCapital = 500000000m,
                            MinCapital = 5000m,
                            Name = "Surgery",
                            PremiumRate = 0.0052m
                        },
                        new
                        {
                            Id = 2,
                            MaxCapital = 400000000m,
                            MinCapital = 4000m,
                            Name = "Dental",
                            PremiumRate = 0.0042m
                        },
                        new
                        {
                            Id = 3,
                            MaxCapital = 200000000m,
                            MinCapital = 2000m,
                            Name = "Hospitalization",
                            PremiumRate = 0.005m
                        });
                });

            modelBuilder.Entity("kish_insurance_service.Models.InsuranceRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InsuranceRequests");
                });

            modelBuilder.Entity("kish_insurance_service.Models.Coverage", b =>
                {
                    b.HasOne("kish_insurance_service.Models.CoverageType", "CoverageType")
                        .WithMany()
                        .HasForeignKey("CoverageTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("kish_insurance_service.Models.InsuranceRequest", "InsuranceRequest")
                        .WithMany("Coverages")
                        .HasForeignKey("InsuranceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CoverageType");

                    b.Navigation("InsuranceRequest");
                });

            modelBuilder.Entity("kish_insurance_service.Models.InsuranceRequest", b =>
                {
                    b.Navigation("Coverages");
                });
#pragma warning restore 612, 618
        }
    }
}