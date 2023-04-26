using System.Data;
using Microsoft.Data.Sqlite;
using XReports.DataReader;
using XReports.DocsSamples.Common;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.Table;

await using SqliteConnection connection = new SqliteConnection("Data Source=:memory:");

await connection.OpenAsync();

// Setup database
await using SqliteCommand command = connection.CreateCommand();
command.CommandText = "CREATE TABLE Customers(Name text, Email text, Age integer)";
await command.ExecuteNonQueryAsync();
command.CommandText =
    @"INSERT INTO Customers(Name, Email, Age) VALUES
        ('John Doe', 'john@example.com', 23),
        ('Jane Doe', 'jane@example.com', 22)";
await command.ExecuteNonQueryAsync();

// Type of data source is IDataReader.
ReportSchemaBuilder<IDataReader> builder = new ReportSchemaBuilder<IDataReader>();

// Callbacks accept IDataReader.
builder.AddColumn("Name", (IDataReader x) => x.GetString(0));
builder.AddColumn("Email", (IDataReader x) => x.GetString(1));
builder.AddColumn("Age", (IDataReader x) => x.GetInt32(2));

// Build schema.
IReportSchema<IDataReader> schema = builder.BuildVerticalSchema();

// Get IDataReader.
command.CommandText = "SELECT Name, Email, Age FROM Customers";
IDataReader dataReader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

// AsEnumerable is an extension method that allows iterating over IDataReader.
IReportTable<ReportCell> reportTable = schema.BuildReportTable(dataReader.AsEnumerable());

// SimpleConsoleWriter is custom class that writes report table data into console.
new SimpleConsoleWriter().Write(reportTable);

