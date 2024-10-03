using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSU.IPW.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaskList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TaskLists",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TaskLists");
        }
    }
}
