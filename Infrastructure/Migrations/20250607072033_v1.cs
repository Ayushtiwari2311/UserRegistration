using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MGenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MHobbies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MHobbies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MCities_MStates_StateId",
                        column: x => x.StateId,
                        principalTable: "MStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrnUserRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnUserRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnUserRegistrations_MCities_CityId",
                        column: x => x.CityId,
                        principalTable: "MCities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnUserRegistrations_MGenders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "MGenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnUserRegistrations_MStates_StateId",
                        column: x => x.StateId,
                        principalTable: "MStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MUserHobbies",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HobbyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MUserHobbies", x => new { x.UserId, x.HobbyId });
                    table.ForeignKey(
                        name: "FK_MUserHobbies_MHobbies_HobbyId",
                        column: x => x.HobbyId,
                        principalTable: "MHobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MUserHobbies_TrnUserRegistrations_UserId",
                        column: x => x.UserId,
                        principalTable: "TrnUserRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MCities_StateId",
                table: "MCities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_MUserHobbies_HobbyId",
                table: "MUserHobbies",
                column: "HobbyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnUserRegistrations_CityId",
                table: "TrnUserRegistrations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnUserRegistrations_GenderId",
                table: "TrnUserRegistrations",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnUserRegistrations_StateId",
                table: "TrnUserRegistrations",
                column: "StateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MUserHobbies");

            migrationBuilder.DropTable(
                name: "MHobbies");

            migrationBuilder.DropTable(
                name: "TrnUserRegistrations");

            migrationBuilder.DropTable(
                name: "MCities");

            migrationBuilder.DropTable(
                name: "MGenders");

            migrationBuilder.DropTable(
                name: "MStates");
        }
    }
}
