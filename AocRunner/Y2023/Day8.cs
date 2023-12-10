using System.Text.RegularExpressions;
using Map = System.Collections.Generic.Dictionary<string, (string Left, string Right)>;

namespace Y2023;

public class Day8(ITestOutputHelper helper) : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var instructions = loadedInput[0].ToCharArray();
        var map = ParseMap(loadedInput[2..]);
        var count = CountFromAAAtoZZZ(instructions, map);
        return $"{count}";
    }

    public string SolvePart2(string[] loadedInput)
    {
        var instructions = loadedInput[0].ToCharArray();
        var map = ParseMap(loadedInput[2..]);
        var count = CountSimultanousAtoZ(instructions, map);
        return $"{count}";
    }

    private long CountSimultanousAtoZ(char[] instructions, Map map)
    {
        string[] startingPoints = map.Keys.Where(k => k.EndsWith("A")).ToArray();

        List<long> counters = [];

        foreach (string startingPoint in startingPoints)
        {
            var current = startingPoint;
            var endsWithZ = false;
            var counter = 0;

            while (!endsWithZ)
            {
                var instruction = instructions[counter % instructions.Length];
                current = instruction switch
                {
                    'L' => map[current].Left,
                    'R' => map[current].Right
                };
                counter++;
                if (!current.EndsWith("Z"))
                {
                    continue;
                }

                endsWithZ = true;
                counters.Add(counter);
            }
        }

        return counters.Aggregate(LCM);
    }
    long LCM(long a, long b) => (a * b) / GCD(a, b);

    long GCD(long a, long b)
    {
        while (b != 0)
        {
            long remainder = a % b;
            a = b;
            b = remainder;
        }

        return a;
    }

    private int CountFromAAAtoZZZ(char[] instructions, Map map)
    {
        var counter = 0;
        var start = "AAA";
        var end = "ZZZ";

        while(start != end)
        {
            var instruction = instructions[counter % instructions.Length];
            start = instruction switch
            {
                'L' => map[start].Left,
                'R' => map[start].Right
            };
            counter++;
        }

        return counter;
    }

    private Map ParseMap(string[] mapRows)
    {
        var map = new Map();

        for (int index = 0; index < mapRows.Length; index++)
        {
            var row = mapRows[index];
            var match = Regex.Match(row, @"(\w+) = \((\w+), (\w+)\)");
            map[match.Groups[1].Value] = (match.Groups[2].Value, match.Groups[3].Value);
        }

        return map;
    }
}
