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
                    { 1, new DateTime(2018, 10, 8, 21, 56, 34, 321, DateTimeKind.Local), new DateTime(2018, 10, 7, 17, 56, 34, 320, DateTimeKind.Local), "Create a todo context persist your todos", true, "Create Todo dbcontext" },
                    { 2, new DateTime(2018, 10, 8, 23, 56, 34, 321, DateTimeKind.Local), new DateTime(2018, 10, 7, 19, 56, 34, 321, DateTimeKind.Local), "Create a service to perform CRUD operations on the database", true, "Create Todo Service" },
                    { 3, null, new DateTime(2018, 10, 7, 21, 56, 34, 321, DateTimeKind.Local), "Create base classes voor EntityService classes to make your live easier in the long run!", false, "Make base class for Service classes" },
                    { 4, null, new DateTime(2018, 10, 7, 21, 56, 34, 321, DateTimeKind.Local), "Add loads of tests!", false, "Add tests" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
