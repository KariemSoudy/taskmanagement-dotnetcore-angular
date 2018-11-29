using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TasksManagement.Data.Migrations
{
    public partial class CreateTasksDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    RoleID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    OwnerUserID = table.Column<int>(nullable: false),
                    AssignedToUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AssignedToUserID",
                        column: x => x.AssignedToUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_OwnerUserID",
                        column: x => x.OwnerUserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Support" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Name", "RoleID" },
                values: new object[] { 1, "Admin1", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Name", "RoleID" },
                values: new object[] { 2, "Support1", 2 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Name", "RoleID" },
                values: new object[] { 3, "Support2", 2 });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "ID", "AssignedToUserID", "Created", "Description", "OwnerUserID", "Title" },
                values: new object[] { 1, null, new DateTime(2018, 11, 30, 2, 21, 13, 217, DateTimeKind.Local), "Task 1 Description", 2, "Task1" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "ID", "AssignedToUserID", "Created", "Description", "OwnerUserID", "Title" },
                values: new object[] { 2, null, new DateTime(2018, 11, 30, 2, 21, 13, 218, DateTimeKind.Local), "Task 2 Description", 3, "Task2" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToUserID",
                table: "Tasks",
                column: "AssignedToUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerUserID",
                table: "Tasks",
                column: "OwnerUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
