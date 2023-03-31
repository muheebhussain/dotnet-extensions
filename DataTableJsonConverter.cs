To create a custom JsonConverter for serializing a DataTable to a dictionary of key-value pairs in .NET 6, you can follow the steps below:

Create a new class, e.g., DataTableToDictionaryJsonConverter, that inherits from JsonConverter<DataTable>.

Override the Read and Write methods to handle the deserialization and serialization, respectively. For this specific case, you only need to implement the Write method, as you want to serialize the DataTable to a dictionary.

Here's an example implementation:

using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DataTableToDictionaryJsonConverter : JsonConverter<DataTable>
{
    public override DataTable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // You only want to serialize, so you can throw a NotImplementedException for the Read method
        throw new NotImplementedException("Deserialization of DataTable to dictionary is not supported.");
    }

    public override void Write(Utf8JsonWriter writer, DataTable table, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (DataRow row in table.Rows)
        {
            if (row[0] != DBNull.Value)
            {
                string key = row[0].ToString();
                Dictionary<string, object> rowData = new();

                for (int columnIndex = 1; columnIndex < table.Columns.Count; columnIndex++)
                {
                    DataColumn column = table.Columns[columnIndex];
                    rowData[column.ColumnName] = row[columnIndex] == DBNull.Value ? null : row[columnIndex];
                }

                JsonSerializer.Serialize(writer, key, options);
                JsonSerializer.Serialize(writer, rowData, options);
            }
        }

        writer.WriteEndObject();
    }
}
To use this custom JsonConverter, you can add it to the JsonSerializerOptions:
var dataTable = new DataTable();
// Fill your dataTable here

JsonSerializerOptions options = new();
options.Converters.Add(new DataTableToDictionaryJsonConverter());

string json = JsonSerializer.Serialize(dataTable, options);
