using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class EditableElapsedParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "collapsed",
                table: "goal_elapsed_time_parts",
                newName: "edited_by_hand");

            migrationBuilder.AddColumn<int>(
                name: "contractor_id",
                table: "jobs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_jobs_contractor_id",
                table: "jobs",
                column: "contractor_id");

            migrationBuilder.AddForeignKey(
                name: "fk_jobs_contractors_contractor_id",
                table: "jobs",
                column: "contractor_id",
                principalTable: "contractors",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_jobs_contractors_contractor_id",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "ix_jobs_contractor_id",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "contractor_id",
                table: "jobs");

            migrationBuilder.RenameColumn(
                name: "edited_by_hand",
                table: "goal_elapsed_time_parts",
                newName: "collapsed");
        }
    }
}
