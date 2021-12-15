using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wasp.WebApi.Migrations
{
    public partial class AddedDataAreaReferenceToDataArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilterFrom",
                table: "DataFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataTableId",
                table: "DataAreas",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<string>(
                name: "TableId",
                table: "DataAreas",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DataAreaReference",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAreaId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ReferenceDataItemId = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    ReferenceDataTableId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    KeyDataItemId = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    KeyDataItemDataTableId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAreaReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataAreaReference_DataAreas_DataAreaId",
                        column: x => x.DataAreaId,
                        principalTable: "DataAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataAreaReference_DataItem_KeyDataItemId_KeyDataItemDataTableId",
                        columns: x => new { x.KeyDataItemId, x.KeyDataItemDataTableId },
                        principalTable: "DataItem",
                        principalColumns: new[] { "Id", "DataTableId" });
                    table.ForeignKey(
                        name: "FK_DataAreaReference_DataItem_ReferenceDataItemId_ReferenceDataTableId",
                        columns: x => new { x.ReferenceDataItemId, x.ReferenceDataTableId },
                        principalTable: "DataItem",
                        principalColumns: new[] { "Id", "DataTableId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataAreas_TableId",
                table: "DataAreas",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_DataAreaReference_DataAreaId",
                table: "DataAreaReference",
                column: "DataAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_DataAreaReference_KeyDataItemId_KeyDataItemDataTableId",
                table: "DataAreaReference",
                columns: new[] { "KeyDataItemId", "KeyDataItemDataTableId" });

            migrationBuilder.CreateIndex(
                name: "IX_DataAreaReference_ReferenceDataItemId_ReferenceDataTableId",
                table: "DataAreaReference",
                columns: new[] { "ReferenceDataItemId", "ReferenceDataTableId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DataAreas_DataTable_TableId",
                table: "DataAreas",
                column: "TableId",
                principalTable: "DataTable",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataAreas_DataTable_TableId",
                table: "DataAreas");

            migrationBuilder.DropTable(
                name: "DataAreaReference");

            migrationBuilder.DropIndex(
                name: "IX_DataAreas_TableId",
                table: "DataAreas");

            migrationBuilder.DropColumn(
                name: "FilterFrom",
                table: "DataFields");

            migrationBuilder.DropColumn(
                name: "DataTableId",
                table: "DataAreas");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "DataAreas");
        }
    }
}
