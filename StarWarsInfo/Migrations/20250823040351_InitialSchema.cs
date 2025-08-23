using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StarWarsInfo.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    FilmId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    EpisodeId = table.Column<int>(type: "integer", nullable: false),
                    OpeningCrawl = table.Column<string>(type: "text", nullable: false),
                    Director = table.Column<string>(type: "text", nullable: false),
                    Producer = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.FilmId);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Classification = table.Column<string>(type: "text", nullable: false),
                    Designation = table.Column<string>(type: "text", nullable: false),
                    AverageHeight = table.Column<int>(type: "integer", nullable: true),
                    SkinColors = table.Column<string>(type: "text", nullable: false),
                    HairColors = table.Column<string>(type: "text", nullable: false),
                    EyeColors = table.Column<string>(type: "text", nullable: false),
                    AverageLifespan = table.Column<int>(type: "integer", nullable: true),
                    HomeworldId = table.Column<int>(type: "integer", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.SpeciesId);
                });

            migrationBuilder.CreateTable(
                name: "Starships",
                columns: table => new
                {
                    StarshipId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    CostInCredits = table.Column<decimal>(type: "numeric", nullable: true),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxAtmospheringSpeed = table.Column<decimal>(type: "numeric", nullable: true),
                    Crew = table.Column<int>(type: "integer", nullable: true),
                    Passengers = table.Column<int>(type: "integer", nullable: true),
                    CargoCapacity = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Consumables = table.Column<string>(type: "text", nullable: true),
                    HyperdriveRating = table.Column<decimal>(type: "numeric", nullable: true),
                    Mglt = table.Column<int>(type: "integer", nullable: true),
                    StarshipClass = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Starships", x => x.StarshipId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: true),
                    CostInCredits = table.Column<decimal>(type: "numeric", nullable: true),
                    Length = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxAtmospheringSpeed = table.Column<int>(type: "integer", nullable: true),
                    Crew = table.Column<int>(type: "integer", nullable: true),
                    Passengers = table.Column<int>(type: "integer", nullable: true),
                    CargoCapacity = table.Column<string>(type: "text", nullable: true),
                    Consumables = table.Column<string>(type: "text", nullable: true),
                    VehicleClass = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "FilmSpecies",
                columns: table => new
                {
                    FilmsFilmId = table.Column<int>(type: "integer", nullable: false),
                    SpeciesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmSpecies", x => new { x.FilmsFilmId, x.SpeciesId });
                    table.ForeignKey(
                        name: "FK_FilmSpecies_Films_FilmsFilmId",
                        column: x => x.FilmsFilmId,
                        principalTable: "Films",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    PlanetId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RotationPeriod = table.Column<int>(type: "integer", nullable: true),
                    OrbitalPeriod = table.Column<int>(type: "integer", nullable: true),
                    Diameter = table.Column<int>(type: "integer", nullable: true),
                    Climate = table.Column<string>(type: "text", nullable: false),
                    Gravity = table.Column<decimal>(type: "numeric", nullable: true),
                    Terrain = table.Column<string>(type: "text", nullable: false),
                    SurfaceWater = table.Column<int>(type: "integer", nullable: true),
                    Population = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SpeciesId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.PlanetId);
                    table.ForeignKey(
                        name: "FK_Planets_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId");
                });

            migrationBuilder.CreateTable(
                name: "FilmStarship",
                columns: table => new
                {
                    FilmsFilmId = table.Column<int>(type: "integer", nullable: false),
                    StarshipsStarshipId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmStarship", x => new { x.FilmsFilmId, x.StarshipsStarshipId });
                    table.ForeignKey(
                        name: "FK_FilmStarship_Films_FilmsFilmId",
                        column: x => x.FilmsFilmId,
                        principalTable: "Films",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmStarship_Starships_StarshipsStarshipId",
                        column: x => x.StarshipsStarshipId,
                        principalTable: "Starships",
                        principalColumn: "StarshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmVehicle",
                columns: table => new
                {
                    FilmsFilmId = table.Column<int>(type: "integer", nullable: false),
                    VehiclesVehicleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmVehicle", x => new { x.FilmsFilmId, x.VehiclesVehicleId });
                    table.ForeignKey(
                        name: "FK_FilmVehicle_Films_FilmsFilmId",
                        column: x => x.FilmsFilmId,
                        principalTable: "Films",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmVehicle_Vehicles_VehiclesVehicleId",
                        column: x => x.VehiclesVehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmPlanet",
                columns: table => new
                {
                    FilmsFilmId = table.Column<int>(type: "integer", nullable: false),
                    PlanetsPlanetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmPlanet", x => new { x.FilmsFilmId, x.PlanetsPlanetId });
                    table.ForeignKey(
                        name: "FK_FilmPlanet_Films_FilmsFilmId",
                        column: x => x.FilmsFilmId,
                        principalTable: "Films",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmPlanet_Planets_PlanetsPlanetId",
                        column: x => x.PlanetsPlanetId,
                        principalTable: "Planets",
                        principalColumn: "PlanetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    Mass = table.Column<int>(type: "integer", nullable: true),
                    HairColor = table.Column<string>(type: "text", nullable: false),
                    SkinColor = table.Column<string>(type: "text", nullable: false),
                    EyeColor = table.Column<string>(type: "text", nullable: false),
                    BirthYear = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    HomeworldId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Edited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_People_Planets_HomeworldId",
                        column: x => x.HomeworldId,
                        principalTable: "Planets",
                        principalColumn: "PlanetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmPerson",
                columns: table => new
                {
                    CharactersPersonId = table.Column<int>(type: "integer", nullable: false),
                    FilmsFilmId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmPerson", x => new { x.CharactersPersonId, x.FilmsFilmId });
                    table.ForeignKey(
                        name: "FK_FilmPerson_Films_FilmsFilmId",
                        column: x => x.FilmsFilmId,
                        principalTable: "Films",
                        principalColumn: "FilmId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmPerson_People_CharactersPersonId",
                        column: x => x.CharactersPersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonSpecies",
                columns: table => new
                {
                    PeoplePersonId = table.Column<int>(type: "integer", nullable: false),
                    SpeciesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonSpecies", x => new { x.PeoplePersonId, x.SpeciesId });
                    table.ForeignKey(
                        name: "FK_PersonSpecies_People_PeoplePersonId",
                        column: x => x.PeoplePersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonStarship",
                columns: table => new
                {
                    PilotsPersonId = table.Column<int>(type: "integer", nullable: false),
                    StarshipsStarshipId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonStarship", x => new { x.PilotsPersonId, x.StarshipsStarshipId });
                    table.ForeignKey(
                        name: "FK_PersonStarship_People_PilotsPersonId",
                        column: x => x.PilotsPersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonStarship_Starships_StarshipsStarshipId",
                        column: x => x.StarshipsStarshipId,
                        principalTable: "Starships",
                        principalColumn: "StarshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonVehicle",
                columns: table => new
                {
                    PilotsPersonId = table.Column<int>(type: "integer", nullable: false),
                    VehiclesVehicleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonVehicle", x => new { x.PilotsPersonId, x.VehiclesVehicleId });
                    table.ForeignKey(
                        name: "FK_PersonVehicle_People_PilotsPersonId",
                        column: x => x.PilotsPersonId,
                        principalTable: "People",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonVehicle_Vehicles_VehiclesVehicleId",
                        column: x => x.VehiclesVehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmPerson_FilmsFilmId",
                table: "FilmPerson",
                column: "FilmsFilmId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmPlanet_PlanetsPlanetId",
                table: "FilmPlanet",
                column: "PlanetsPlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmSpecies_SpeciesId",
                table: "FilmSpecies",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmStarship_StarshipsStarshipId",
                table: "FilmStarship",
                column: "StarshipsStarshipId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmVehicle_VehiclesVehicleId",
                table: "FilmVehicle",
                column: "VehiclesVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_People_HomeworldId",
                table: "People",
                column: "HomeworldId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonSpecies_SpeciesId",
                table: "PersonSpecies",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonStarship_StarshipsStarshipId",
                table: "PersonStarship",
                column: "StarshipsStarshipId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonVehicle_VehiclesVehicleId",
                table: "PersonVehicle",
                column: "VehiclesVehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_SpeciesId",
                table: "Planets",
                column: "SpeciesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmPerson");

            migrationBuilder.DropTable(
                name: "FilmPlanet");

            migrationBuilder.DropTable(
                name: "FilmSpecies");

            migrationBuilder.DropTable(
                name: "FilmStarship");

            migrationBuilder.DropTable(
                name: "FilmVehicle");

            migrationBuilder.DropTable(
                name: "PersonSpecies");

            migrationBuilder.DropTable(
                name: "PersonStarship");

            migrationBuilder.DropTable(
                name: "PersonVehicle");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "Starships");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Species");
        }
    }
}
