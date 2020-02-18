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
    [Migration("20200213102438_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("AuditService.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AuditID");

                    b.Property<int>("AuditLevelId");

                    b.Property<int>("AuditTypeId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Details")
                        .IsRequired();

                    b.Property<string>("Source")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AuditLevelId");

                    b.HasIndex("AuditTypeId");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("AuditService.Models.AuditLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AuditLevelID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AuditLevels");
                });

            modelBuilder.Entity("AuditService.Models.AuditType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AuditTypeID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AuditTypes");
                });

            modelBuilder.Entity("AuditService.Models.Audit", b =>
                {
                    b.HasOne("AuditService.Models.AuditLevel", "AuditLevel")
                        .WithMany()
                        .HasForeignKey("AuditLevelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AuditService.Models.AuditType", "AuditType")
                        .WithMany()
                        .HasForeignKey("AuditTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
