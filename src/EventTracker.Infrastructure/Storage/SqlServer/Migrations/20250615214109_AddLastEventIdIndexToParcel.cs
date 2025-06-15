using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTracker.Infrastructure.Storage.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddLastEventIdIndexToParcel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Parcels_LastEvent_Id",
                table: "Parcels",
                column: "LastEvent_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Parcels_LastEvent_Id",
                table: "Parcels");
        }
    }
}
