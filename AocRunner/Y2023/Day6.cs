using System.Text.RegularExpressions;

namespace Y2023;

public class Day6 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var races = GetData(loadedInput);
        var winVariants = races.Select(CountWinVariants);
        return $"{winVariants.Aggregate((x, y) => x * y)}";
    }

    public string SolvePart2(string[] loadedInput)
    {
        var races = GetData(loadedInput.Select(r => r.Replace(" ", "")).ToArray());
        var winVariants = races.Select(CountWinVariants);
        return $"{winVariants.Aggregate((x, y) => x * y)}";
    }

    long CountWinVariants(Race race)
    {
        var duration = race.RecordDuration;
        long count = 0;
        for (long velocity = 0; velocity < duration; velocity++)
        {
            long leftoverTime = (duration - velocity);
            var achievedDistance = velocity * leftoverTime;
            if (achievedDistance > race.RecordDistance)
            {
                count++;
            }
        }

        return count;
    }

    private IEnumerable<Race> GetData(string[] loadedInput)
    {
        var times = Regex.Matches(loadedInput.First(), @"(\d+)").Select(m => long.Parse(m.Value)).ToArray();
        var distances = Regex.Matches(loadedInput.Last(), @"(\d+)").Select(m => long.Parse(m.Value)).ToArray();
        return times.Select((t, i) => new Race(t, distances[i]));
    }

    record Race(long RecordDuration, long RecordDistance);

}
