using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

using ReverseMarkdown;

public class Framework
{
    private static readonly HttpClient HttpClient = new();
    private static bool _loggedIn;
    private static string? _session;
    private static IDaySolver? _solver;
    private static int? _solverDay;
    private const string EnvVariablename = "AOC_SESSION";

    private Framework()
    {
        
    }

    public static Framework Init(IDaySolver solver)
    {
        _solver = solver;
        return new Framework();
    }

    public static Action<string?> Logger { get; set; } = Console.WriteLine;

    public void Solve1(IDaySolver solver, bool askForSubmit = false)
    {
        Info($"Solving day {GetSolverDay()} - part 1");
        if (!_loggedIn)
        {
            Login();
        }

        var unfinishedPart = GetStatus();
        if (unfinishedPart == null || unfinishedPart == 2)
        {
            Warn("Part 1 already finished!");
            return;
        }
        var sln = Solve(solver, s => s.SolvePart1);
        if (sln != null && askForSubmit)
        {
            var submit = ShouldSubmit(sln);
            if (submit)
            {
                var res = PostAnswer(GetSolverDay(), 1, sln);
                External(res);
            }
        }
        else
        {
            Environment.Exit(-1);
        }
    }

    public void Solve2(IDaySolver solver, bool askForSubmit = false)
    {
        Info($"Solving day {GetSolverDay()} - part 2");
        if (!_loggedIn)
        {
            Login();
        }

        var unfinishedPart = GetStatus();

        if (unfinishedPart == null || unfinishedPart == 1)
        {
            Warn("Part 1 needs to be completed first");
            return;
        }


        var sln = Solve(solver, s => s.SolvePart2);
        if (sln != null && askForSubmit)
        {
            var submit = ShouldSubmit(sln);
            if (submit)
            {
                var res = PostAnswer(GetSolverDay(), 2, sln);
                External(res);
            }
        }
        else
        {
            Environment.Exit(-1);
        }
    }

    private int? GetStatus()
    {
        if (!_loggedIn)
        {
            Login();
        }

        var html = GetTaskForDay(GetSolverDay()).GetAwaiter().GetResult();
        var regex = new Regex(@"<main>(.*?)</main>", RegexOptions.Compiled);
        var match = regex.Match(html);
        if (match.Success)
        {
            html = match.Groups[1].Value;
        }
        var regexInput = new Regex("""<input type="hidden" name="level" value="(.*?)\"/>""", RegexOptions.Compiled);
        var inputMatches = regexInput.Match(html);

        int? unfinishedPart = null;
        if (inputMatches.Success)
        {
            string value = inputMatches.Groups[1].Value;
            unfinishedPart = int.Parse(value);
        }

        return unfinishedPart;
    }

    static bool ShouldSubmit(string sln)
    {
        Ask($"Submit '{sln}'? y/n: ");

        char answer = Console.ReadKey().KeyChar;
        Console.WriteLine();
        if (answer == 'y')
        {
            return true;
        }
        Warn("Not submitting");
        return false;
    }

    public void Login()
    {
        var environmentVariable = Environment.GetEnvironmentVariable(EnvVariablename);
        if (environmentVariable is not { })
        {
            Error("⛔️ Please set the AOC_SESSION env variable");
            Environment.Exit(-1);
        }

        _session = environmentVariable;
        HttpClient.DefaultRequestHeaders.Add("cookie", new []{ $"session={_session}" });
        var res = HttpClient.GetAsync($"https://adventofcode.com/2022/day/{GetSolverDay()}/input").GetAwaiter().GetResult();
        if (!res.IsSuccessStatusCode)
        {
            Error($"⛔️ Failed to login. StatusCode from adventofcode.com: {res.StatusCode}. Check/refresh your {EnvVariablename} env variable. Exiting..");
            Environment.Exit(-1);
        }

        _loggedIn = true;
    }

    public delegate string SolverMethod(string[] rows);

    public delegate SolverMethod SolvePartMethod(IDaySolver solver);

    public delegate string? LoadInputDelegate();

    public string? Solve(IDaySolver solver, Expression<SolvePartMethod> daySolverAction, LoadInputDelegate? loadInput = null)
    {
        string? loadedInput = loadInput != null ? loadInput.Invoke() : GetInputForDay(GetSolverDay()).GetAwaiter().GetResult();
        if (string.IsNullOrEmpty(loadedInput))
        {
            Error("Failed to load input.");
            return null;
        }


        var rows = loadedInput.Split(Environment.NewLine).ToArray();
        if (rows.Last() == "")
            rows = rows.SkipLast(1).ToArray();

        var methodName = GetMethodName(daySolverAction);
        try
        {
            var solution = daySolverAction.Compile().Invoke(solver).Invoke(rows);
            Info($"Solution: '{solution}'");

            return solution;

        }
        catch (NotImplementedException)
        {
            var type = solver.GetType();
            Warn($"❌️ Could not run `{methodName}` for day {GetSolverDay()} using '{type.Namespace}.{type.Name}'. You need to implement it!");
        }

        return null;
    }

    static void Info(string text)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Logger(text);
        Console.ResetColor();
    }

    static void Ask(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Logger(text);
        Console.ResetColor();
    }

    static void External(string html)
    {
        Config config = new Config
        {
            UnknownTags = Config.UnknownTagsOption.Bypass,
            GithubFlavored = true,
            SmartHrefHandling = true,
            RemoveComments = true,
        };
        var converter = new Converter(config);
        var mkd = converter.Convert(html);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(mkd);
        Console.ResetColor();
    }

    static void Warn(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Logger(text);
        Console.ResetColor();
    }

    static void Error(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Logger(text);
        Console.ResetColor();
    }


    public int GetSolverDay()
    {
        if (_solverDay != null)
            return _solverDay.Value;

        var regex = new Regex(@"Day(\d)", RegexOptions.Singleline);
        var match = regex.Match(_solver!.GetType().Name);
        if (match.Success)
        {
            _solverDay = int.Parse(match.Groups[1].Value);
            return _solverDay.Value;
        }

        throw new NotSupportedException("Your implementation type must follow the Day<X> naming convention");
    }

    static string GetMethodName(LambdaExpression expression)
    {
        var unaryExpression = (UnaryExpression)expression.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
        var methodCallObject = (ConstantExpression)methodCallExpression.Object!;
        var methodInfo = (MethodInfo)methodCallObject.Value!;
        return methodInfo.Name;
    }

    static async Task<string?> GetInputForDay(int day)
    {
        string requestUri = $"https://adventofcode.com/2022/day/{day}/input";
        var res = await HttpClient.GetAsync(requestUri);
        var body = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            Info($":/ Could not input day {day} ({res.StatusCode})");

            Info($"-----");
            Info($"{body}");
            Info($"-----");
            
            Error($"⛔️ Failed to fetch input from {requestUri}.\n" +
                  $"StatusCode from adventofcode.com: {res.StatusCode}.\n" +
                  $"Check/refresh your '{EnvVariablename}:{Environment.GetEnvironmentVariable(EnvVariablename)}' env variable. " +
                  $"Exiting..");
            return null;
        }
        return body;
    }

    static async Task<string> GetTaskForDay(int day)
    {
        var res = await HttpClient.GetAsync($"https://adventofcode.com/2022/day/{day}");
        var body = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            Info($":/ Could not load task for day {day} ({res.StatusCode})");

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
    string SolvePart1(string[] loadedInput);

    string SolvePart2(string[] loadedInput) => "Not implemented";
}
