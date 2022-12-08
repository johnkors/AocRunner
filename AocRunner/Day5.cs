using System.Collections;

public class Day5 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var stacks = new List<Stack>();
        var stackRows = loadedInput.SplitUntil(string.Empty).ToList();
        return stackRows.Count().ToString();
    }
}

public static class InputExtensions
{
    public static IEnumerable<string?> SplitUntil(this IEnumerable<string> rows, string row)
    {
        using IEnumerator<string> enumerator = rows.AsEnumerable().GetEnumerator();
        while (enumerator.MoveNext())
        {
            Framework.Logger(enumerator.Current);
            if(enumerator.Current == row)
            {
                Framework.Logger("Found what you're looking for");
                break;
            }
            yield return enumerator.Current;
        }
    }
}
