using System.Text.RegularExpressions;

public class Day5 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var stacks = loadedInput.GetStacks(string.Empty);
        var directionsList = loadedInput.GetDirections();

        foreach (var directions in directionsList)
        {
            int amount = directions.Amount;

            while (amount > 0)
            {
                stacks[directions.To].Push(stacks[directions.From].Pop());
                amount--;
            }
        }

        return string.Join("", stacks.Select(s => s.Value.Peek()));
    }

    public string SolvePart2(string[] loadedInput)
    {
        var stacks = loadedInput.GetStacks(string.Empty);
        var directionsList = loadedInput.GetDirections();
        
        foreach (var directions in directionsList)
        {
            int amount = directions.Amount;
            
            var stack = new Stack<char>();
            
            while (amount > 0)
            {
                stack.Push(stacks[directions.From].Pop());
                amount--;
            }

            while(stack.TryPop(out var crate))
            {
                stacks[directions.To].Push(crate);
            }
        }

        return string.Join("", stacks.Select(s => s.Value.Peek()));
    }
}

public static class InputExtensions
{
    public static IEnumerable<(int Amount,int From,int To)> GetDirections(this IEnumerable<string> rows)
    {
        var regex = new Regex(@"move (\d+) from (\d) to (\d)", RegexOptions.Compiled);
        using IEnumerator<string> enumerator = rows.AsEnumerable().GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (regex.Match(enumerator.Current) is { Success: true } match)
            {
                yield return (Amount: Matchvalue(1), From: Matchvalue(2), To: Matchvalue(3));

                int Matchvalue(int i)
                {
                    return int.Parse(match.Groups[i].Value);
                }
            }
        }
    }

    public static IDictionary<int, Stack<char>> GetStacks(this IEnumerable<string> rows, string row)
    {
        var listMap = new Dictionary<int, List<char>>();
        using IEnumerator<string> enumerator = rows.AsEnumerable().GetEnumerator();
        while (enumerator.MoveNext())
        {
            var i = 0;
            var prepended = enumerator.Current.ToCharArray().Prepend(' ').Prepend(' ').ToArray();
            foreach (var dork in prepended)
            {
                if (i % 4 == 0 && !char.IsWhiteSpace(dork))
                {
                    int stackNo = i / 4;
                    if (!listMap.ContainsKey(stackNo))
                    {
                        listMap.Add(stackNo, new List<char>());
                    }
                    char crateContent = prepended[i-1];
                    listMap[stackNo].Add(crateContent);
                }
                i++;
            }

            if(enumerator.Current == row)
            {
                break;
            }
        }

        var stackMap = new Dictionary<int, Stack<char>>();
        foreach (KeyValuePair<int,List<char>> queueMapEntry in listMap.OrderBy(c => c.Key))
        {
            var stack = new Stack<char>();
            var list = queueMapEntry.Value;
            list.Reverse();
            foreach (char crate in list)
            {
                stack.Push(crate);
            }
            stackMap.Add(queueMapEntry.Key, stack);
        }
        return stackMap;
    }
}
