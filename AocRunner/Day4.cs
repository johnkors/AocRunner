public class Day4 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var count = 0;
        foreach (string row in loadedInput)
        {
            string[] strings = row.Split(',');
            (Range r1, Range r2) = (ToRange(strings[0]), ToRange(strings[1]));
            if (RangeContainsRange(r1, r2) || RangeContainsRange(r2, r1))
                count++;
        }
        return count.ToString();
    }

    public string SolvePart2(string[] loadedInput)
    {
        var count = 0;
        foreach (string row in loadedInput)
        {
            string[] strings = row.Split(',');
            (Range r1, Range r2) = (ToRange(strings[0]), ToRange(strings[1]));
            if (StartsWithin(r1, r2) || StartsWithin(r2, r1))
                count++;
        }
        return count.ToString();
    }

    private Range ToRange(string str)
    {
        string[] rangeStr = str.Split('-');
        return new Range(int.Parse(rangeStr[0]), int.Parse(rangeStr[1]));
    }

    static bool RangeContainsRange(Range r1, Range r2)
    {
        return r1.Start.Value <= r2.Start.Value && r2.End.Value <= r1.End.Value;
    }

    static bool StartsWithin(Range r1, Range r2)
    {
        return r1.Start.Value <= r2.Start.Value && r2.Start.Value <= r1.End.Value;
    }
}
