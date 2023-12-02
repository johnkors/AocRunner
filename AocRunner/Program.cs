using Y2023;

var day = new Day1();
// Test.Verify(new Day8Tests(new TestOutputHelper()));
var fw = Framework.Init(day, Console.WriteLine);
if (fw.HasUnsolvedParts())
{
    fw.PrintUnsolvedPartTask();
}

fw.Solve1(day);
fw.Solve2(day);
