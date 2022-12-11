public class Day5 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        return Day5Solve(loadedInput, (directions, stacks) =>
        {
            int amount = directions.Amount;

            var queue = new Queue<char>();

            while (amount > 0)
            {
                queue.Enqueue(stacks[directions.From].Pop());
                amount--;
            }

            while (queue.TryDequeue(out var crate))
            {
                stacks[directions.To].Push(crate);
            }
        });
    }

    public string SolvePart2(string[] loadedInput)
    {
        return Day5Solve(loadedInput, (directions, stacks) =>
        {
            int amount = directions.Amount;

            var stack = new Stack<char>();

            while (amount > 0)
            {
                stack.Push(stacks[directions.From].Pop());
                amount--;
            }

            while (stack.TryPop(out var crate))
            {
                stacks[directions.To].Push(crate);
            }
        });
    }

    record Directions(int Amount, int From, int To);

    private static string Day5Solve(string[] loadedInput, Action<Directions, IDictionary<int, Stack<char>>> mutateStacks)
    {
        (int endRow, IEnumerable<string> stackRows) = loadedInput.GetRowsUntilInReverse(string.Empty);
        var directionsList = loadedInput[endRow..].GetByPattern<Directions>(@"move (\d+) from (\d) to (\d)", m => new(m.AsInt(1), m.AsInt(2), m.AsInt(3)));
        
        var stacks = ConvertToStackMap(stackRows);
        
        foreach (var directions in directionsList)
        {
            mutateStacks(directions, stacks);
        }

        return string.Join("", stacks.Select(s => s.Value.Peek()));
    }

    private static IDictionary<int, Stack<char>> ConvertToStackMap(IEnumerable<string> stackRows)
    {
        var stackMap = new Dictionary<int, Stack<char>>();

        using var rowsReader = stackRows.AsEnumerable().GetEnumerator();
        while (rowsReader.MoveNext())
        {
            var i = 0;
            var prepended = rowsReader.Current.ToCharArray().Prepend(' ').Prepend(' ').ToArray();
            foreach (var dork in prepended)
            {
                if (i % 4 == 0 && !char.IsWhiteSpace(dork))
                {
                    int stackNo = i / 4;
                    if (!stackMap.ContainsKey(stackNo))
                    {
                        stackMap.Add(stackNo, new Stack<char>());
                    }
                    char crateContent = prepended[i-1];
                    stackMap[stackNo].Push(crateContent);
                }
                i++;
            }
        }

        return stackMap;
    }
}
