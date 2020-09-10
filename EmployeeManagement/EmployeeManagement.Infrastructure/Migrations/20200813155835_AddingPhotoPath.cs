using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagement.Infrastructure.Migrations
{
    public partial class AddingPhotoPath : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "PhotoPath",
                "Employees");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "PhotoPath",
                "Employees",
                nullable: true);
        }
    }
}