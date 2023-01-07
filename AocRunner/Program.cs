TestOutputHelper testOutputHelper = new TestOutputHelper();
var day = new Day7(testOutputHelper);
Test.Verify(new Day7Tests(testOutputHelper));
var fw = Framework.Init(day, Console.WriteLine);
if (fw.HasUnsolvedParts())
{
    fw.PrintUnsolvedPartTask();
}

fw.Solve1(day);
fw.Solve2(day);
