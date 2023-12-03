using Xunit;

namespace Y2023;

public class Day1Tests(ITestOutputHelper helper) : DayTests(helper, new Day1(helper))
{
    protected override string TestData =>
        """
        1abc2
        pqr3stu8vwx
        a1b2c3d4e5f
        treb7uchet
        """;

    protected override string ExpectedForTestInputPart1 => "142";

    protected override string ExpectedForInputPart1 => "52974";

    protected override string TestData2 =>
        """
        two1nine
        eightwothree
        abcone2threexyz
        xtwone3four
        4nineeightseven2
        zoneight234
        7pqrstsixteen
        """;

    protected override string ExpectedForTestInputPart2 => "281";

    protected override string ExpectedForInputPart2 => "53340";

    [Fact]
    public void RightToLeft()
    {
        var solved = base.Daysolver.SolvePart2(["8seveneightwox"]);
        Assert.Equal("82", solved);
    }

}
