using System.Text.Json;

public static class Log
{
    public static void PrintJson(object obj)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
        Console.Out.Flush();
        Console.ForegroundColor = prev;
    }
}
