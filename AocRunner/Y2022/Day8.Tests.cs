namespace Y2022;

public class Day8Tests(ITestOutputHelper testOutputHelper) : DayTests(testOutputHelper, new Day8())
{
    protected override string TestData =>
        """
        30373
        25512
        65332
        33549
        35390
        """;

    protected override string ExpectedForTestInputPart1 => "21";
    
    protected override string ExpectedForTestInputPart2 => "8";
    
    protected override string ExpectedForInputPart1 => "1647";

}
