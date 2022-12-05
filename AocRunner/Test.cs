using System.Reflection;

using Xunit.Runners;

public static class Test
{
    // We use consoleLock because messages can arrive in parallel, so we want to make sure we get
    // consistent console output.
    static object consoleLock = new object();

    // Use an event to know when we're done
    static ManualResetEvent finished = new ManualResetEvent(false);

    // Start out assuming success; we'll set this to 1 if we get a failed test
    static int result = 0;

    public static void Verify<T>(DayTests<T> baseTest) where T: IDaySolver, new()
    {
        using var runner = AssemblyRunner.WithoutAppDomain(Assembly.GetExecutingAssembly().Location);
        runner.OnDiscoveryComplete = OnDiscoveryComplete;
        runner.OnExecutionComplete = OnExecutionComplete;
        runner.OnTestFailed = OnTestFailed;
        runner.OnTestSkipped = OnTestSkipped;
        runner.OnTestPassed = OnTestPassed;

        Console.WriteLine("Discovering...");
        runner.Start(baseTest.GetType().Name);

        finished.WaitOne();
        finished.Dispose();

        if(result > 0)
            System.Environment.Exit(result);
    }

    static void OnDiscoveryComplete(DiscoveryCompleteInfo info)
    {
        lock (consoleLock)
            Console.WriteLine($"Running {info.TestCasesToRun} of {info.TestCasesDiscovered} tests...");
    }

    static void OnExecutionComplete(ExecutionCompleteInfo info)
    {
        lock (consoleLock)
            Console.WriteLine($"Finished: {info.TotalTests} tests in {Math.Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed, {info.TestsSkipped} skipped)");

        finished.Set();
    }

    static void OnTestFailed(TestFailedInfo info)
    {
        lock (consoleLock)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("[FAIL] {0}: {1}", info.TestDisplayName, info.ExceptionMessage);
            if (info.ExceptionStackTrace != null)
                Console.WriteLine(info.ExceptionStackTrace);

            Console.ResetColor();
        }

        result = 1;
    }

    static void OnTestPassed(TestPassedInfo info)
    {
        lock (consoleLock)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[PASSED] {0}: {1}", info.TestDisplayName, info.Output);
            Console.ResetColor();
        }

        result = 0;
    }

    static void OnTestSkipped(TestSkippedInfo info)
    {
        lock (consoleLock)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[SKIP] {0}: {1}", info.TestDisplayName, info.SkipReason);
            Console.ResetColor();
        }
    }
}
