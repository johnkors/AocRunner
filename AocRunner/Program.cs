var day = new Y2023.Day6();
var fw = Framework.Init(day, Console.WriteLine);
if (fw.HasUnsolvedParts())
{
    fw.PrintUnsolvedPartTask();
}

fw.Solve1(day);
fw.Solve2(day);
