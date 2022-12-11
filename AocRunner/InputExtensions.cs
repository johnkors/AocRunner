using System.Text.RegularExpressions;

public static class InputExtensions
{
    public static (int endRowNumber, IEnumerable<string>) GetRowsUntilInReverse(this IEnumerable<string> rows, string row)
    {
        using IEnumerator<string> rowReader = rows.AsEnumerable().GetEnumerator();
        var rowsToFirstEmpty = new List<string>();
        var rowNo = 1;
        while (rowReader.MoveNext())
        {
            rowsToFirstEmpty = rowsToFirstEmpty.Prepend(rowReader.Current).ToList();
            if (rowReader.Current == row)
            {
                break;
            }

            rowNo++;
        }

        return (rowNo, rowsToFirstEmpty);
    }
    
    public static IEnumerable<T> GetByPattern<T>(this IEnumerable<string> rows, string pattern, Func<Match, T> matchConverter)
    {
        var regex = new Regex(pattern, RegexOptions.Compiled);
        using IEnumerator<string> enumerator = rows.AsEnumerable().GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (regex.Match(enumerator.Current) is { Success: true } match)
            {
                yield return matchConverter(match);
            }
        }
    }

    public static int AsInt(this Match m, int i)
    {
        return int.Parse(m.Groups[i].Value);
    }
}
