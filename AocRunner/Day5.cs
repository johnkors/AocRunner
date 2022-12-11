using System.Text.RegularExpressions;

public class Day5 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var stacks = loadedInput.GetStackMapUntil(string.Empty);
        var directionsList = loadedInput.GetDirections();

        foreach (var directions in directionsList)
        {
            new StackCraneMove().ModifyStacks(directions, stacks);
        }

        return string.Join("", stacks.OrderBy(c => c.Key).Select(s => s.Value.Peek()));
    }

    public string SolvePart2(string[] loadedInput)
    {
        var stacks = loadedInput.GetStackMapUntil(string.Empty);
        var directionsList = loadedInput.GetDirections();

        foreach (var directions in directionsList)
        {
            new QueueCraneMove().ModifyStacks(directions, stacks);
        }

        return string.Join("", stacks.OrderBy(c => c.Key).Select(s => s.Value.Peek()));
    }
}


internal abstract class CraneMove
{
    protected abstract void Pickup(char crate);
    protected abstract char PutDown();
    protected abstract int Count { get; }
    
    public void ModifyStacks((int Amount, int From, int To) direction, IDictionary<int, Queue<char>> stacks)
    {
        int directionAmount = direction.Amount;

        while (directionAmount > 0)
        {
            Pickup(stacks[direction.From].Dequeue());
            directionAmount--;
        }

        var newTo = new Queue<char>();

        while (Count > 0)
        {
            newTo.Enqueue(PutDown());
        }

        while (stacks[direction.To].Count > 0)
        {
            newTo.Enqueue(stacks[direction.To].Dequeue());
        }

        stacks[direction.To] = newTo;
    }
}

internal class QueueCraneMove : CraneMove
{
    private readonly Queue<char> _queue;

    public QueueCraneMove()
    {
        _queue = new Queue<char>();
    }

    protected override void Pickup(char crate)
    {
        _queue.Enqueue(crate);
    }

    protected override char PutDown()
    {
        return _queue.Dequeue();
    }

    protected override int Count => _queue.Count;
}

internal class StackCraneMove : CraneMove
{
    private readonly Stack<char> _stack;

    public StackCraneMove()
    {
        _stack = new Stack<char>();
    }

    protected override void Pickup(char crate)
    {
        _stack.Push(crate);
    }

    protected override char PutDown()
    {
        return _stack.Pop();
    }

    protected override int Count => _stack.Count;
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
