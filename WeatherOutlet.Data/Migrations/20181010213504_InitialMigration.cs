using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherOutlet.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "Description", "IsCompleted", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 10, 10, 22, 35, 4, 799, DateTimeKind.Local), new DateTime(2018, 10, 9, 18, 35, 4, 798, DateTimeKind.Local), "Create a todo context persist your todos", true, "Create Todo dbcontext" },
                    { 2, new DateTime(2018, 10, 11, 0, 35, 4, 799, DateTimeKind.Local), new DateTime(2018, 10, 9, 20, 35, 4, 799, DateTimeKind.Local), "Create a service to perform CRUD operations on the database", true, "Create Todo Service" },
                    { 3, null, new DateTime(2018, 10, 9, 22, 35, 4, 799, DateTimeKind.Local), "Create base classes voor EntityService classes to make your live easier in the long run!", false, "Make base class for Service classes" },
                    { 4, null, new DateTime(2018, 10, 9, 22, 35, 4, 799, DateTimeKind.Local), "Add loads of more tests!", false, "Add more tests" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
