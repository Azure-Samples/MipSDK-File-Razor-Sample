using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MipSdkRazorSample.Migrations
{
    public partial class DataPolicyEnumFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PolicyDirection",
                table: "DataPolicy",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyDirection",
                table: "DataPolicy");
        }
    }
}
