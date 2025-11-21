using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Prioryties_PriorityId",
                table: "Todos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prioryties",
                table: "Prioryties");

            migrationBuilder.RenameTable(
                name: "Prioryties",
                newName: "Priorities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Priorities_PriorityId",
                table: "Todos",
                column: "PriorityId",
                principalTable: "Priorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Priorities_PriorityId",
                table: "Todos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities");

            migrationBuilder.RenameTable(
                name: "Priorities",
                newName: "Prioryties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prioryties",
                table: "Prioryties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Prioryties_PriorityId",
                table: "Todos",
                column: "PriorityId",
                principalTable: "Prioryties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
