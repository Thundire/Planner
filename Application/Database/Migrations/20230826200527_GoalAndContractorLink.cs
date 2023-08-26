using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Application.Database.Migrations
{
    /// <inheritdoc />
    public partial class GoalAndContractorLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "contractor_id",
                table: "goals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_goals_contractor_id",
                table: "goals",
                column: "contractor_id");

            migrationBuilder.AddForeignKey(
                name: "fk_goals_contractors_contractor_id",
                table: "goals",
                column: "contractor_id",
                principalTable: "contractors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_goals_contractors_contractor_id",
                table: "goals");

            migrationBuilder.DropIndex(
                name: "ix_goals_contractor_id",
                table: "goals");

            migrationBuilder.DropColumn(
                name: "contractor_id",
                table: "goals");
        }
    }
}
