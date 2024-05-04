﻿// <auto-generated />
using System;
using GalaxyBudsClient.Model.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GalaxyBudsClient.Migrations
{
    [DbContext(typeof(HistoryDbContext))]
    partial class HistoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4");

            modelBuilder.Entity("GalaxyBudsClient.Model.Database.HistoryRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BatteryCase")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BatteryL")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BatteryR")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("HostDevice")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsChargingCase")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsChargingL")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsChargingR")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NoiseControlMode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlacementL")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlacementR")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Records");
                });
#pragma warning restore 612, 618
        }
    }
}