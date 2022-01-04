using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wasp.WebApi.Migrations
{
    public partial class AddedRequiredToDataItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "DataItem",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 4);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Required",
                table: "DataItem");
        }
    }
}
