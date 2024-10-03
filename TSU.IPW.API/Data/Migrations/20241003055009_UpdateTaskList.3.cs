using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSU.IPW.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaskList3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListId",
                table: "TaskItems");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "TaskItems",
                newName: "Completed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "TaskItems",
                newName: "IsCompleted");

            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "TaskItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
