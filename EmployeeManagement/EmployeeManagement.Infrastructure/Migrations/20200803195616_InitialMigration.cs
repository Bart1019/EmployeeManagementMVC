using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagement.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Employees");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Employees",
                table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50),
                    Surname = table.Column<string>(),
                    EmailAddress = table.Column<string>(),
                    Department = table.Column<int>(),
                    GenderType = table.Column<int>()
                },
                constraints: table => { table.PrimaryKey("PK_Employees", x => x.Id); });
        }
    }
}