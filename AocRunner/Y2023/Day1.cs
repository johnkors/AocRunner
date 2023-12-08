using System.Text.RegularExpressions;

namespace Y2023;

public class Day1 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        string pattern = @"\d";
        return loadedInput.Sum(s =>
            {
                string rowNum = $"{Regex.Match(s, pattern)}" +
                                $"{Regex.Match(s, pattern, RegexOptions.RightToLeft)}";
                return int.Parse(rowNum);
            })
            .ToString();
    }

    public string SolvePart2(string[] loadedInput)
    {
        const string ptrn = @"(one|two|three|four|five|six|seven|eight|nine)|(\d)";
        return loadedInput.Sum(row =>
            {
                string rowNum = $"{ToDigit(Regex.Match(row, ptrn).Value)}" +
                                $"{ToDigit(Regex.Match(row, ptrn, RegexOptions.RightToLeft).Value)}";
                return int.Parse(rowNum);
            })
            .ToString();
    }

    static int ToDigit(string word) => word switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => int.Parse(word)
    };

}
