using Xunit;

namespace AocFramework;

public abstract class DayTests<T> where T: IDaySolver, new()
{
    protected DayTests(ITestOutputHelper helper)
    {
        Daysolver = new T();
        Framework.Logger = s =>
        {
            try
            {
                helper.WriteLine(s);
            }
            catch
            {
            }
        };
    }

    private IDaySolver Daysolver { get; }

    protected abstract string TestData { get; }
    protected abstract string ExpectedForTestInputPart1 { get; }
    protected virtual string ExpectedForTestInputPart2 => "??? No expectation set yet ???";

    [Fact]
    public void TestPart1_WithTestData()
    {
        var sln = Framework.Init(Daysolver).Solve(Daysolver, s => s.SolvePart1, loadInput:() => TestData);
        Assert.NotNull(sln);
        Assert.Equal(ExpectedForTestInputPart1, sln);
    }
    
    [Fact]
    public void TestPart1_WithInput()
    {
        var framework = Framework.Init(Daysolver);
        framework.Login();
        var sln = framework.Solve(Daysolver, s => s.SolvePart1);
        Assert.NotNull(sln);
    }

    [Fact]
    public void TestPart2_WithTestData()
    {
        var sln = Framework.Init(Daysolver).Solve(Daysolver, s => s.SolvePart2, loadInput:() => TestData);
        Assert.NotNull(sln);
        Assert.Equal(ExpectedForTestInputPart2, sln);
    }
    
    // [Fact]
    // public void TestPart2_WithInput()
    // {
    //     var slnRealInput = Framework.Solve(Daysolver, s => s.SolvePart2);
    //     Assert.NotNull(slnRealInput);
    // }
}
