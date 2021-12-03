using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MipSdkRazorSample.Migrations
{
    public partial class DataPolicyEnumFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PolicyDirection",
                table: "DataPolicy",
                newName: "Direction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Direction",
                table: "DataPolicy",
                newName: "PolicyDirection");
        }
    }
}
