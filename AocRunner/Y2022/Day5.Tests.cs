namespace Y2022;

public class Day5Tests(ITestOutputHelper helper) : DayTests(helper, new Day5())
{
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
    
    protected override string ExpectedForTestInputPart2 => "MCD";

    protected override string ExpectedForInputPart1 => "VJSFHWGFT";
    
    protected override string ExpectedForInputPart2 => "LCTQFBVZV";


}
