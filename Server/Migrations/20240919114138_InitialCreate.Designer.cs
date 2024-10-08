﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(TaborIsDbContext))]
    [Migration("20240919114138_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Allergen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Allergens");
                });

            modelBuilder.Entity("Meal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("MealTime")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("MealAllergen", b =>
                {
                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<int>("AllergenId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("MealId", "AllergenId");

                    b.HasIndex("AllergenId");

                    b.ToTable("MealAllergens");
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MealId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Participant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("BirthNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("ParticipantAllergen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AllergenId")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AllergenId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("ParticipantAllergens");
                });

            modelBuilder.Entity("MealAllergen", b =>
                {
                    b.HasOne("Allergen", "Allergen")
                        .WithMany("MealAllergens")
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal", "Meal")
                        .WithMany("MealAllergens")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Allergen");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.HasOne("Meal", "Meal")
                        .WithMany("Orders")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Participant", "Participant")
                        .WithMany("Orders")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meal");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("ParticipantAllergen", b =>
                {
                    b.HasOne("Allergen", "Allergen")
                        .WithMany()
                        .HasForeignKey("AllergenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Participant", "Participant")
                        .WithMany("ParticipantAllergens")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Allergen");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("Allergen", b =>
                {
                    b.Navigation("MealAllergens");
                });

            modelBuilder.Entity("Meal", b =>
                {
                    b.Navigation("MealAllergens");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Participant", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("ParticipantAllergens");
                });
#pragma warning restore 612, 618
        }
    }
}
