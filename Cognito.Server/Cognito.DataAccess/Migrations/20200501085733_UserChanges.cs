using Microsoft.EntityFrameworkCore.Migrations;

namespace Cognito.DataAccess.Migrations
{
    public partial class UserChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                schema: "User",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                schema: "User",
                maxLength: 50,
                nullable: true);

            migrationBuilder.Sql("UPDATE [User].AspNetUsers SET FirstName = UserName, LastName = UserName WHERE FirstName IS NULL AND LastName IS NULL");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                schema: "User",
                maxLength: 50,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                schema: "User",
                maxLength: 50,
                nullable: false);
            
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                schema: "User",
                nullable: false, defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers",
                schema: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers",
                schema: "User");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers",
                schema: "User");
        }
    }
}
