﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherOutlet.Data;

namespace WeatherOutlet.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181010213504_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WeatherOutlet.Data.Models.TodoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CompletedAt");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsCompleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Todos");

                    b.HasData(
                        new { Id = 1, CompletedAt = new DateTime(2018, 10, 10, 22, 35, 4, 799, DateTimeKind.Local), CreatedAt = new DateTime(2018, 10, 9, 18, 35, 4, 798, DateTimeKind.Local), Description = "Create a todo context persist your todos", IsCompleted = true, Name = "Create Todo dbcontext" },
                        new { Id = 2, CompletedAt = new DateTime(2018, 10, 11, 0, 35, 4, 799, DateTimeKind.Local), CreatedAt = new DateTime(2018, 10, 9, 20, 35, 4, 799, DateTimeKind.Local), Description = "Create a service to perform CRUD operations on the database", IsCompleted = true, Name = "Create Todo Service" },
                        new { Id = 3, CreatedAt = new DateTime(2018, 10, 9, 22, 35, 4, 799, DateTimeKind.Local), Description = "Create base classes voor EntityService classes to make your live easier in the long run!", IsCompleted = false, Name = "Make base class for Service classes" },
                        new { Id = 4, CreatedAt = new DateTime(2018, 10, 9, 22, 35, 4, 799, DateTimeKind.Local), Description = "Add loads of more tests!", IsCompleted = false, Name = "Add more tests" }
                    );
                });
#pragma warning restore 612, 618
        }
    }
}