using System.Text.RegularExpressions;

public class Day5 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var stacks = loadedInput.GetStackMapUntil(string.Empty);
        
        var directions = loadedInput.GetMatches();

        foreach (var direction in directions)
        {
            int directionAmount = direction.Amount;
         
            var newTo = new Queue<char>();
            var tmpStack = new Stack<char>();
            
            while (directionAmount > 0)
            {
                tmpStack.Push(stacks[direction.From].Dequeue());
                directionAmount--;
            }

            while(tmpStack.Count > 0)
            {
                newTo.Enqueue(tmpStack.Pop());
            }
            
            while(stacks[direction.To].Count > 0)
            {
                newTo.Enqueue(stacks[direction.To].Dequeue());
            }
            
            stacks[direction.To] = newTo;
            
        }

        return string.Join("", stacks.OrderBy(c => c.Key).Select(s => s.Value.Count > 0 ? s.Value.Peek() : ' '));
    }

    private static void LogBeforeStatus((int Amount, int From, int To) direction, IDictionary<int, Queue<char>> stacks)
    {
        Framework.Logger("EXECUTING DIRECTIONS!");
        Framework.Logger($"moving {direction.Amount} from {direction.From} to {direction.To}\n");
        Framework.Logger("stack " + direction.From + ":\n[" + string.Join("]\n[", stacks[direction.From].ToArray()) + "]");
        Framework.Logger("stack " + direction.To + ":\n[" + string.Join("]\n[", stacks[direction.To].ToArray()) + "]\n");
    }

    private static void LogAfterStatus((int Amount, int From, int To) direction, IDictionary<int, Queue<char>> stacks)
    {
        Framework.Logger($"\n{new string('*', 60)}");
        Framework.Logger("  AFTER:");
        Framework.Logger("  New Stack " + direction.From + ":\n  [" +
                         string.Join("]\n  [", stacks[direction.From].ToArray()) + "]");
        Framework.Logger("  New Stack " + direction.To + ":\n  [" + string.Join("]\n  [", stacks[direction.To].ToArray()) +
                         "]\n");
        Framework.Logger($"{new string('*', 60)}");
    }

    public string SolvePart2(string[] loadedInput)
{
        var stacks = loadedInput.GetStackMapUntil(string.Empty);
        
        var directions = loadedInput.GetMatches();

        foreach (var direction in directions)
        {
            int directionAmount = direction.Amount;
            
            var newTo = new Queue<char>();
            
            while (directionAmount > 0)
            {
                newTo.Enqueue(stacks[direction.From].Dequeue());
                directionAmount--;
            }

            while(stacks[direction.To].Count > 0)
            {
                newTo.Enqueue(stacks[direction.To].Dequeue());
            }

            stacks[direction.To] = newTo;
        }

        return string.Join("", stacks.OrderBy(c => c.Key).Select(s => s.Value.Count > 0 ? s.Value.Peek() : ' '));
    }
}

public static class InputExtensions
{
    public static IEnumerable<(int Amount,int From,int To)> GetMatches(this IEnumerable<string> rows)
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

    
    public static IDictionary<int, Queue<char>> GetStackMapUntil(this IEnumerable<string> rows, string row)
    {
        var stackMap = new Dictionary<int, Queue<char>>();
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
                        stackMap.Add(stackNo, new Queue<char>());
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
