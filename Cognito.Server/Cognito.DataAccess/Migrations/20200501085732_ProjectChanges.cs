using Microsoft.EntityFrameworkCore.Migrations;

namespace Cognito.DataAccess.Migrations
{
    public partial class ProjectChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(name: "OwnerId", table: "Projects", nullable: true);
            migrationBuilder.Sql("UPDATE PROJECTS SET OwnerId = (SELECT TOP(1) ID FROM [User].AspNetUsers) WHERE OwnerId IS NULL");
            migrationBuilder.AlterColumn<int>(name: "OwnerId", "Projects", nullable: false);
            migrationBuilder.AddForeignKey(name: "FK_Projects_AspNetUsers_OwnerId", table: "Projects", column: "OwnerId", principalTable: "AspNetUsers", principalSchema: "User", principalColumn: "Id");
            migrationBuilder.CreateIndex(name: "IX_Projects_OwnerId", table: "Projects", column: "OwnerId");

            migrationBuilder.AddColumn<int>(name: "ProxyId", table: "Projects", nullable: true);
            migrationBuilder.AddForeignKey(name: "FK_Projects_AspNetUsers_ProxyId", table: "Projects", column: "ProxyId", principalTable: "AspNetUsers", principalSchema: "User", principalColumn: "Id");
            migrationBuilder.CreateIndex( name: "IX_Projects_ProxyId", table: "Projects", column: "ProxyId");

            migrationBuilder.AddColumn<string>(name: "Description", "Projects", nullable: true, maxLength: 1000);
            migrationBuilder.AddColumn<bool>(name: "IsBillable", "Projects", nullable: false, defaultValue: false);
            migrationBuilder.DropColumn(name: "IsPrivate", "Projects");
            migrationBuilder.DropColumn(name: "IsArchived", "Projects");

            migrationBuilder.DropIndex(name: "IX_Projects_ProjectTypeId", table: "Projects");
            migrationBuilder.DropForeignKey(name: "FK_Projects_ProjectTypes_ProjectTypeId", table: "Projects");
            migrationBuilder.DropColumn(name: "ProjectTypeId", "Projects");
            migrationBuilder.DropTable(name: "ProjectTypes", schema: "Lookup");

            migrationBuilder.AlterColumn<string>(name: "FullName", "Projects", nullable: false, maxLength: 200);
            migrationBuilder.AlterColumn<string>(name: "Name", "Projects", nullable: false, maxLength: 25);
            migrationBuilder.RenameColumn("Name", "Projects", "Nickname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

