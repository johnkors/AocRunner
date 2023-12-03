using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

using ReverseMarkdown;

public class Framework
{
    private readonly HttpClient _httpClient = new();
    private bool _loggedIn;
    private string? _session;
    private readonly IDaySolver _solver;
    private SolverDate? _solverDate;
    private static Action<string?> Logger { get; set; } = Console.WriteLine;
    private const string EnvVariablename = "AOC_SESSION";
    record SolverDate(int Year, int Day);

    private Framework(IDaySolver solver)
    {
        _solver = solver;
    }

    public static Framework Init(IDaySolver solver, Action<string?> logger)
    {
        var f = new Framework(solver);
        f.Login();
        f.SetLogger(logger);
        return f;
    }

    private void SetLogger(Action<string?> logger)
    {
        Logger = logger;
    }

    public void Solve1(IDaySolver solver)
    {
        Info($"Solving day {GetSolverDate()} - part 1");

        var unfinishedPart = GetUnsolvedPart();
        if (unfinishedPart == null || unfinishedPart == 2)
        {
            Warn("Part 1 already complete");
            return;
        }
        var sln = Solve(solver, s => s.SolvePart1);
        if (sln != null)
        {
            var submit = ShouldSubmit(sln);
            if (submit)
            {
                var res = PostAnswer(GetSolverDate(), 1, sln);
                External(res);
            }
        }
    }

    public void Solve2(IDaySolver solver)
    {
        Info($"Solving day {GetSolverDate().Day} - part 2");
        if (!_loggedIn)
        {
            Login();
        }

        var unfinishedPart = GetUnsolvedPart();
        
        if (!unfinishedPart.HasValue)
        {
            Warn("Part 2 already complete");
            return;
        }

        if (unfinishedPart == 1)
        {
            Warn("Part 1 needs to be completed first");
            return;
        }

        var sln = Solve(solver, s => s.SolvePart2);
        if (sln != null && sln != "Not implemented")
        {
            var submit = ShouldSubmit(sln);
            if (submit)
            {
                var res = PostAnswer(GetSolverDate(), 2, sln);
                External(res);
            }
        }
    }

    private int? GetUnsolvedPart()
    {
        var html = GetHtmlForDay(GetSolverDate()).GetAwaiter().GetResult();
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
        _httpClient.DefaultRequestHeaders.Add("cookie", new []{ $"session={_session}" });
        var date = GetSolverDate();
        var res = _httpClient.GetAsync($"https://adventofcode.com/{date.Year}/day/{date.Day}/input").GetAwaiter().GetResult();
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
        string? loadedInput = loadInput != null ? loadInput.Invoke() : GetInputForDay(GetSolverDate()).GetAwaiter().GetResult();
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
            Warn($"❌️ Could not run `{methodName}` for day {GetSolverDate()} using '{type.Namespace}.{type.Name}'. You need to implement it!");
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

    private SolverDate GetSolverDate()
    {
        if (_solverDate is not null)
            return _solverDate;

        Type solverType = _solver.GetType();
        var dayMatch = new Regex(@"Day(\d)", RegexOptions.Singleline).Match(solverType.Name);
        var yearMatch = new Regex(@"Y(\d{4})", RegexOptions.Singleline).Match(solverType.Namespace!);

        if (dayMatch.Success && yearMatch.Success)
        {
            var solverDay = int.Parse(dayMatch.Groups[1].Value);
            var solverYear = int.Parse(yearMatch.Groups[1].Value);
            _solverDate = new SolverDate(solverYear, solverDay);
            return _solverDate;
        }

        throw new NotSupportedException("Your implementation type must follow the 'Day<\\d{1,2}>' naming convention in a " +
                                        " 'Y<\\d{4}>' namespace");
    }

    static string GetMethodName(LambdaExpression expression)
    {
        var unaryExpression = (UnaryExpression)expression.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
        var methodCallObject = (ConstantExpression)methodCallExpression.Object!;
        var methodInfo = (MethodInfo)methodCallObject.Value!;
        return methodInfo.Name;
    }

    async Task<string?> GetInputForDay(SolverDate date)
    {
        string requestUri = $"https://adventofcode.com/{date.Year}/day/{date.Day}/input";
        var res = await _httpClient.GetAsync(requestUri);
        var body = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            Info($":/ Could not input day {date} ({res.StatusCode})");

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

    async Task<string> GetHtmlForDay(SolverDate date)
    {
        var res = await _httpClient.GetAsync($"https://adventofcode.com/{date.Year}/day/{date.Day}");
        var body = await res.Content.ReadAsStringAsync();
        if (!res.IsSuccessStatusCode)
        {
            Info($":/ Could not load task for day {date} ({res.StatusCode})");

            Info($"-----");
            Info($"{body}");
            Info($"-----");
        }
        return body;
    }

    string PostAnswer(SolverDate date, int part, string answer)
    {
        var nameValueCollection = new Dictionary<string,string>
        {
            { "level", part.ToString() },
            { "answer", answer }
        };
        var formUrlEncodedContent = new FormUrlEncodedContent(nameValueCollection);

        var res = _httpClient.PostAsync($"https://adventofcode.com/{date.Year}/day/{date.Day}/answer", formUrlEncodedContent).GetAwaiter().GetResult();
        var body = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        if (!res.IsSuccessStatusCode)
        {
            Error($":/ Could not post answer {date.Day} part {part} ({res.StatusCode})");
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

    public bool HasUnsolvedParts()
    {
        return GetUnsolvedPart() != null;
    }

    public void PrintUnsolvedPartTask()
    {
        int unsolved = GetUnsolvedPart()!.Value;
        var html = GetHtmlForDay(GetSolverDate()).GetAwaiter().GetResult();
        var regex = new Regex(@"<main>(.*?)</main>", RegexOptions.Singleline);
        var match = regex.Match(html);
        if (match.Success)
        {
            var articleRegex = new Regex("""<article class="day-desc">(.*?)</article>""", RegexOptions.Singleline);
            var articleMatches = articleRegex.Matches(html);
            var partTask = articleMatches[unsolved-1].Value;
            External(partTask);
        }
    }
}

public interface IDaySolver
{
    string SolvePart1(string[] loadedInput);

    string SolvePart2(string[] loadedInput) => "Not implemented";
}
