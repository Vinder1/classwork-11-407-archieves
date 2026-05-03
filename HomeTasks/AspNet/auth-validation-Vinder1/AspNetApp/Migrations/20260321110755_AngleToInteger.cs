using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetApp.Migrations
{
    /// <inheritdoc />
    public partial class AngleToInteger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaceshipStarVisits_Stars_StarGalaxyVersion_StarRadius_Star~",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stars",
                table: "Stars");

            migrationBuilder.DropIndex(
                name: "IX_Stars_GalaxyVersion_Radius_Angle",
                table: "Stars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpaceshipStarVisits",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropIndex(
                name: "IX_SpaceshipStarVisits_StarGalaxyVersion_StarRadius_StarAngle",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropColumn(
                name: "Angle",
                table: "Stars");

            migrationBuilder.DropColumn(
                name: "StarAngle",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropColumn(
                name: "LocatedAngle",
                table: "Spaceships");

            migrationBuilder.AddColumn<int>(
                name: "AngleMilliradians",
                table: "Stars",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarAngleMilliradians",
                table: "SpaceshipStarVisits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocatedAngleMilliradians",
                table: "Spaceships",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stars",
                table: "Stars",
                columns: new[] { "GalaxyVersion", "Radius", "AngleMilliradians" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpaceshipStarVisits",
                table: "SpaceshipStarVisits",
                columns: new[] { "SpaceshipId", "StarGalaxyVersion", "StarRadius", "StarAngleMilliradians" });

            migrationBuilder.CreateIndex(
                name: "IX_Stars_GalaxyVersion_Radius_AngleMilliradians",
                table: "Stars",
                columns: new[] { "GalaxyVersion", "Radius", "AngleMilliradians" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpaceshipStarVisits_StarGalaxyVersion_StarRadius_StarAngleM~",
                table: "SpaceshipStarVisits",
                columns: new[] { "StarGalaxyVersion", "StarRadius", "StarAngleMilliradians" });

            migrationBuilder.AddForeignKey(
                name: "FK_SpaceshipStarVisits_Stars_StarGalaxyVersion_StarRadius_Star~",
                table: "SpaceshipStarVisits",
                columns: new[] { "StarGalaxyVersion", "StarRadius", "StarAngleMilliradians" },
                principalTable: "Stars",
                principalColumns: new[] { "GalaxyVersion", "Radius", "AngleMilliradians" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpaceshipStarVisits_Stars_StarGalaxyVersion_StarRadius_Star~",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stars",
                table: "Stars");

            migrationBuilder.DropIndex(
                name: "IX_Stars_GalaxyVersion_Radius_AngleMilliradians",
                table: "Stars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpaceshipStarVisits",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropIndex(
                name: "IX_SpaceshipStarVisits_StarGalaxyVersion_StarRadius_StarAngleM~",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropColumn(
                name: "AngleMilliradians",
                table: "Stars");

            migrationBuilder.DropColumn(
                name: "StarAngleMilliradians",
                table: "SpaceshipStarVisits");

            migrationBuilder.DropColumn(
                name: "LocatedAngleMilliradians",
                table: "Spaceships");

            migrationBuilder.AddColumn<double>(
                name: "Angle",
                table: "Stars",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StarAngle",
                table: "SpaceshipStarVisits",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LocatedAngle",
                table: "Spaceships",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stars",
                table: "Stars",
                columns: new[] { "GalaxyVersion", "Radius", "Angle" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpaceshipStarVisits",
                table: "SpaceshipStarVisits",
                columns: new[] { "SpaceshipId", "StarGalaxyVersion", "StarRadius", "StarAngle" });

            migrationBuilder.CreateIndex(
                name: "IX_Stars_GalaxyVersion_Radius_Angle",
                table: "Stars",
                columns: new[] { "GalaxyVersion", "Radius", "Angle" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpaceshipStarVisits_StarGalaxyVersion_StarRadius_StarAngle",
                table: "SpaceshipStarVisits",
                columns: new[] { "StarGalaxyVersion", "StarRadius", "StarAngle" });

            migrationBuilder.AddForeignKey(
                name: "FK_SpaceshipStarVisits_Stars_StarGalaxyVersion_StarRadius_Star~",
                table: "SpaceshipStarVisits",
                columns: new[] { "StarGalaxyVersion", "StarRadius", "StarAngle" },
                principalTable: "Stars",
                principalColumns: new[] { "GalaxyVersion", "Radius", "Angle" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
