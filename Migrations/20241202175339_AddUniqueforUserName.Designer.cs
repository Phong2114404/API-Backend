﻿// <auto-generated />
using System;
using IoTwithMysql.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IoTwithMysql.Migrations
{
    [DbContext(typeof(MyDBcontext))]
    [Migration("20241202175339_AddUniqueforUserName")]
    partial class AddUniqueforUserName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("IoTwithMysql.Data.Measurement", b =>
                {
                    b.Property<int>("MeasurementID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("MeasurementID"));

                    b.Property<decimal>("HeartRate")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("MeasurementTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("SpO2")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Temp")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.Property<decimal>("Weight")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("MeasurementID");

                    b.HasIndex("UserID");

                    b.HasIndex("Version");

                    b.ToTable("Measurement");
                });

            modelBuilder.Entity("IoTwithMysql.Data.Sensor", b =>
                {
                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Version"));

                    b.Property<string>("HeartRate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SpO2")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Temp")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Weight")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Version");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("IoTwithMysql.Data.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserID");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("IoTwithMysql.Data.Measurement", b =>
                {
                    b.HasOne("IoTwithMysql.Data.User", "User")
                        .WithMany("Measurements")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IoTwithMysql.Data.Sensor", "Sensor")
                        .WithMany("Measurements")
                        .HasForeignKey("Version")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sensor");

                    b.Navigation("User");
                });

            modelBuilder.Entity("IoTwithMysql.Data.Sensor", b =>
                {
                    b.Navigation("Measurements");
                });

            modelBuilder.Entity("IoTwithMysql.Data.User", b =>
                {
                    b.Navigation("Measurements");
                });
#pragma warning restore 612, 618
        }
    }
}
