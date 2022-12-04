using System.Text.Json;

public class Helpers
{
    public static void Print(object obj)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
        Console.ForegroundColor = prev;
    }
}
