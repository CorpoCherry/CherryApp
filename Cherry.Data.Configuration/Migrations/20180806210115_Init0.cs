using Microsoft.EntityFrameworkCore.Migrations;

namespace Cherry.Data.Configuration.Migrations
{
    public partial class Init0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "DatabaseLogins",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseLogins", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Tag = table.Column<string>(maxLength: 255, nullable: false),
                    OfficialName = table.Column<string>(nullable: true),
                    PseudoName = table.Column<string>(nullable: true),
                    NamedBy = table.Column<string>(nullable: true),
                    CityName = table.Column<string>(nullable: true),
                    Adrress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Tag);
                    table.ForeignKey(
                        name: "FK_Tenants_Cities_CityName",
                        column: x => x.CityName,
                        principalTable: "Cities",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CityName",
                table: "Tenants",
                column: "CityName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatabaseLogins");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
