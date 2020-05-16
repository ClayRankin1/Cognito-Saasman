using Microsoft.EntityFrameworkCore.Migrations;

namespace Cognito.DataAccess.Migrations
{
    public partial class ProjectUserIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProjectUsers",
                nullable: false, defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
