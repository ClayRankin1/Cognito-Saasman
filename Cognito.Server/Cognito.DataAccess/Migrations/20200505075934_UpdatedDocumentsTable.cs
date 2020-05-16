using Microsoft.EntityFrameworkCore.Migrations;

namespace Cognito.DataAccess.Migrations
{
    public partial class UpdatedDocumentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectDocuments");

            migrationBuilder.EnsureSchema(
                name: "Task");

            migrationBuilder.RenameTable(
                name: "TaskWebsites",
                newName: "TaskWebsites",
                newSchema: "Task");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Tasks",
                newSchema: "Task");

            migrationBuilder.RenameTable(
                name: "TaskContacts",
                newName: "TaskContacts",
                newSchema: "Task");

            migrationBuilder.RenameTable(
                name: "Subtasks",
                newName: "Subtasks",
                newSchema: "Task");

            migrationBuilder.CreateTable(
                name: "TaskDocuments",
                schema: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false),
                    DocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDocuments", x => new { x.TaskId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_TaskDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Detail",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskDocuments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "Task",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Label",
                value: "Web Reference");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Label",
                value: "Contact Reference");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 40,
                column: "Label",
                value: "Doc Reference");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 41,
                column: "Label",
                value: "Phone Call");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "LicenseTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Label",
                value: "Time Keeper");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDocuments_DocumentId",
                schema: "Task",
                table: "TaskDocuments",
                column: "DocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDocuments",
                schema: "Task");

            migrationBuilder.RenameTable(
                name: "TaskWebsites",
                schema: "Task",
                newName: "TaskWebsites");

            migrationBuilder.RenameTable(
                name: "Tasks",
                schema: "Task",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "TaskContacts",
                schema: "Task",
                newName: "TaskContacts");

            migrationBuilder.RenameTable(
                name: "Subtasks",
                schema: "Task",
                newName: "Subtasks");

            migrationBuilder.CreateTable(
                name: "ProjectDocuments",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDocuments", x => new { x.ProjectId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Detail",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectDocuments_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Label",
                value: "WebReference");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Label",
                value: "ContactReference");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 40,
                column: "Label",
                value: "DocReference");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 41,
                column: "Label",
                value: "PhoneCall");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "LicenseTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Label",
                value: "TimeKeeper");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_DocumentId",
                table: "ProjectDocuments",
                column: "DocumentId");
        }
    }
}
