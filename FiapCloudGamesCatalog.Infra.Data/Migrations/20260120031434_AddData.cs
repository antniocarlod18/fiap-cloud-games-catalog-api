using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiapCloudGamesCatalog.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "Id", "DateCreated", "Available", "Description", "Developer", "Distributor", "GamePlatforms", "GameVersion", "Genre", "Price", "Title" },
                values: new object[,]
                {
                    { new Guid("10101010-1010-1010-1010-101010101010"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "A neon-soaked platformer with rhythm elements.", "SynthWave Studios", "IndiePub", "PC,Ps5", "1.0", "Platformer", 14.99m, "Neon Drift" },
                    { new Guid("20202020-2020-2020-2020-202020202020"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "A roguelike dungeon crawler with procedurally generated levels.", "DungeonCraft", "DungeonCraft", "PC", "0.8", "Roguelike", 24.99m, "Depths of Dread" },
                    { new Guid("30303030-3030-3030-3030-303030303030"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Turn-based tactics on a hex grid.", "Tactica Labs", "Tactica Labs", "PC,NintendoSwitch", "1.3", "Strategy", 39.99m, "Hex Dominion" },
                    { new Guid("40404040-4040-4040-4040-404040404040"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Casual puzzle game with relaxing music.", "CalmPlay", "CalmPlay", "Mobile,PC", "2.1", "Puzzle", 2.99m, "Ripple Relax" },
                    { new Guid("50505050-5050-5050-5050-505050505050"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Sci-fi exploration and base building.", "OrbitWorks", "OrbitWorks", "PC,XboxSeriesX", "0.9", "Simulation", 49.99m, "Frontier Foundry" },
                    { new Guid("60606060-6060-6060-6060-606060606060"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Fast-paced card battler with deck customization.", "Cardinal Games", "Cardinal Games", "PC,Mobile", "1.5", "Card", 9.99m, "Arcane Decks" },
                    { new Guid("70707070-7070-7070-7070-707070707070"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Multiplayer arena with short matches.", "ArenaForge", "ArenaForge", "PC,Ps5,XboxSeriesX", "4.0", "MOBA", 0.00m, "Arena Pulse" },
                    { new Guid("80808080-8080-8080-8080-808080808080"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Emotional narrative walking sim.", "QuietStone", "IndiePub", "PC,Ps4,Ps5", "1.0", "Narrative", 12.99m, "Beneath the Willow" },
                    { new Guid("90909090-9090-9090-9090-909090909090"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Retro-style beat 'em up.", "RetroWorks", "RetroWorks", "PC,NintendoSwitch", "1.2", "Action", 19.99m, "Street Brawlers" },
                    { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true, "Co-op puzzle adventure for friends.", "PairPlay", "PairPlay", "PC,NintendoSwitch", "1.0", "Co-op", 24.99m, "Twinlight" }
                }
            );

            migrationBuilder.InsertData(
                table: "Cart",
                columns: new[] { "Id", "UserId", "DateCreated", "Active" },
                values: new object[,]
                {
                    { new Guid("10101010-1010-1010-1010-101010101010"), new Guid("bbbbbbbb-2222-3333-4444-555555555555"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true}
                }
            );

            migrationBuilder.InsertData(
                table: "Library",
                columns: new[] { "Id", "UserId", "DateCreated", "Active" },
                values: new object[,]
                {
                    { new Guid("10101010-1010-1010-1010-101010101010"), new Guid("bbbbbbbb-2222-3333-4444-555555555555"), new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc), true}
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
