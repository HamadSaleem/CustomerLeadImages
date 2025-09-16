
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerLeadImages.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1,  1"),
                    Company = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileImages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerType = table.Column<string>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    Base64Data = table.Column<string>(nullable: false),
                    MimeType = table.Column<string>(nullable: false),
                    Caption = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileImages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_OwnerType_OwnerId",
                table: "ProfileImages",
                columns: new[] { "OwnerType", "OwnerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ProfileImages");
            migrationBuilder.DropTable(name: "Customers");
            migrationBuilder.DropTable(name: "Leads");
        }
    }
}
