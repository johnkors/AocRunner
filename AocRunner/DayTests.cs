using Xunit;

namespace AocFramework;

public abstract class DayTests<T> where T: IDaySolver, new()
{
    protected DayTests()
    {
        Daysolver = new T();
    }

    private IDaySolver Daysolver { get; }

    protected abstract string TestData { get; }
    protected abstract string ExpectedForTestInputPart1 { get; }
    protected abstract string ExpectedForTestInputPart2 { get; }

    [Fact]
    public void TestPart1()
    {
        var sln = Framework.Solve(Daysolver, s => s.SolvePart1, () => TestData);
        Assert.Equal(ExpectedForTestInputPart1, sln);
    }

    [Fact]
    public void TestPart2()
    {
        var sln = Framework.Solve(Daysolver, s => s.SolvePart2, () => TestData);
        Assert.Equal(ExpectedForTestInputPart2, sln);
    }
}
