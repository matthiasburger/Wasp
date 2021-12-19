using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wasp.WebApi.Migrations
{
    public partial class InitialDatabase : Migration
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
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryKeys",
                columns: table => new
                {
                    TableName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryKeys", x => x.TableName);
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
                name: "DataAreas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ModuleId = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    DataTableId = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataAreas_DataAreas_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DataAreas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DataAreas_DataTable_DataTableId",
                        column: x => x.DataTableId,
                        principalTable: "DataTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DataAreas_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id");
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

            migrationBuilder.CreateTable(
                name: "DataFields",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    DataAreaId = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    DataTableId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DataItemId = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    FilterFrom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataFields_DataAreas_DataAreaId",
                        column: x => x.DataAreaId,
                        principalTable: "DataAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataFields_DataItem_DataItemId_DataTableId",
                        columns: x => new { x.DataItemId, x.DataTableId },
                        principalTable: "DataItem",
                        principalColumns: new[] { "Id", "DataTableId" });
                    table.ForeignKey(
                        name: "FK_DataFields_DataTable_DataTableId",
                        column: x => x.DataTableId,
                        principalTable: "DataTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_DataAreas_DataTableId",
                table: "DataAreas",
                column: "DataTableId");

            migrationBuilder.CreateIndex(
                name: "IX_DataAreas_ModuleId",
                table: "DataAreas",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_DataAreas_ParentId",
                table: "DataAreas",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DataFields_DataAreaId",
                table: "DataFields",
                column: "DataAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_DataFields_DataItemId_DataTableId",
                table: "DataFields",
                columns: new[] { "DataItemId", "DataTableId" });

            migrationBuilder.CreateIndex(
                name: "IX_DataFields_DataTableId",
                table: "DataFields",
                column: "DataTableId");

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
                name: "DataAreaReference");

            migrationBuilder.DropTable(
                name: "DataFields");

            migrationBuilder.DropTable(
                name: "PrimaryKeys");

            migrationBuilder.DropTable(
                name: "Relation");

            migrationBuilder.DropTable(
                name: "DataAreas");

            migrationBuilder.DropTable(
                name: "DataItem");

            migrationBuilder.DropTable(
                name: "Index");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "DataTable");
        }
    }
}
