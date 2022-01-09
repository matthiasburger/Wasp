using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wasp.WebApi.Migrations
{
    public partial class AddedAppendToDataArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Append",
                table: "DataAreas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Append",
                table: "DataAreas");
        }
    }
}
