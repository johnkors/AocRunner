using System.Text.RegularExpressions;

public class Day5 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var stacks = loadedInput.GetStackMapUntil(string.Empty);
        Framework.Logger(Log.ToJson(stacks));
        var directions = loadedInput.GetMatches();
        var i = 1;

        foreach (var direction in directions)
        {
            int directionAmount = direction.Amount;
            while (directionAmount > 0)
            {
                Framework.Logger(direction.ToString());
                if (stacks[direction.From].Count > 0)
                {
                    var crate = stacks[direction.From].Dequeue();

                    var newTo = new Queue();
                    newTo.Enqueue(crate);
                    while(stacks[direction.To].Count > 0)
                    {
                        if (stacks[direction.To].Count > 0)
                        {
                            var dequeue = stacks[direction.To].Dequeue();
                            newTo.Enqueue(dequeue);
                        }
                    }

                    stacks[direction.To] = newTo;
                }

                directionAmount--;
            }
        }

        return string.Join("", stacks.OrderBy(c => c.Key).Select(s => s.Value.Count > 0 ? s.Value.Peek() : ""));
    }
}

public static class InputExtensions
{
    public static IEnumerable<(int Amount,int From,int To)> GetMatches(this IEnumerable<string> rows)
    {
        var regex = new Regex(@"move (\d) from (\d) to (\d)", RegexOptions.Compiled);
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

    
    public static IDictionary<int, Queue> GetStackMapUntil(this IEnumerable<string> rows, string row)
    {
        var stackMap = new Dictionary<int, Queue>();
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
                    if (!stackMap.ContainsKey(stackNo))
                    {
                        stackMap.Add(stackNo, new Queue());
                    }
                    char crateContent = prepended[i-1];
                    stackMap[stackNo].Enqueue(crateContent);
                }
                i++;
            }

            if(enumerator.Current == row)
            {
                break;
            }
        }

        return stackMap;
    }
}
