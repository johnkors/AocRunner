namespace Y2022;

public class Day3 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var priorityMap = BuildPriorityMap();
        var sum = 0;
        foreach (string ruckSack in loadedInput)
        {
            var compartments = ruckSack.Chunk(ruckSack.Length / 2);
            var compartment1 = compartments.First();
            var compartment2 = compartments.Last();
            var inBoth = compartment1.Intersect(compartment2);
            sum += inBoth.Sum(dup => priorityMap[dup]);
        }

        return sum.ToString();
    }

    public string SolvePart2(string[] loadedInput)
    {
        var priorityMap = BuildPriorityMap();
        var groups = loadedInput.Chunk(3);
        var sum = 0;

        foreach (string[] group in groups)
        {
            IEnumerable<char> intersects = group.Select(c => c.AsEnumerable()).Aggregate((x, y) => x.Intersect(y));
            sum += intersects.Sum(dup => priorityMap[dup]);
        }

        return sum.ToString();
    }

    private Dictionary<char, int> BuildPriorityMap()
    {
        int priority = 1;
        var dic = new Dictionary<char, int>();

        foreach (char c in Enumerable.Range('a', 26))
        {
            dic.Add(c, priority);
            priority++;
        }

        foreach (char c in Enumerable.Range('A', 26))
        {
            dic.Add(c, priority);
            priority++;
        }

        return dic;
    }
}
