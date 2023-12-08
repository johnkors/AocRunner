using System.Text.RegularExpressions;

namespace Y2023;

public class Day1 : IDaySolver
{
    public string SolvePart1(string[] loadedInput) =>
        loadedInput
            .Sum(s => int.Parse($"{Regex.Match(s, @"\d")}" +
                                $"{Regex.Match(s, @"\d", RegexOptions.RightToLeft)}"))
            .ToString();

    const string ptrn = @"(one|two|three|four|five|six|seven|eight|nine)|(\d)";

    public string SolvePart2(string[] loadedInput)
    {
        return loadedInput
            .Sum(row => int.Parse(
                $"{ToDigit(Regex.Match(row, ptrn).Value)}{ToDigit(Regex.Match(row, ptrn, RegexOptions.RightToLeft).Value)}"))
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
