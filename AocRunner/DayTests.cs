using Xunit;

namespace AocFramework;

public abstract class DayTests
{
    protected readonly ITestOutputHelper _helper;

    protected DayTests(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    protected IDaySolver Daysolver { get; set; }

    protected abstract string TestData { get; }
    protected abstract string ExpectedForTestInputPart1 { get; }
    protected virtual string ExpectedForTestInputPart2 => "Not implemented";
    protected virtual string ExpectedForInputPart1 => "Not implemented";
    protected virtual string ExpectedForInputPart2 => "Not implemented";

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
    
    [SkippableFact]
    public void TestPart1_WithInput()
    {
        Skip.If(ExpectedForInputPart1 == "Not implemented", "ExpectedForInputPart1 not set");
        
        var framework = Framework.Init(Daysolver, _helper.WriteLine);
        var sln = framework.Solve(Daysolver, s => s.SolvePart1);
        Assert.Equal(ExpectedForInputPart1, sln);
    }

    [SkippableFact]
    public void TestPart2_WithInput()
    {
        Skip.If(ExpectedForInputPart2 == "Not implemented", "ExpectedForInputPart2 not set");
        var framework = Framework.Init(Daysolver, _helper.WriteLine);
        var sln = framework.Solve(Daysolver, s => s.SolvePart2);
        Assert.Equal(ExpectedForInputPart2, sln);
    }
}
