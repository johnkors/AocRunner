namespace Y2022;

public class Day4Tests(ITestOutputHelper helper) : DayTests(helper, new Day4())
{
    protected override string TestData =>
        """
        2-4,6-8
        2-3,4-5
        5-7,7-9
        2-8,3-7
        6-6,4-6
        2-6,4-8
        """;

    protected override string ExpectedForTestInputPart1 => "2";

    protected override string ExpectedForTestInputPart2 => "4";
}
