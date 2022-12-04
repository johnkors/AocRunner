using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

public static partial class Program
{
    private static readonly HttpClient HttpClient = new();
    private static bool _loggedIn;
    private static string? _session;
    private const string EnvVariablename = "AOC_SESSION";

    public static void Solve1(IDaySolver solver, bool askForSubmit = false)
    {
        Info($"Solving day {solver.Day} - part 1");
        var sln = Solve(solver, s => s.SolvePart1, () => LoadInput(solver));
        if (sln != null && askForSubmit)
        {
            var submit = ShouldSubmit(sln);
            if (submit)
            {
                var res = PostAnswer(solver.Day, 1, sln);
                External(res);
            }
        }
        Info("");
    }

    public static void Solve2(IDaySolver solver, bool askForSubmit = false)
    {
        Info($"Solving day {solver.Day} - part 2");
        var sln = Solve(solver, s => s.SolvePart2, () => LoadInput(solver));
        if (sln != null && askForSubmit)
        {
            var submit = ShouldSubmit(sln);
            if (submit)
            {
                var res = PostAnswer(solver.Day, 2, sln);
                External(res);
            }
        }
    }

    static bool ShouldSubmit(string sln)
    {
        Info($"Submit '{sln}'? y/n: ");
        char answer = Console.ReadKey().KeyChar;
        Console.WriteLine();
        if (answer == 'y')
        {
            return true;
        }
        Warn("Not submitting");
        return false;
    }

    private static void Login(int day)
    {
        var environmentVariable = Environment.GetEnvironmentVariable(EnvVariablename);
        if (environmentVariable is not { })
        {
            Error("⛔️ Please set the AOC_SESSION env variable");
            Environment.Exit(-1);
        }

        _session = environmentVariable;
        HttpClient.DefaultRequestHeaders.Add("cookie", new []{ $"session={_session}" });
        var res = HttpClient.GetAsync($"https://adventofcode.com/2022/day/{day}/input").GetAwaiter().GetResult();
        if (!res.IsSuccessStatusCode)
        {
            Error($"⛔️ Failed to login. StatusCode from adventofcode.com: {res.StatusCode}. Check/refresh your {EnvVariablename} env variable. Exiting..");
            Environment.Exit(-1);
        }

        _loggedIn = true;
    }

    public delegate string SolverMethod(string[] rows);

    public delegate SolverMethod SolvePartMethod(IDaySolver solver);

    public delegate string LoadInputDelegate();

    public static string? Solve(IDaySolver solver, Expression<SolvePartMethod> daySolverAction, LoadInputDelegate loadInput)
    {
        if (!_loggedIn)
        {
            Login(solver.Day);
        }

        var loadedInput = loadInput();
        var rows = loadedInput.Split(Environment.NewLine).Select(s => s.TrimEnd()).ToArray();
        if (rows.Last() == "")
            rows = rows.SkipLast(1).ToArray();

        var methodName = GetMethodName(daySolverAction);
        try
        {
            var solution = daySolverAction.Compile().Invoke(solver).Invoke(rows);
            Info($"Solution: {solution}");

            return solution;

        }
        catch (NotImplementedException)
        {
            var type = solver.GetType();
            Warn($"❌️ Could not run `{methodName}` for day {solver.Day} using '{type.Namespace}.{type.Name}'. You need to implement it!");
        }

        return null;
    }

    static string GetMethodName(LambdaExpression expression)
    {
        var unaryExpression = (UnaryExpression)expression.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
        var methodCallObject = (ConstantExpression)methodCallExpression.Object!;
        var methodInfo = (MethodInfo)methodCallObject.Value!;
        return methodInfo.Name;
    }

    private static string LoadInput(IDaySolver solver)
    {
        return GetInputForDay(solver.Day).GetAwaiter().GetResult();;
    }

    static void Info(string text)
    {
        Console.WriteLine(text);
    }

    static void External(string text)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(text);
        Console.ForegroundColor = prev;
    }

    static void Warn(string text)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ForegroundColor = prev;
    }

    static void Error(string text)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = prev;
    }

    static async Task<string> GetInputForDay(int day)
    {
        var res = await HttpClient.GetAsync($"https://adventofcode.com/2022/day/{day}/input");
        var body = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            Info($":/ Could not load day {day} ({res.StatusCode})");
            Info($"-----");
            Info($"{body}");
            Info($"-----");
        }
        return body;
    }

    static string PostAnswer(int day, int part, string answer)
    {
        var nameValueCollection = new Dictionary<string,string>
        {
            { "level", part.ToString() },
            { "answer", answer }
        };
        var formUrlEncodedContent = new FormUrlEncodedContent(nameValueCollection);

        var res = HttpClient.PostAsync($"https://adventofcode.com/2022/day/{day}/answer", formUrlEncodedContent).GetAwaiter().GetResult();
        var body = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        if (!res.IsSuccessStatusCode)
        {
            Error($":/ Could not post answer {day} part {part} ({res.StatusCode})");
            Error($"-----");
            Error($"{body}");
            Error($"-----");
            return body;
        }
        var regex = new Regex(@"<main>(.*?)</main>", RegexOptions.Singleline);
        var match = regex.Match(body);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        return body;
    }
}

public interface IDaySolver
{
    int Day { get; }

    string SolvePart1(string[] loadedInput);

    string SolvePart2(string[] loadedInput);
}
