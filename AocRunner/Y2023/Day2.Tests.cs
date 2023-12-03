namespace Y2023;

public class Day2Tests(ITestOutputHelper helper) : DayTests(helper, new Day2())
{
    protected override string TestData =>
        """
        Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
        Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
        Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
        Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green;
        """;

    protected override string ExpectedForTestInputPart1 => "8";

    protected override string ExpectedForInputPart1 => "2239";

    protected override string ExpectedForTestInputPart2 => "2286";

    protected override string ExpectedForInputPart2 => "83435";
}
