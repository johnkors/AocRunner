public class Day2Tests : DayTests<Day2>
{
    public Day2Tests(ITestOutputHelper helper) : base(helper) { }
    protected override string TestData =>
    """
    A Y
    B X
    C Z
    """;

    protected override string ExpectedForTestInputPart1 => "15";
    protected override string ExpectedForTestInputPart2 => "12";
}
