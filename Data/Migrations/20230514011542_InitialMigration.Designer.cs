﻿// <auto-generated />
using System;
using Data.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230514011542_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Data.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ObservationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ObservationId");

                    b.ToTable("Channel");
                });

            modelBuilder.Entity("Data.Models.Observation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeTriggered")
                        .HasColumnType("TEXT");

                    b.Property<int>("TriggerMethod")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Observation");
                });

            modelBuilder.Entity("Data.Models.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Offset")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Scale")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Values")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("Data.Models.Channel", b =>
                {
                    b.HasOne("Data.Models.Observation", null)
                        .WithMany("Channels")
                        .HasForeignKey("ObservationId");
                });

            modelBuilder.Entity("Data.Models.Series", b =>
                {
                    b.HasOne("Data.Models.Channel", null)
                        .WithMany("Series")
                        .HasForeignKey("ChannelId");
                });

            modelBuilder.Entity("Data.Models.Channel", b =>
                {
                    b.Navigation("Series");
                });

            modelBuilder.Entity("Data.Models.Observation", b =>
                {
                    b.Navigation("Channels");
                });
#pragma warning restore 612, 618
        }
    }
}