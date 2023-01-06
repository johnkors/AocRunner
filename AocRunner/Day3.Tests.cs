public class Day3Tests : DayTests
{
    public Day3Tests(ITestOutputHelper helper) : base(helper)
    {
        Daysolver = new Day3();
    }
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
