using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wasp.WebApi.Migrations
{
    public partial class MadeDataAreaRecursive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "DataAreas",
                type: "nvarchar(10)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.CreateIndex(
                name: "IX_DataAreas_ParentId",
                table: "DataAreas",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataAreas_DataAreas_ParentId",
                table: "DataAreas",
                column: "ParentId",
                principalTable: "DataAreas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataAreas_DataAreas_ParentId",
                table: "DataAreas");

            migrationBuilder.DropIndex(
                name: "IX_DataAreas_ParentId",
                table: "DataAreas");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "DataAreas");
        }
    }
}
