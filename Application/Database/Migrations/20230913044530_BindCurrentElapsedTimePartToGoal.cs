using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class BindCurrentElapsedTimePartToGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "current_elapsed_time_part_id",
                table: "goals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_elapsed_time_part_id",
                table: "goals");
        }
    }
}
