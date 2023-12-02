namespace Y2022;

public class Day3Tests(ITestOutputHelper helper) : DayTests(helper, new Day3())
{
    protected override string TestData =>
        """
        vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw
        """;

    protected override string ExpectedForTestInputPart1 => "157";
    protected override string ExpectedForTestInputPart2 => "70";
}
