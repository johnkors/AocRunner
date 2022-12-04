public class Day1Tests : DayTests<Day1>
{
    protected override string TestData =>
    """
    1000
    2000
    3000

    4000

    5000
    6000

    7000
    8000
    9000

    10000
    """;

    protected override string  ExpectedForTestInputPart1 => "24000";
    protected override string  ExpectedForTestInputPart2 => "45000";
}
