public class Day5Tests : DayTests<Day5>
{
    public Day5Tests(ITestOutputHelper helper) : base(helper) { }

    protected override string TestData =>
    """
        [D]    
    [N] [C]    
    [Z] [M] [P]
     1   2   3 

    move 1 from 2 to 1
    move 3 from 1 to 3
    move 2 from 2 to 1
    move 1 from 1 to 2
    """;
   
    protected override string ExpectedForTestInputPart1 => "CMZ";
    protected override string ExpectedForTestInputPart2 => "Not implemented";
}