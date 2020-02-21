﻿// <auto-generated />
using System;
using AuditService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuditService.Migrations
{
    [DbContext(typeof(AuditContext))]
    [Migration("20200221120751_AddApplication")]
    partial class AddApplication
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AuditService.Infrastructure.Idempotency.ClientRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("ClientRequests");
                });

            modelBuilder.Entity("AuditService.Model.Admin.AuditApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ApplicationID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AuditApplications");
                });

            modelBuilder.Entity("AuditService.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AuditID")
                        .HasColumnType("int");

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<int?>("AuditApplicationId")
                        .HasColumnType("int");

                    b.Property<int>("AuditLevelId")
                        .HasColumnType("int");

                    b.Property<int>("AuditTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Details")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Source")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AuditApplicationId");

                    b.HasIndex("AuditLevelId");

                    b.HasIndex("AuditTypeId");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("AuditService.Models.AuditLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AuditLevelID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AuditLevels");
                });

            modelBuilder.Entity("AuditService.Models.AuditType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AuditTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AuditTypes");
                });

            modelBuilder.Entity("AuditService.Models.Audit", b =>
                {
                    b.HasOne("AuditService.Model.Admin.AuditApplication", "AuditApplication")
                        .WithMany()
                        .HasForeignKey("AuditApplicationId");

                    b.HasOne("AuditService.Models.AuditLevel", "AuditLevel")
                        .WithMany()
                        .HasForeignKey("AuditLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuditService.Models.AuditType", "AuditType")
                        .WithMany()
                        .HasForeignKey("AuditTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
