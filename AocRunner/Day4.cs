public class Day4 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var count = 0;
        foreach (string row in loadedInput)
        {
            string[] strings = row.Split(',');
            var (range1, range2) = (ToRange(strings[0]), ToRange(strings[1]));
            if (HasFullOverLap((range1, range2)))
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
            var (range1, range2) = (ToRange(strings[0]), ToRange(strings[1]));
            if (HasOverLap((range1, range2)))
                count++;
        }
        return count.ToString();
    }

    private Range ToRange(string str)
    {
        string[] rangeStr = str.Split('-');
        return new Range(int.Parse(rangeStr[0]), int.Parse(rangeStr[1]));
    }

    private bool HasFullOverLap((Range Range1, Range Range2) ranges)
    {
        return ranges switch
        {
            { Range1: var r1, Range2: var r2 } when RangeContainsRange(r1, r2) || RangeContainsRange(r2, r1) => true,
            _ => false
        };

        bool RangeContainsRange(Range r1, Range r2)
        {
            return r1.Start.Value <= r2.Start.Value && r2.End.Value <= r1.End.Value;
        }
    }

    private bool HasOverLap((Range Range1, Range Range2) ranges)
    {
        return ranges switch
        {
            { Range1: var r1, Range2: var r2 } when StartsWithin(r1, r2) || StartsWithin(r2, r1) => true,
            _ => false
        };

        bool StartsWithin(Range r1, Range r2)
        {
            return r1.Start.Value <= r2.Start.Value && r2.Start.Value <= r1.End.Value;
        }
    }
}
