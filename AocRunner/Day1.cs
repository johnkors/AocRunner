public class Day1 : IDaySolver
{
    public int Day => 1;

    public string SolvePart1(string[] loadedInput)
    {
        int largestElf = 0;
        int currentElfSum = 0;

        foreach (var meal in loadedInput)
        {
            if (string.IsNullOrEmpty(meal))
            {
                if (currentElfSum > largestElf)
                {
                    largestElf = currentElfSum;
                }

                currentElfSum = 0;
            }
            else
            {
                currentElfSum += int.Parse(meal);
            }
        }

        return Math.Max(largestElf, currentElfSum).ToString();
    }

    public string SolvePart2(string[] loadedInput) => "test";

    public string TestData =>
"""
1000
2000
3000

4000

5000
6000

7000
8000
9000

10000
""";
}