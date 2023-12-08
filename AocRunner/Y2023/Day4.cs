using System.Globalization;
using System.Text.RegularExpressions;

namespace Y2023;

public class Day4 : IDaySolver
{
    public string SolvePart1(string[] loadedInput) =>
        loadedInput
            .GetByPattern(@"Card\s+(\d+): (.*) \| (.*)", ToCard)
            .Sum(c => (int) Math.Pow(2, c.MatchCount - 1))
            .ToString(CultureInfo.InvariantCulture);

    public string SolvePart2(string[] loadedInput)
    {
        var cards = loadedInput
            .GetByPattern(@"Card\s+(\d+): (.*) \| (.*)", ToCard)
            .ToArray();

        // init counter array, [1, 1, 1 … 1]
        var counterPrCard = cards.Select(_ => 1).ToArray();

        for (var cardIndex = 0; cardIndex < cards.Length; cardIndex++)
        {
            var card = cards[cardIndex];
            int numberOfCopiesToIncrement = counterPrCard[cardIndex];

            // increment next cards with number of copies of currentCard
            for (var nextCardIndex = 1; nextCardIndex <= card.MatchCount; nextCardIndex++)
            {
                counterPrCard[cardIndex + nextCardIndex] += numberOfCopiesToIncrement;
            }
        }

        return $"{counterPrCard.Sum()}";
    }

    Card ToCard(Match m) => new(GetInts(m.Groups[2].Value).Intersect(GetInts(m.Groups[3].Value)).Count());

    static IEnumerable<int> GetInts(string str) => Regex.Matches(str, @"(\d+)").Select(m => m.AsInt(1));

    record Card(int MatchCount);
}
