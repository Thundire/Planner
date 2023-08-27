using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class DataExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "collapsed",
                table: "goal_elapsed_time_parts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "collapsed",
                table: "goal_elapsed_time_parts");
        }
    }
}
