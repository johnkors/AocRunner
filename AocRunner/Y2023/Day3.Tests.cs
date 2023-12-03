using Xunit;

namespace Y2023;

public class Day3Tests(ITestOutputHelper helper) : DayTests(helper, new Day3())
{
    protected override string TestData =>
        """
        467..114..
        ...*......
        ..35..633.
        ......#...
        617*......
        .....+.58.
        ..592.....
        ......755.
        ...$.*....
        .664.598..
        """;

    protected override string ExpectedForTestInputPart1 => "4361";

    protected override string ExpectedForInputPart1 => "507214";

    protected override string ExpectedForTestInputPart2 => "467835";

    protected override string ExpectedForInputPart2 => "72553319";

    [Fact]
    public void DuplicateNumbers()
    {
        var testdata = """
                       .......
                       .52.52.
                       .......
                       """;
        var rows = testdata.Split(Environment.NewLine).ToArray();

        var data = ((Daysolver as Day3)!).GetData(rows);
        Assert.Equal(2, data.Numbers.Count);

        var first = data.Numbers.First();
        Assert.Equal(1, first.ColumnIndices[0]);
        Assert.Equal(2, first.ColumnIndices[1]);

        var last = data.Numbers.Last();
        Assert.Equal(4, last.ColumnIndices[0]);
        Assert.Equal(5, last.ColumnIndices[1]);
    }
}
