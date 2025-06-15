using System.Reflection;
using EventTracker.Infrastructure.Common.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTracker.Infrastructure.Storage.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateParcelsStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.SqlFromEmbeddedFile(Assembly.GetExecutingAssembly(),
                "20250615153502_AddUpdateParcelsStoredProcedure.sql");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
