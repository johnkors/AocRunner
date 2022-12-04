public class Day2Tests : DayTests
{
    protected override string TestData =>
    """
    A Y
    B X
    C Z
    """;

    protected override string ExpectedForTestInputPart1 => "15";
    protected override string ExpectedForTestInputPart2 => "12";

    public Day2Tests() : base(new Day2()) { }
}
