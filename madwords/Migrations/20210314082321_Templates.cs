using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace madwords.Migrations
{
    public partial class Templates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MadwordTemplates",
                columns: table => new
                {
                    MadwordTemplateID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MadwordTemplateTitle = table.Column<string>(maxLength: 50, nullable: false),
                    AuthorId = table.Column<string>(nullable: true),
                    MadwordTemplateText = table.Column<string>(nullable: false),
                    MadwordTemplateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MadwordTemplates", x => x.MadwordTemplateID);
                    table.ForeignKey(
                        name: "FK_MadwordTemplates_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MadwordTemplates_AuthorId",
                table: "MadwordTemplates",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MadwordTemplates");
        }
    }
}
