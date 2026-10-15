using Inventory;
using Xunit;

namespace Inventory.Tests;

public class StockTests
{
    [Fact]
    public void QuantityForKnownSku() => Assert.Equal(new List<long> { 5 }, new Stock().Quantity("apple"));

    [Fact]
    public void QuantityForUnknownSkuIsEmpty() => Assert.Empty(new Stock().Quantity("kiwi"));

    [Fact]
    public void LabelForSku() => Assert.Equal("LABEL-apple", Stock.Label("apple"));

    [Fact]
    public void ReadsNote()
    {
        var dir = Directory.CreateTempSubdirectory().FullName;
        File.WriteAllText(Path.Combine(dir, "n1.txt"), "delivered");
        Assert.Equal("delivered", Stock.ReadNote(dir, "n1.txt"));
    }

    [Fact]
    public void Title() => Assert.Equal("Apple", Stock.Title("apple"));
}
