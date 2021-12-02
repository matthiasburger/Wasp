using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wasp.WebApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataTable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    SqlId = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Index",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Index", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    DataTableId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    PythonId = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataItem", x => new { x.Id, x.DataTableId });
                    table.ForeignKey(
                        name: "FK_DataItem_DataTable_DataTableId",
                        column: x => x.DataTableId,
                        principalTable: "DataTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relation",
                columns: table => new
                {
                    IndexId = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    KeyDataItemId = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    KeyDataTableId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ReferenceDataItemId = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    ReferenceDataTableId = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relation", x => new { x.IndexId, x.KeyDataItemId, x.KeyDataTableId })
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Relation_DataItem_KeyDataItemId_KeyDataTableId",
                        columns: x => new { x.KeyDataItemId, x.KeyDataTableId },
                        principalTable: "DataItem",
                        principalColumns: new[] { "Id", "DataTableId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relation_DataItem_ReferenceDataItemId_ReferenceDataTableId",
                        columns: x => new { x.ReferenceDataItemId, x.ReferenceDataTableId },
                        principalTable: "DataItem",
                        principalColumns: new[] { "Id", "DataTableId" });
                    table.ForeignKey(
                        name: "FK_Relation_Index_IndexId",
                        column: x => x.IndexId,
                        principalTable: "Index",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataItem_DataTableId",
                table: "DataItem",
                column: "DataTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Relation_KeyDataItemId_KeyDataTableId",
                table: "Relation",
                columns: new[] { "KeyDataItemId", "KeyDataTableId" });

            migrationBuilder.CreateIndex(
                name: "IX_Relation_ReferenceDataItemId_ReferenceDataTableId",
                table: "Relation",
                columns: new[] { "ReferenceDataItemId", "ReferenceDataTableId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relation");

            migrationBuilder.DropTable(
                name: "DataItem");

            migrationBuilder.DropTable(
                name: "Index");

            migrationBuilder.DropTable(
                name: "DataTable");
        }
    }
}
