namespace Y2022;

public class Day4 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        int count = loadedInput.GetCountOf(FullOverlaps);
        return count.ToString();
    }

    public string SolvePart2(string[] loadedInput)
    {
        var count = loadedInput.GetCountOf(PartialOverlaps);
        return count.ToString();
    }

    static bool FullOverlaps(Range r1, Range r2)
    {
        return r1.Start.Value <= r2.Start.Value && r2.End.Value <= r1.End.Value;
    }

    static bool PartialOverlaps(Range r1, Range r2)
    {
        return r1.Start.Value <= r2.Start.Value && r2.Start.Value <= r1.End.Value;
    }
}

static class Day4Extensions
{
    public static int GetCountOf(this string[] rows, Func<Range, Range, bool> rangeCheck)
    {
        var count = 0;
        foreach (string row in rows)
        {
            (Range r1, Range r2) = ToRanges(row);
            if (rangeCheck(r1, r2) || rangeCheck(r2, r1))
                count++;
        }
        return count;
    }

    private static (Range r1, Range r2) ToRanges(string row)
    {
        string[] strings = row.Split(',');
        return (ToRange(strings[0]), ToRange(strings[1]));
    }

    private static Range ToRange(string str)
    {
        string[] rangeStr = str.Split('-');
        return new Range(int.Parse(rangeStr[0]), int.Parse(rangeStr[1]));
    }
}