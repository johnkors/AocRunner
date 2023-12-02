namespace Y2022;

public class Day7Tests(ITestOutputHelper testOutputHelper) : DayTests(testOutputHelper, new Day7(testOutputHelper))
{
    protected override string TestData =>
        """
        $ cd /
        $ ls
        dir a
        14848514 b.txt
        8504156 c.dat
        dir d
        $ cd a
        $ ls
        dir e
        29116 f
        2557 g
        62596 h.lst
        $ cd e
        $ ls
        584 i
        $ cd ..
        $ cd ..
        $ cd d
        $ ls
        4060174 j
        8033020 d.log
        5626152 d.ext
        7214296 k
        """;

    protected override string ExpectedForTestInputPart1 => "95437";
    
    protected override string ExpectedForInputPart1 => "1582412";
    
    protected override string ExpectedForInputPart2 => "3696336";

    protected override string ExpectedForTestInputPart2 => "24933642";
}
