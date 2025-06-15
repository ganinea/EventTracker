using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTracker.Infrastructure.Storage.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parcels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PickupDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastEvent_Id = table.Column<long>(type: "bigint", nullable: true),
                    LastEvent_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastEvent_CreatedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastEvent_StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastEvent_RunId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    LastProcessedEventId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerStates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcels");

            migrationBuilder.DropTable(
                name: "WorkerStates");
        }
    }
}
