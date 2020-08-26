using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class ServiceEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    StorageValue = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longituted = table.Column<double>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    ResponsibleOperators = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "admin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.UpdateData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "adnmin");
        }
    }
}
