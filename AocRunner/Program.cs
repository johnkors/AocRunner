TestOutputHelper testOutputHelper = new TestOutputHelper();
var day = new Day8(testOutputHelper);
Test.Verify(new Day8Tests(testOutputHelper));
var fw = Framework.Init(day, Console.WriteLine);
if (fw.HasUnsolvedParts())
{
    fw.PrintUnsolvedPartTask();
}

fw.Solve1(day);
fw.Solve2(day);

public class Day8 : IDaySolver
{
    public Day8(ITestOutputHelper testOutputHelper)
    {
        throw new NotImplementedException();
    }

    public string SolvePart1(string[] loadedInput)
    {
        var trees = LoadTrees(loadedInput);
        return "3";
    }

    private Map LoadTrees(string[] loadedInput)
    {
        var currentCoordinates = new Coordinates(X: 0, Y: 0);
        var dic = new Dictionary<Coordinates, Tree>();
        foreach (string row in loadedInput)
        {
            foreach (char height in row)
            {
                var tree = new Tree(height, new Coordinates(currentCoordinates.X, currentCoordinates.Y));
                dic.Add(tree.Location, tree);
                currentCoordinates = currentCoordinates with { X = currentCoordinates.X + 1 };
            }
            currentCoordinates = currentCoordinates with { Y = currentCoordinates.Y + 1 };
        }

        return new Map(dic, new Coordinates(currentCoordinates.X, currentCoordinates.Y));
    }
}

internal record Map(IDictionary<Coordinates, Tree> Locations, Coordinates MapSize);
internal record Coordinates(int X, int Y);

internal record Tree(char Height, Coordinates Location)
{
    void AddX(Tree tree)
    {
        
    }
    
    void AddY(Tree tree)
    {
        
    }
}

public class Day8Tests : DayTests
{
    public Day8Tests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        throw new NotImplementedException();
    }

    protected override IDaySolver Daysolver => new Day8(_helper);

    protected override string TestData =>
    """
    30373
    25512
    65332
    33549
    35390
    """;

    protected override string ExpectedForTestInputPart1 => "21";
}
