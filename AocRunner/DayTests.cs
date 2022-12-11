using Xunit;

namespace AocFramework;

public abstract class DayTests<T> where T: IDaySolver, new()
{
    protected readonly ITestOutputHelper _helper;

    protected DayTests(ITestOutputHelper helper)
    {
        _helper = helper;
        Daysolver = new T();
    }

    private IDaySolver Daysolver { get; }

    protected abstract string TestData { get; }
    protected abstract string ExpectedForTestInputPart1 { get; }
    protected virtual string ExpectedForTestInputPart2 => "Not implemented";

    [Fact]
    public void TestPart1_WithTestData()
    {
        var sln = Framework.Init(Daysolver, _helper.WriteLine).Solve(Daysolver, s => s.SolvePart1, loadInput:() => TestData);
        Assert.NotNull(sln);
        Assert.Equal(ExpectedForTestInputPart1, sln);
    }

    [SkippableFact]
    public void TestPart2_WithTestData()
    {
        Skip.If(ExpectedForTestInputPart2 == "Not implemented", "ExpectedForTestInputPart2 not set");
        var sln = Framework.Init(Daysolver, _helper.WriteLine).Solve(Daysolver, s => s.SolvePart2, loadInput:() => TestData);
        Assert.NotNull(sln);
        Assert.Equal(ExpectedForTestInputPart2, sln);
    }
}
