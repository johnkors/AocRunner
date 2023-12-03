using Y2023;


var day = new Day1(new FakeHelper());
Test.Verify(new Day1Tests(new FakeHelper()));
var fw = Framework.Init(day, Console.WriteLine);
if (fw.HasUnsolvedParts())
{
    fw.PrintUnsolvedPartTask();
}

fw.Solve1(day);
fw.Solve2(day);
