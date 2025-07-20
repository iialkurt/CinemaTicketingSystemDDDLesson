using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class dfdfdfd3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinemaHallScheduleMovieSchedule",
                schema: "scheduling");

            migrationBuilder.AddColumn<Guid>(
                name: "CinemaHallScheduleId",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CinemaHallId",
                schema: "scheduling",
                table: "CinemaHallSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MovieSchedules_CinemaHallScheduleId",
                schema: "scheduling",
                table: "MovieSchedules",
                column: "CinemaHallScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSchedules_CinemaHallSchedules_CinemaHallScheduleId",
                schema: "scheduling",
                table: "MovieSchedules",
                column: "CinemaHallScheduleId",
                principalSchema: "scheduling",
                principalTable: "CinemaHallSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieSchedules_CinemaHallSchedules_CinemaHallScheduleId",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropIndex(
                name: "IX_MovieSchedules_CinemaHallScheduleId",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "CinemaHallScheduleId",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "MovieId",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "CinemaHallId",
                schema: "scheduling",
                table: "CinemaHallSchedules");

            migrationBuilder.CreateTable(
                name: "CinemaHallScheduleMovieSchedule",
                schema: "scheduling",
                columns: table => new
                {
                    CinemaHallSchedulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieSchedulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHallScheduleMovieSchedule", x => new { x.CinemaHallSchedulesId, x.MovieSchedulesId });
                    table.ForeignKey(
                        name: "FK_CinemaHallScheduleMovieSchedule_CinemaHallSchedules_CinemaHallSchedulesId",
                        column: x => x.CinemaHallSchedulesId,
                        principalSchema: "scheduling",
                        principalTable: "CinemaHallSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CinemaHallScheduleMovieSchedule_MovieSchedules_MovieSchedulesId",
                        column: x => x.MovieSchedulesId,
                        principalSchema: "scheduling",
                        principalTable: "MovieSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CinemaHallScheduleMovieSchedule_MovieSchedulesId",
                schema: "scheduling",
                table: "CinemaHallScheduleMovieSchedule",
                column: "MovieSchedulesId");
        }
    }
}
