using System.Text.RegularExpressions;

namespace Y2023;

public class Day3 : IDaySolver
{
    public string SolvePart1(string[] loadedInput)
    {
        var paddedInput = PaddedInput(loadedInput);
        var data = GetData(paddedInput);

        return data.Numbers.Where(n => n.Adjacent.Any()).Sum(n => n.Value).ToString();
    }

    public string SolvePart2(string[] loadedInput)
    {
        var paddedInput = PaddedInput(loadedInput);
        var data = GetData(paddedInput);

        return data.Symbols
            .Where(g => g is { Value: "*", Adjacent.Count: 2})
            .Sum(s => s.Adjacent.Select(n => n.Value).Aggregate((x, y) => x * y))
            .ToString();
    }

    private void CalculateAdjacency(Number number, List<Symbol> dataSymbols)
    {
        foreach (Symbol symbol in dataSymbols)
        {
            if (symbol.RowIndex == number.RowIndex)
            {
                var first = number.ColumnIndices.First();
                var last = number.ColumnIndices.Last();
                var isLeftOfSymbol = symbol.ColumnIndex == first - 1;
                var isRightOfSymbol = symbol.ColumnIndex == last + 1;
                var isHorizontalAdjacent = isLeftOfSymbol || isRightOfSymbol;
                if (isHorizontalAdjacent)
                {
                    symbol.AddAdjacentNumber(number);
                    number.AddAdjacentSymbol(symbol);
                }
            }

            if(symbol.RowIndex == number.RowIndex - 1 || symbol.RowIndex == number.RowIndex + 1 )
            {
                var first = number.ColumnIndices.First();
                var last = number.ColumnIndices.Last();
                var isLeftOfSymbol = symbol.ColumnIndex == first - 1;
                var isRightOfSymbol = symbol.ColumnIndex == last + 1;
                var isHorizontallyAdjacent = isLeftOfSymbol || isRightOfSymbol;
                var isVerticallyAdjacent = number.ColumnIndices.Any(i => i == symbol.ColumnIndex);
                if (isHorizontallyAdjacent || isVerticallyAdjacent)
                {
                    symbol.AddAdjacentNumber(number);
                    number.AddAdjacentSymbol(symbol);
                }
            }
        }
    }

    internal (List<Number> Numbers, List<Symbol> Symbols) GetData(string[] paddedInput)
    {
        int rowId = 0;
        List<Number> allNumbers = [];
        List<Symbol> allSymbols = [];
        foreach (string row in paddedInput)
        {
            var numberMatches = Regex.Matches(row, @"\d+");
            if (numberMatches.Any())
            {
                List<Number> nList = [];
                foreach (Match num in numberMatches)
                {
                    int[] indices = [];
                    var numberIndex = num.Index;

                    for (int i = 0; i < num.Length; i++)
                    {
                        indices = indices.Append(numberIndex).ToArray();
                        numberIndex++;
                    }

                    var number = new Number(rowId, indices, int.Parse(num.Value));
                    nList.Add(number);
                }
                allNumbers.AddRange(nList);
            }

            var symbolMatches = Regex.Matches(row, @"[^\d|.]");
            if (symbolMatches.Any())
            {
                List<Symbol> sList = [];
                foreach (Match s in symbolMatches)
                {
                    sList.Add(new Symbol(rowId, s.Index, s.Value));
                }
                allSymbols.AddRange(sList);
            }

            rowId++;
        }

        foreach (var number in allNumbers)
        {
            CalculateAdjacency(number, allSymbols);
        }

        return (Numbers: allNumbers, Symbols: allSymbols);
    }

    internal record Symbol(int RowIndex, int ColumnIndex, string Value)
    {
        public List<Number> Adjacent = new();

        public void AddAdjacentNumber(Number number)
        {
            Adjacent.Add(number);
        }
    }

    internal record Number(int RowIndex, int[] ColumnIndices, int Value)
    {
        public List<Symbol> Adjacent = new();

        public void AddAdjacentSymbol(Symbol number)
        {
            Adjacent.Add(number);
        }
    }

    private string[] PaddedInput(string[] loadedInput)
    {
        string[] paddedInput = [];
        for (int i = 0; i < loadedInput.Length; i++)
        {
            var row = loadedInput[i];
            if (i == 0)
            {
                paddedInput = paddedInput.Append(new String('.', row.Length + 2)).ToArray();
                paddedInput = paddedInput.Append($".{row}.").ToArray();
            }
            else if(i == loadedInput.Length - 1)
            {
                paddedInput = paddedInput.Append($".{row}.").ToArray();
                paddedInput = paddedInput.Append(new String('.', row.Length + 2)).ToArray();
            }
            else
            {
                paddedInput = paddedInput.Append($".{row}.").ToArray();
            }
        }

        return paddedInput;
    }
}
