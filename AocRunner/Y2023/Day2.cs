using System.Text.RegularExpressions;

namespace Y2023;

public class Day2 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var games = ParseGames(loadedInput);
        return games.Where(g => g.ColorPicks.All(p => p switch
                {
                    { Color: "red", ColorCount: <= 12 } or
                    { Color: "green", ColorCount: <= 13 } or
                    { Color: "blue", ColorCount: <= 14 } => true,
                    _ => false
                }))
            .Sum(g => g.Id).ToString();
    }

    public string SolvePart2(string[] loadedInput)
    {
        var games = ParseGames(loadedInput);
        return games.Select(g => g.ColorPicks
            .GroupBy(c => c.Color)
            .Select(group => group.Max(pick => pick.ColorCount))
            .Aggregate((x,y) => x*y))
            .Sum().ToString();
    }

    private static IEnumerable<Game> ParseGames(string[] loadedInput)
    {
        List<Game> games = [];
        var id = 1;
        foreach (string row in loadedInput)
        {
            var game = new Game(id);
            id++;

            var matches = Regex.Matches(row, @"(\d+)\s(\w+)");
            foreach (Match colorPickMatch in matches)
            {
                game.ColorPicks.Add(new(int.Parse(colorPickMatch.Groups[1].Value), colorPickMatch.Groups[2].Value));
            }
            games.Add(game);
        }

        return games;
    }

    record Game(int Id)
    {
        public readonly List<ColorPick> ColorPicks = new();
    }

    record ColorPick(int ColorCount, string Color);
}
