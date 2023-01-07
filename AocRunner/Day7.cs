using System.Text.RegularExpressions;
using _testOutputHelper = System.Console;


public class Day7 : IDaySolver
{
    private readonly ITestOutputHelper _helper;
    private Regex cmd_changeDir = new(@"\$ cd (\w+|\/|\.{2})");
    private Regex fs_Directory = new(@"dir (\w+)");
    private Regex fs_File = new(@"(\d+) (.*)");

    public Day7(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    public string SolvePart1(string[] loadedInput)
    {
        var rootDir = BuildFileSystem(loadedInput);
        var sum = CalculateSumOfSmallDirectories(rootDir);
        return sum.ToString(); 
    }
    
    public string SolvePart2(string[] loadedInput)
    {
        var rootDir = BuildFileSystem(loadedInput);
        var sum = GetSmallestDirectoryToDelete(rootDir);
        return sum.ToString(); 
    }

    private int CalculateSumOfSmallDirectories(Dir root)
    {
        var sum = 0;
        foreach (Dir subDir in root.SubDirectories)
        {
            TotalSize(subDir, filter:r => r < 100000, onFilter: totalSizeRecursive => sum += totalSizeRecursive);
        }
        return sum;
    }
    
    private int GetSmallestDirectoryToDelete(Dir rootDir)
    {
        int limit = 30000000;
        var free = 70000000 - TotalSize(rootDir, filter:_ => true, onFilter: _ => { });
        var remainingToLimit = limit - free;

        var possibleFolders = new List<int>();
        foreach (Dir subDir in rootDir.SubDirectories)
        {
            TotalSize(subDir, filter:r => r > remainingToLimit, onFilter: totalSizeRecursive =>
            {
                possibleFolders.Add(totalSizeRecursive);
            });
        }

        return possibleFolders.Min();
    }

    private Dir BuildFileSystem(string[] terminalLines)
    {
        var rootDir = new Dir("/", null);
        var currentDirectory = rootDir;
        foreach (string terminalline in terminalLines)
        {
            var changeDirMatch = cmd_changeDir.Match(terminalline);
            if (changeDirMatch.Success)
            {
                string cdArgument = changeDirMatch.Groups[1].Value;
                currentDirectory = cdArgument switch
                {
                    "/" => rootDir,
                    ".." => currentDirectory.Parent ?? rootDir,
                    _ => currentDirectory.GetSubDir($"{cdArgument}/")
                };
            }

            var fsDirectoryMatch = fs_Directory.Match(terminalline);
            if (fsDirectoryMatch.Success)
            {
                var dirFound = fsDirectoryMatch.Groups[1].Value;
                currentDirectory.AddSubdirectory(dirFound);
            }

            var fsFileMatch = fs_File.Match(terminalline);
            if (fsFileMatch.Success)
            {
                int fileSize = int.Parse(fsFileMatch.Groups[1].Value);
                currentDirectory.TotalSize += fileSize;
            }
        }

        return rootDir;
    }

    private int TotalSize(Dir dir, Func<int,bool> filter, Action<int> onFilter)
    {
        var totalSizeRecursive = dir.TotalSize + dir.SubDirectories.Sum(subDir => TotalSize(subDir, filter, onFilter));

        if (filter(totalSizeRecursive))
        {
            onFilter(totalSizeRecursive);
        }
        
        return totalSizeRecursive;
    }
}

public class Dir
{
    private readonly List<Dir> _subDirectories;

    public Dir(string fullPath, Dir? parent)
    {
        FullPath = fullPath;
        Parent = parent;
        _subDirectories = new List<Dir>();
    }
    
    public string FullPath { get; }
    public Dir? Parent { get; }
    public int TotalSize { get; set; }
    public IEnumerable<Dir> SubDirectories => _subDirectories;

    public void AddSubdirectory(string dirFound)
    {
        _subDirectories.Add(new Dir($"{FullPath}{dirFound}/", this));
    }
    
    public Dir GetSubDir(string subPath)
    {
        return SubDirectories.First(c => c.FullPath == $"{FullPath}{subPath}");
    }
}
