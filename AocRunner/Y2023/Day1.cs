using System.Text.RegularExpressions;

namespace Y2023;

public class Day1(ITestOutputHelper? helper = null) : IDaySolver
{
    private readonly ITestOutputHelper? _helper = helper ?? new TestOutputHelper();

    public string SolvePart1(string[] loadedInput) =>
        loadedInput.Sum(s => int.Parse($"{s.FirstOrDefault(char.IsDigit)}{s.LastOrDefault(char.IsDigit)}")).ToString();


    public string SolvePart2(string[] loadedInput)
    {
        const string ptrn = @"(one|two|three|four|five|six|seven|eight|nine)|(\d)";
        var leftToRight = new Regex(ptrn);
        var rightToLeft = new Regex(ptrn, RegexOptions.RightToLeft);
        return loadedInput.Sum(row =>
            {
                string rowNum = $"{ToDigit(leftToRight.Match(row).Value)}{ToDigit(rightToLeft.Match(row).Value)}";
                return int.Parse(rowNum);
            })
            .ToString();
    }

    readonly (string word, string digit)[] _nums = [
        ("one", "1"), ("two","2"), ("three","3"),
        ("four","4"), ("five","5"), ("six","6"),
        ("seven","7"), ("eight","8"), ("nine","9")
    ];

    string ToDigit(string wordOrDigit)
    {
        var word = _nums.FirstOrDefault(n => n.word == wordOrDigit).digit;
        return word ?? wordOrDigit;
    }
}
