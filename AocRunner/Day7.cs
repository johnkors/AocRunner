using System.Text.RegularExpressions;
using _testOutputHelper = System.Console;


public class Day7 : IDaySolver
{
    private Regex cmd_changeDir = new(@"\$ cd (\w+|\/|\.{2})");
    private Regex cmd_listDir = new(@"\$ ls");
    private Regex fs_Directory = new(@"dir (\w+)");
    private Regex fs_File = new(@"(\d+) (.*)");

    public string SolvePart1(string[] loadedInput)
    {
        var rootDir = BuildFileSystem(loadedInput);
        
        var sum = Calculate(rootDir);
        return sum.ToString();
    }

    private Dir BuildFileSystem(string[] terminalLines)
    {
        var rootDir = new Dir("/", null);
        var currentDirectory = rootDir;
        var currentPath = rootDir.FullPath;
        foreach (string terminalline in terminalLines)
        {
            Match changeDirMatch = cmd_changeDir.Match(terminalline);
            if (changeDirMatch.Success)
            {
                string newPath = changeDirMatch.Groups[1].Value;
                if (newPath == "/")
                {
                    currentPath = newPath;
                    currentDirectory = rootDir;
                    continue;
                }

                if (newPath != ".." && newPath != "/")
                {
                    currentPath += $"{newPath}/";
                    currentDirectory = currentDirectory.SubDirectories.First(c => c.FullPath == currentPath);
                    continue;
                }

                if (newPath == "..")
                {
                    string parentPath = currentPath.Split("/")[..^2].Aggregate((x, y) => x + "/" + y) + "/";
                    currentPath = parentPath;
                    currentDirectory = currentDirectory.Parent ?? rootDir;
                    continue;
                }
            }

            Match cmdLsMatch = cmd_listDir.Match(terminalline);
            if (cmdLsMatch.Success)
            {
                continue;
            }

            Match fsDirectoryMatch = fs_Directory.Match(terminalline);
            if (fsDirectoryMatch.Success)
            {
                var dirFound = fsDirectoryMatch.Groups[1].Value;
                var fullPath = currentPath == "/" ? $"/{dirFound}/" : $"{currentPath}{dirFound}/";
                var newDir = new Dir(fullPath, currentDirectory);
                currentDirectory.AddSubdirectory(newDir);
                continue;
            }

            Match fsFileMatch = fs_File.Match(terminalline);
            if (fsFileMatch.Success)
            {
                int fileSize = int.Parse(fsFileMatch.Groups[1].Value);
                currentDirectory.FileSize += fileSize;
            }
        }

        return rootDir;
    }

    private int Calculate(Dir dir)
    {
        var sumOfTotalSizes = 0;
        foreach (Dir subDir in dir.SubDirectories)
        {
            TotalSize(subDir, OnSmallTotalSize: val => sumOfTotalSizes += val);
        }
        return sumOfTotalSizes;
    }
    
    private int TotalSize(Dir dir, Action<int> OnSmallTotalSize)
    {
        var totalSize = dir.FileSize;
        foreach (Dir subDir in dir.SubDirectories)
        {
            totalSize += TotalSize(subDir, OnSmallTotalSize);
        }
        
        if (totalSize < 100000)
        {
            _testOutputHelper.WriteLine($"{dir.FullPath}: {totalSize}");
            OnSmallTotalSize(totalSize);
        }
        
        return totalSize;
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

    public string FullPath { get; set; }

    public Dir? Parent { get; }
    

    public void AddSubdirectory(Dir dir)
    {
        if (dir.FullPath == FullPath)
        {
            throw new Exception("La til et subdir med samme fullpath! som parent :/");
        }

        _subDirectories.Add(dir);
    }
    public int FileSize { get; set; }
    public IEnumerable<Dir> SubDirectories => _subDirectories;

    public override string ToString()
    {
        return $"FullPath: '{FullPath}' - ParentPath:'{Parent?.FullPath}' - SubDirectories: {_subDirectories.Count()} - Size: {FileSize}";
    }
}
