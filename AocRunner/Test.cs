using System.Reflection;

using Xunit.Runners;

public static class Test
{
    // We use consoleLock because messages can arrive in parallel, so we want to make sure we get
    // consistent console output.
    static object consoleLock = new object();

    // Use an event to know when we're done
    static ManualResetEvent finished = new ManualResetEvent(false);
   

    public static void Verify(DayTests baseTest)
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
            Console.WriteLine("[FAIL] {0} {1}", info.TestDisplayName, info.ExceptionMessage.Replace("Assert.Equal() Failure", ""));
            Console.ResetColor();
        }

    }

    static void OnTestPassed(TestPassedInfo info)
    {
        lock (consoleLock)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[PASSED] {0}", info.TestDisplayName);
            Console.ResetColor();
        }
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

public class FakeHelper : ITestOutputHelper
{
    public void WriteLine(string message)
    {
        
    }

    public void WriteLine(string format, params object[] args)
    {
        
    }
}
