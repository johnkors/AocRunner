namespace Y2022;

public class Day8 : IDaySolver
{
    private readonly ITestOutputHelper _helper;

    public Day8() : this(new FakeHelper()) {}

    private Day8(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    public string SolvePart1(string[] loadedInput)
    {
        var map = LoadMap(loadedInput);
        
        var visibleTrees = FindVisibleTrees(map);
        return visibleTrees.ToString();
    }
    
    public string SolvePart2(string[] loadedInput)
    {
        var map = LoadMap(loadedInput);
        
        var scenicScore = FindLargestScenicScore(map);
        return scenicScore.ToString();
    }

    int FindVisibleTrees(Map map)
    {
        var visibles = 0;
        foreach (var tree in map.Locations)
        {
            bool isEdge = tree.Location switch
            {
                { X: 1 } or { Y: 1 } => true,
                { } c when c.X == map.MapSize.X => true,
                { } c when c.Y == map.MapSize.Y => true,
                _ => false
            };

            if (isEdge)
            {
                // _helper.WriteLine($"Visible: {tree}");
                visibles++;
                continue;
            }

            var west = map.GetWestOf(tree);
            if (west.All(c => c.Height < tree.Height))
            {
                _helper.WriteLine($"Visible from west: {tree}");
                visibles++;
                continue;
            }
            
            var east = map.GetEastOf(tree);
            if (east.All(c => c.Height < tree.Height))
            {
                _helper.WriteLine($"Visible from east: {tree}");
                visibles++;
                continue;
            }
                
            var north = map.GetNorthOf(tree);
            if (north.All(c => c.Height < tree.Height))
            {
                _helper.WriteLine($"Visible from north: {tree}");
                visibles++;
                continue;
            }
                
            var south = map.GetSouthOf(tree);
            if (south.All(c => c.Height < tree.Height))
            {
                _helper.WriteLine($"Visible from south: {tree}");
                visibles++;
                continue;
            }
        }
        return visibles;
    }
    
    int FindLargestScenicScore(Map map)
    {
        var scenicScores = new List<ScenicScore>();
        foreach (var tree in map.Locations)
        {
            var west = map.GetWestOf(tree);
            var westScore = west.CountUntil(c => c.Height >= tree.Height);
            
            var east = map.GetEastOf(tree);
            var eastScore = east.CountUntil(c => c.Height >= tree.Height);
            
            var north = map.GetNorthOf(tree);
            var northScore = north.CountUntil(c => c.Height >= tree.Height);
            
            var south = map.GetSouthOf(tree);
            var southScore = south.CountUntil(c => c.Height >= tree.Height);

            var score = new Score(westScore, eastScore, northScore, southScore);
            if (tree.Location == new Coordinates(3, 2) || tree.Location == new Coordinates(3,4))
            {
                _helper.WriteLine(tree.ToString());
                _helper.WriteLine(score.ToString());
            }
            scenicScores.Add(new ScenicScore(score.Calculated, tree));
        }
        return scenicScores.Max(c => c.Score);
    }

    record ScenicScore(int Score, Tree Tree);

    Map LoadMap(string[] loadedInput)
    {
        var currentCoordinates = new Coordinates(X: 1, Y: 1);
        var dic = new List<Tree>();
        foreach (string treeRow in loadedInput)
        {
            foreach (char height in treeRow)
            {
                var tree = new Tree(height, new(currentCoordinates.X, currentCoordinates.Y));
                dic.Add(tree);
                
                if(currentCoordinates.X < treeRow.Length)
                    currentCoordinates = currentCoordinates.MoveX();
            }

            if (currentCoordinates.Y < loadedInput.Length)
            {
                currentCoordinates = currentCoordinates.MoveY();    
            }
        }

        return new Map(dic, new Coordinates(currentCoordinates.X, currentCoordinates.Y));
    }

    record Map(IEnumerable<Tree> Locations, Coordinates MapSize)
    {
        public IEnumerable<Tree> GetWestOf(Tree tree)
        {
            return Locations.Where(x => x.Location.X < tree.Location.X && x.Location.Y == tree.Location.Y).OrderByDescending(x => x.Location.X);
        }
        
        public IEnumerable<Tree> GetEastOf(Tree tree)
        {
            return Locations.Where(x => x.Location.X > tree.Location.X  && x.Location.Y == tree.Location.Y).OrderBy(x => x.Location.X);
        }
        
        public IEnumerable<Tree> GetNorthOf(Tree tree)
        {
            return Locations.Where(x => x.Location.Y < tree.Location.Y && x.Location.X == tree.Location.X).OrderByDescending(x => x.Location.Y);
        }
        
        public IEnumerable<Tree> GetSouthOf(Tree tree)
        {
            return Locations.Where(x =>  x.Location.Y > tree.Location.Y && x.Location.X == tree.Location.X).OrderBy(x => x.Location.Y);
        }
    }

    record Coordinates(int X, int Y)
    {
        public Coordinates MoveX()
        {
            return this with { X = X + 1 };
        }
    
        public Coordinates MoveY()
        {
            return this with { X = 1, Y = Y + 1 };
        }
    }

    record Tree(char Height, Coordinates Location);
}

internal record Score(int WestScore, int EastScore, int NorthScore, int SouthScore)
{
    public int Calculated => WestScore * EastScore * NorthScore * SouthScore;
};
