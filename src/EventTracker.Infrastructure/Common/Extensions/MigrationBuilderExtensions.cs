using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace EventTracker.Infrastructure.Common.Extensions;

public static class MigrationBuilderExtensions
{
    public static OperationBuilder<SqlOperation> SqlFromEmbeddedFile(this MigrationBuilder self, Assembly assembly,
        string sqlFileName, bool suppressTransaction = false)
    {
        var resourceName = assembly.GetManifestResourceNames()
            .SingleOrDefault(x => x.EndsWith(sqlFileName));
        if (resourceName == null)
        {
            throw new FileNotFoundException("Unable to find the SQL file", sqlFileName);
        }

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException("Unable to find the SQL file from an embedded resource", resourceName);
        }

        using var reader = new StreamReader(stream);
        var sql = reader.ReadToEnd();
        return self.Sql(sql, suppressTransaction);
    }
}
