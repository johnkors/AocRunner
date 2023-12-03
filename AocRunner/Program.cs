using Y2023;

var day = new Day2();
var fw = Framework.Init(day, Console.WriteLine);
if (fw.HasUnsolvedParts())
{
    fw.PrintUnsolvedPartTask();
}

fw.Solve1(day);
fw.Solve2(day);
