using System.Text.Json;

namespace AocRunner;

public class Helpers
{
    public static void PrintJson(object obj)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
        Console.ForegroundColor = prev;
    }
}
