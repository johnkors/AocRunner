public class Day2Tests : DayTests
{
    public Day2Tests(ITestOutputHelper helper) : base(helper)
    {
    }
    
    protected override IDaySolver Daysolver => new Day2();
    
    protected override string TestData =>
    """
    A Y
    B X
    C Z
    """;

    protected override string ExpectedForTestInputPart1 => "15";
    protected override string ExpectedForTestInputPart2 => "12";
}
