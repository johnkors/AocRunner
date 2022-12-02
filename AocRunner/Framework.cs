global using static System.Console;
using System.Linq.Expressions;
using System.Reflection;

public static partial class Program
{
    private static readonly HttpClient HttpClient = new();
    private static bool _loggedIn;
    private static string? _session;
    private const string EnvVariablename = "AOC_SESSION";

    public static void Solve1(IDaySolver solver, bool useTestInput = false)
    {
        WriteLine($"Solving part 1 for day {solver.Day}..");
        Solve(solver, s => s.SolvePart1, useTestInput);
    }

    public static void Solve2(IDaySolver solver, bool useTestInput = false)
    {
        WriteLine($"Solving part 2 for day {solver.Day}..");
        Solve(solver, s => s.SolvePart2, useTestInput);
    }

    static void Login(int day)
    {
        var environmentVariable = Environment.GetEnvironmentVariable(EnvVariablename);
        if (environmentVariable is not { })
        {
            Error.WriteLine("⛔️ Please set the AOC_SESSION env variable");
            Environment.Exit(-1);
        }

        _session = environmentVariable;
        HttpClient.DefaultRequestHeaders.Add("cookie", new []{ $"session={_session}" });
        var res = HttpClient.GetAsync($"https://adventofcode.com/2022/day/{day}/input").GetAwaiter().GetResult();
        if (!res.IsSuccessStatusCode)
        {
            Error.WriteLine($"⛔️ Failed to login. StatusCode from adventofcode.com: {res.StatusCode}. Check/refresh your {EnvVariablename} env variable. Exiting..");
            Environment.Exit(-1);
        }

        _loggedIn = true;
    }

    static void Solve(IDaySolver solver, Expression<Func<IDaySolver, Func<string[],string>>> daySolverAction, bool useTestInput = false)
    {
        if (!_loggedIn)
        {
            Login(solver.Day);
        }

        var loadedInput = LoadInput(solver, useTestInput);
        var rows = loadedInput.Split("\n");
        var methodName = GetMethodName(daySolverAction);
        try
        {
            var solution = daySolverAction.Compile().Invoke(solver).Invoke(rows);
            WriteLine($"✅ Solution:\n{solution}");
        }
        catch (NotImplementedException)
        {
            var type = solver.GetType();
            WriteLine($"❌️ Could not run `{methodName}` for day {solver.Day} using '{type.Namespace}.{type.Name}'. You need to implement it!");
        }
    }

    static string GetMethodName(LambdaExpression expression)
    {
        var unaryExpression = (UnaryExpression)expression.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
        var methodCallObject = (ConstantExpression)methodCallExpression.Object;
        var methodInfo = (MethodInfo)methodCallObject.Value;
        return methodInfo.Name;
    }

    private static string LoadInput(IDaySolver solver, bool useTestInput)
    {
        string loadedInput;
        if (useTestInput)
        {
            if (solver.TestData == null)
            {
                Error.WriteLine($"⛔️ FAILURE: Want to use testdata, but no testdata found for day {solver.Day}");;
                Environment.Exit(-3);
            }
            WriteLine("ℹ️ Using testdata as input!");
            loadedInput = solver.TestData;
        }
        else
        {
            loadedInput = LoadDayInput(solver.Day).GetAwaiter().GetResult();
        }

        return loadedInput;
    }

    static async Task<string> LoadDayInput(int day)
    {
        var res = await HttpClient.GetAsync($"https://adventofcode.com/2022/day/{day}/input");
        var body = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            WriteLine($":/ Could not load day {day} ({res.StatusCode})");
            WriteLine($"-----");
            WriteLine($"{body}");
            WriteLine($"-----");
        }
        return body;
    }
}

public interface IDaySolver
{
    int Day { get; }
    string SolvePart1(string[] loadedInput);

    string SolvePart2(string[] loadedInput)
    {
        throw new NotImplementedException();
    }

    string? TestData => null;
}
