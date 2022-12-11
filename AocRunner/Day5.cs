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
            // Framework.Logger("EXECUTING DIRECTIONS!");
            // Framework.Logger($"moving {direction.Amount} from {direction.From} to {direction.To}\n");
            // Framework.Logger("stack " +direction.From  + ":\n[" + string.Join("]\n[", stacks[direction.From].ToArray()) + "]");
            // Framework.Logger("stack " +direction.To  + ":\n[" + string.Join("]\n[", stacks[direction.To].ToArray()) + "]\n");

            while (directionAmount > 0)
            {
                // Framework.Logger("STATUS:");
                // Framework.Logger($"Moving 1 from Stack " +direction.From  + ":\n[" + string.Join("]\n[", stacks[direction.From].ToArray()) + "]");
                // Framework.Logger("To Stack " +direction.To  + ":\n[" + string.Join("]\n[", stacks[direction.To].ToArray()) + "]\n");
                if (stacks[direction.From].Count > 0)
                {
                    var crate = stacks[direction.From].Dequeue();

                    var newTo = new Queue<char>();
                    newTo.Enqueue(crate);
                    while(stacks[direction.To].Count > 0)
                    {
                        var dequeue = stacks[direction.To].Dequeue();
                        newTo.Enqueue(dequeue);
                    }

                    stacks[direction.To] = newTo;
                    // Framework.Logger($"*Moving [{crate}]*");
                }

                // Framework.Logger($"*NO CRATE TO MOVE. Stack {direction.From} was empty");
                directionAmount--;
            }
            // Framework.Logger($"\n{new string('*', 60)}");
            // Framework.Logger("  AFTER:");
            // Framework.Logger("  New Stack " +direction.From  + ":\n  [" + string.Join("]\n  [", stacks[direction.From].ToArray()) + "]");
            // Framework.Logger("  New Stack " +direction.To  + ":\n  [" + string.Join("]\n  [", stacks[direction.To].ToArray()) + "]\n");
            // Framework.Logger($"{new string('*', 60)}");
        }

        return string.Join("", stacks.OrderBy(c => c.Key).Select(s => s.Value.Count > 0 ? s.Value.Peek() : ' '));
    }

    public string SolvePart2(string[] loadedInput)
{
        var stacks = loadedInput.GetStackMapUntil(string.Empty);
        
        var directions = loadedInput.GetMatches();

        foreach (var direction in directions)
        {
            int directionAmount = direction.Amount;
            Framework.Logger("EXECUTING DIRECTIONS!");
            Framework.Logger($"moving {direction.Amount} from {direction.From} to {direction.To}\n");
            Framework.Logger("stack " +direction.From  + ":\n[" + string.Join("]\n[", stacks[direction.From].ToArray()) + "]");
            Framework.Logger("stack " +direction.To  + ":\n[" + string.Join("]\n[", stacks[direction.To].ToArray()) + "]\n");
            var crates9001 = new List<char>();
            
            while (directionAmount > 0)
            {
                crates9001.Add(stacks[direction.From].Dequeue());
                directionAmount--;
            }

            var stack = new Stack<char>(new Queue<char>());
            var newTo = new Queue<char>();
            
            foreach (var cratesToEnqueue in crates9001)
            {
                newTo.Enqueue(cratesToEnqueue);
            }

            foreach (var oldTo in stacks[direction.To])
            {
                newTo.Enqueue(oldTo);
            }

            stacks[direction.To] = newTo;
             
            Framework.Logger($"\n{new string('*', 60)}");
            Framework.Logger("  AFTER:");
            Framework.Logger("  New Stack " +direction.From  + ":\n  [" + string.Join("]\n  [", stacks[direction.From].ToArray()) + "]");
            Framework.Logger("  New Stack " +direction.To  + ":\n  [" + string.Join("]\n  [", stacks[direction.To].ToArray()) + "]\n");
            Framework.Logger($"{new string('*', 60)}");
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
