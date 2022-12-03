#pragma warning disable CS8509
#pragma warning disable CS8524
public class Day2 : IDaySolver
{
    public int Day => 2;

    public string TestData =>
    """
    A Y
    B X
    C Z
    """;

    public string ExpectedForTestInputPart1 => "15";
    public string ExpectedForTestInputPart2 => "12";


    public string SolvePart1(string[] loadedInput)
    {
        int totalScore = 0;

        foreach (string row in loadedInput)
        {
            (Hand Opponent, Hand Response) round = (Opponent: AsHand(row[0]), Response: AsHand(row[2]));
            int outcome = Play(round);
            int roundScore = (int)round.Response + outcome;
            totalScore += roundScore;
        }

        return totalScore.ToString();
    }

    public string SolvePart2(string[] loadedInput)
    {
        int totalScore = 0;

        foreach (string row in loadedInput)
        {
            (Hand Opponent, Hand Response) round = (Opponent: AsHand(row[0]), Response: CalculateHand(row[0], row[2]));

            int outcome = Play(round);
            int roundScore = (int)round.Response + outcome;
            totalScore += roundScore;
        }

        return totalScore.ToString();
    }

    static int Play((Hand Opponent, Hand Response) round)
    {
        return (int)(round switch
        {
            { Opponent: var o, Response: var r } when o == r => Outcome.Draw,
            (Hand.Paper, Hand.Scissors) or (Hand.Rock, Hand.Paper) or (Hand.Scissors, Hand.Rock) => Outcome.Win,
            _ => Outcome.Loss
        });
    }

    Hand CalculateHand(char opponent, char wantedOutcomeChar)
    {
        return AsOutcome(wantedOutcomeChar) switch
        {
            Outcome.Win => AsHand(opponent) switch
            {
                Hand.Rock => Hand.Paper,
                Hand.Paper => Hand.Scissors,
                Hand.Scissors => Hand.Rock
            },

            Outcome.Draw => AsHand(opponent) switch
            {
                _ => AsHand(opponent)
            },

            Outcome.Loss => AsHand(opponent) switch
            {
                Hand.Rock => Hand.Scissors,
                Hand.Paper => Hand.Rock,
                Hand.Scissors => Hand.Paper
            }
        };
    }


    static Hand AsHand(char hand)
    {
        return hand switch
        {
            'A' or 'X' => Hand.Rock,
            'B' or 'Y' => Hand.Paper,
            'C' or 'Z' => Hand.Scissors,
        };
    }

    static Outcome AsOutcome(char outcome)
    {
        return outcome switch
        {
            'X' => Outcome.Loss,
            'Y' => Outcome.Draw,
            'Z' => Outcome.Win
        };
    }

    enum Hand
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    enum Outcome
    {
        Loss = 0,
        Draw = 3,
        Win = 6
    }
}