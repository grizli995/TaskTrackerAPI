using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskTracker.Database.Migrations
{
    public partial class AddRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProjectId",
                table: "Students",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_ProjectId",
                table: "Students",
                column: "ProjectId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_ProjectId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ProjectId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Courses");
        }
    }
}
