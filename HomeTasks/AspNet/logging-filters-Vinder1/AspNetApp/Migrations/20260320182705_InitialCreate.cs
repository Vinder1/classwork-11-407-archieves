using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AspNetApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Galaxies",
                columns: table => new
                {
                    Version = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galaxies", x => x.Version);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stars",
                columns: table => new
                {
                    GalaxyVersion = table.Column<int>(type: "integer", nullable: false),
                    Radius = table.Column<int>(type: "integer", nullable: false),
                    Angle = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stars", x => new { x.GalaxyVersion, x.Radius, x.Angle });
                    table.ForeignKey(
                        name: "FK_Stars_Galaxies_GalaxyVersion",
                        column: x => x.GalaxyVersion,
                        principalTable: "Galaxies",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spaceships",
                columns: table => new
                {
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GalaxyVersion = table.Column<int>(type: "integer", nullable: false),
                    LocatedRadius = table.Column<int>(type: "integer", nullable: false),
                    LocatedAngle = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spaceships", x => x.OwnerId);
                    table.ForeignKey(
                        name: "FK_Spaceships_Galaxies_GalaxyVersion",
                        column: x => x.GalaxyVersion,
                        principalTable: "Galaxies",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Spaceships_Players_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpaceshipStarVisits",
                columns: table => new
                {
                    SpaceshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    StarGalaxyVersion = table.Column<int>(type: "integer", nullable: false),
                    StarRadius = table.Column<int>(type: "integer", nullable: false),
                    StarAngle = table.Column<double>(type: "double precision", nullable: false),
                    VisitedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpaceshipStarVisits", x => new { x.SpaceshipId, x.StarGalaxyVersion, x.StarRadius, x.StarAngle });
                    table.ForeignKey(
                        name: "FK_SpaceshipStarVisits_Spaceships_SpaceshipId",
                        column: x => x.SpaceshipId,
                        principalTable: "Spaceships",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpaceshipStarVisits_Stars_StarGalaxyVersion_StarRadius_Star~",
                        columns: x => new { x.StarGalaxyVersion, x.StarRadius, x.StarAngle },
                        principalTable: "Stars",
                        principalColumns: new[] { "GalaxyVersion", "Radius", "Angle" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spaceships_GalaxyVersion",
                table: "Spaceships",
                column: "GalaxyVersion");

            migrationBuilder.CreateIndex(
                name: "IX_SpaceshipStarVisits_StarGalaxyVersion_StarRadius_StarAngle",
                table: "SpaceshipStarVisits",
                columns: new[] { "StarGalaxyVersion", "StarRadius", "StarAngle" });

            migrationBuilder.CreateIndex(
                name: "IX_Stars_GalaxyVersion_Radius_Angle",
                table: "Stars",
                columns: new[] { "GalaxyVersion", "Radius", "Angle" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpaceshipStarVisits");

            migrationBuilder.DropTable(
                name: "Spaceships");

            migrationBuilder.DropTable(
                name: "Stars");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Galaxies");
        }
    }
}
