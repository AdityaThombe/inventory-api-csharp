// Demo target for ProofPatch: intentionally vulnerable. Never deploy this code.
using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Inventory;

public class Stock
{
    private readonly SqliteConnection _db;

    public Stock()
    {
        _db = new SqliteConnection("Data Source=:memory:");
        _db.Open();
        using var setup = _db.CreateCommand();
        setup.CommandText = "CREATE TABLE items (sku TEXT, qty INTEGER); INSERT INTO items VALUES ('apple', 5), ('pear', 2);";
        setup.ExecuteNonQuery();
    }

    /// <summary>Quantities for an exact SKU.</summary>
    public List<long> Quantity(string sku)
    {
        using var cmd = _db.CreateCommand();
        cmd.CommandText = "SELECT qty FROM items WHERE sku = '" + sku + "'";
        var result = new List<long>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) result.Add(reader.GetInt64(0));
        return result;
    }

    /// <summary>Label printed by the warehouse tool.</summary>
    public static string Label(string sku)
    {
        var info = new ProcessStartInfo("/bin/sh", "-c \"echo LABEL-" + sku + "\"") { RedirectStandardOutput = true };
        using var process = Process.Start(info)!;
        var output = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();
        return output;
    }

    /// <summary>Contents of a stored delivery note.</summary>
    public static string ReadNote(string root, string name)
    {
        return File.ReadAllText(Path.Combine(root, name));
    }

    public static string Title(string sku) => sku.Length == 0 ? sku : char.ToUpper(sku[0]) + sku[1..];
}
