namespace Y2023;

public class Day6Tests(ITestOutputHelper helper) : DayTests(helper, new Day6())
{
    protected override string TestData =>
        """
        Time:      7  15   30
        Distance:  9  40  200
        """;

    protected override string ExpectedForTestInputPart1 => "288";

    protected override string ExpectedForInputPart1 => "741000";

    protected override string ExpectedForTestInputPart2 => "71503";

    protected override string ExpectedForInputPart2 => "38220708";
}
