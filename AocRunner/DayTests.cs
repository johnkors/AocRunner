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
    public void TestPart1()
    {

        var sln = Framework.Solve(Daysolver, s => s.SolvePart1, loadInput:() => TestData);
        Assert.NotNull(sln);
        Assert.Equal(ExpectedForTestInputPart1, sln);
    }

    [Fact]
    public void TestPart2()
    {
        var sln = Framework.Solve(Daysolver, s => s.SolvePart2, loadInput:() => TestData);
        Assert.NotNull(sln);
        Assert.Equal(ExpectedForTestInputPart2, sln);
    }
}
