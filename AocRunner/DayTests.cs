using Xunit;

public abstract class DayTests
{
    protected DayTests(IDaySolver daysolver)
    {
        Daysolver = daysolver;
    }

    private IDaySolver Daysolver { get; }

    protected abstract string TestData { get; }
    protected abstract string ExpectedForTestInputPart1 { get; }
    protected abstract string ExpectedForTestInputPart2 { get; }

    [Fact]
    public void TestPart1()
    {
        var sln = Program.Solve(Daysolver, s => s.SolvePart1, () => TestData);
        Assert.Equal(ExpectedForTestInputPart1, sln);
    }

    [Fact]
    public void TestPart2()
    {
        var sln = Program.Solve(Daysolver, s => s.SolvePart2, () => TestData);
        Assert.Equal(ExpectedForTestInputPart2, sln);
    }
}
