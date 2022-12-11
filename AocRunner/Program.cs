var day = new Day5();
Test.Verify(new Day5Tests(new TestOutputHelper()));
var fw = Framework.Init(day, Console.WriteLine);
if (fw.HasUnsolvedParts())
{
    fw.PrintUnsolvedPartTask();
}

fw.Solve1(day, askForSubmit:true);
fw.Solve2(day, askForSubmit:true);
