namespace Y2022;

public class Day1 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        int largestElf = 0;
        int currentElfSum = 0;

        foreach (var row in loadedInput)
        {
            if (row == "")
            {
                if (currentElfSum > largestElf)
                {
                    largestElf = currentElfSum;
                }


                currentElfSum = 0;
            }
            else
            {
                currentElfSum += int.Parse(row);
            }
        }

        return Math.Max(largestElf, currentElfSum).ToString();
    }

    private record Elf()
    {
        public int Calories { get; set; }
    }

    public string SolvePart2(string?[] loadedInput)
    {
        var elves = new List<Elf>();
        var currentElf = new Elf();
        foreach (var meal in loadedInput)
        {
            if (string.IsNullOrEmpty(meal))
            {
                currentElf = new Elf();
                elves.Add(currentElf);
            }
            else
            {
                currentElf.Calories += int.Parse(meal);
            }
        }

        return elves.OrderByDescending(c => c.Calories).Take(3).Sum(c => c.Calories).ToString();
    }
}
