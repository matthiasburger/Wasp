using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wasp.WebApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "data");

            migrationBuilder.EnsureSchema(
                name: "system");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                schema: "data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationRole = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseSchema",
                schema: "system",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseSchema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Relationship",
                schema: "system",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationship", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataTable",
                schema: "system",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    ClrName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatabaseSchemaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataTable_DatabaseSchema_DatabaseSchemaId",
                        column: x => x.DatabaseSchemaId,
                        principalSchema: "system",
                        principalTable: "DatabaseSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataItem",
                schema: "system",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    ComputedSql = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUniqueIndex = table.Column<bool>(type: "bit", nullable: false),
                    IsForeignKey = table.Column<bool>(type: "bit", nullable: false),
                    IsPrimaryKey = table.Column<bool>(type: "bit", nullable: false),
                    DataTableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClrName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClrType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatabaseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Virtual = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    Precision = table.Column<int>(type: "int", nullable: true),
                    Scale = table.Column<int>(type: "int", nullable: true),
                    HasValueRange = table.Column<bool>(type: "bit", nullable: false),
                    ValueRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false),
                    IsKey = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataItem_DataTable_DataTableId",
                        column: x => x.DataTableId,
                        principalSchema: "system",
                        principalTable: "DataTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelationshipDataItem",
                schema: "system",
                columns: table => new
                {
                    RelationshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceDataItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetDataItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationshipDataItem", x => new { x.RelationshipId, x.SourceDataItemId, x.TargetDataItemId });
                    table.ForeignKey(
                        name: "FK_RelationshipDataItem_DataItem_SourceDataItemId",
                        column: x => x.SourceDataItemId,
                        principalSchema: "system",
                        principalTable: "DataItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelationshipDataItem_DataItem_TargetDataItemId",
                        column: x => x.TargetDataItemId,
                        principalSchema: "system",
                        principalTable: "DataItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelationshipDataItem_Relationship_RelationshipId",
                        column: x => x.RelationshipId,
                        principalSchema: "system",
                        principalTable: "Relationship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataItem_DataTableId",
                schema: "system",
                table: "DataItem",
                column: "DataTableId");

            migrationBuilder.CreateIndex(
                name: "IX_DataTable_DatabaseSchemaId",
                schema: "system",
                table: "DataTable",
                column: "DatabaseSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipDataItem_SourceDataItemId",
                schema: "system",
                table: "RelationshipDataItem",
                column: "SourceDataItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RelationshipDataItem_TargetDataItemId",
                schema: "system",
                table: "RelationshipDataItem",
                column: "TargetDataItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUser",
                schema: "data");

            migrationBuilder.DropTable(
                name: "RelationshipDataItem",
                schema: "system");

            migrationBuilder.DropTable(
                name: "DataItem",
                schema: "system");

            migrationBuilder.DropTable(
                name: "Relationship",
                schema: "system");

            migrationBuilder.DropTable(
                name: "DataTable",
                schema: "system");

            migrationBuilder.DropTable(
                name: "DatabaseSchema",
                schema: "system");
        }
    }
}
