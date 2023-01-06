public class Day6 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        string longStringAsString = loadedInput.First();

        for (int i = 4; i < longStringAsString.Length; i++)
        {
            var last4 = longStringAsString.Substring(i - 4, 4).ToCharArray();
            var isMarker = last4.Length == last4.Distinct().Count();
            if (isMarker)
                return i.ToString();
        }

        return "0";
    }
    
    public string SolvePart2(string[] loadedInput)
    {
        string longStringAsString = loadedInput.First();

        for (int i = 14; i < longStringAsString.Length; i++)
        {
            var last4 = longStringAsString.Substring(i - 14, 14).ToCharArray();
            
            var isMarker = last4.Length == last4.Distinct().Count();
            if (isMarker)
                return i.ToString();
        }

        return "0";
    }
}
