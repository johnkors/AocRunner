using System.Text.Json;

public static class Log
{
    public static void PrintJson(object obj)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(ToJson(obj));
        Console.Out.Flush();
        Console.ForegroundColor = prev;
    }

    public static string ToJson(object obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
    }
}
