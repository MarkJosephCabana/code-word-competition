using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWord.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cdwrd");

            migrationBuilder.CreateTable(
                name: "competitions",
                schema: "cdwrd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompetitionDates_StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompetitionDates_EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "competition_rounds",
                schema: "cdwrd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionRoundGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodeWord = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CompetitionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition_rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_competition_rounds_competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalSchema: "cdwrd",
                        principalTable: "competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "cdwrd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HomeAddress_AddressLine1 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    HomeAddress_AddressLine2 = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    HomeAddress_Suburb = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HomeAddress_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HomeAddress_PostCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HasOptIn = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CompetitionRoundId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_competition_rounds_CompetitionRoundId",
                        column: x => x.CompetitionRoundId,
                        principalSchema: "cdwrd",
                        principalTable: "competition_rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_competition_rounds_CompetitionId",
                schema: "cdwrd",
                table: "competition_rounds",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_competition_rounds_CompetitionRoundGUID",
                schema: "cdwrd",
                table: "competition_rounds",
                column: "CompetitionRoundGUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_competitions_CompetitionGUID",
                schema: "cdwrd",
                table: "competitions",
                column: "CompetitionGUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_CompetitionRoundId",
                schema: "cdwrd",
                table: "users",
                column: "CompetitionRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_users_UserGUID",
                schema: "cdwrd",
                table: "users",
                column: "UserGUID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users",
                schema: "cdwrd");

            migrationBuilder.DropTable(
                name: "competition_rounds",
                schema: "cdwrd");

            migrationBuilder.DropTable(
                name: "competitions",
                schema: "cdwrd");
        }
    }
}
