namespace Y2023;

public class Day8Tests(ITestOutputHelper helper) : DayTests(helper, new Day8(helper))
{
    protected override string TestData =>
        """
        RL

        AAA = (BBB, CCC)
        BBB = (DDD, EEE)
        CCC = (ZZZ, GGG)
        DDD = (DDD, DDD)
        EEE = (EEE, EEE)
        GGG = (GGG, GGG)
        ZZZ = (ZZZ, ZZZ)
        """;

    protected override string ExpectedForTestInputPart1 => "2";

    protected override string ExpectedForInputPart1 => "18113";

    protected override string TestData2 =>
        """
        LR

        11A = (11B, XXX)
        11B = (XXX, 11Z)
        11Z = (11B, XXX)
        22A = (22B, XXX)
        22B = (22C, 22C)
        22C = (22Z, 22Z)
        22Z = (22B, 22B)
        XXX = (XXX, XXX)
        """;

    protected override string ExpectedForTestInputPart2 => "6";

    protected override string ExpectedForInputPart2 => "12315788159977";

    [Xunit.Fact]
    public void Multi()
    {
        var input = """
                    LLR

                    AAA = (BBB, BBB)
                    BBB = (AAA, ZZZ)
                    ZZZ = (ZZZ, ZZZ)
                    """;

        var res = base.Daysolver.SolvePart1(input.Split("\n"));
        Xunit.Assert.Equal("6", res);
    }
}
