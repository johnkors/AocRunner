using Xunit;

public class Day6Tests : DayTests
{
    public Day6Tests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        Daysolver = new Day6();
    }

    protected override string TestData =>
        """
        mjqjpqmgbljsphdztnvjfqwrcgsmlb
        """;

    protected override string ExpectedForTestInputPart1 => "7";
    protected override string ExpectedForTestInputPart2 => "19";

    protected override string ExpectedForInputPart1 => "1794";
    
    protected override string ExpectedForInputPart2 => "2851";

    [Theory]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", "5")]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", "6")]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", "10")]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", "11")]
    public void TestPart1_WithMoreTestData(string stream, string markerIndex)
    {
        var sln = Framework.Init(Daysolver, _helper.WriteLine).Solve(Daysolver, s => s.SolvePart1, loadInput:() => stream);
        Assert.Equal(markerIndex, sln);
    }
    
    [Theory]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", "23")]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", "23")]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", "29")]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", "26")]
    public void TestPart2_WithMoreTestData(string stream, string markerIndex)
    {
        var sln = Framework.Init(Daysolver, _helper.WriteLine).Solve(Daysolver, s => s.SolvePart2, loadInput:() => stream);
        Assert.Equal(markerIndex, sln);
    }
}
