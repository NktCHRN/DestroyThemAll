using System.Diagnostics.CodeAnalysis;

namespace Demo.UI;

public delegate bool TryParseHandler<T>(string? value, out T result);
public sealed class NumberForm<T> where T : struct, IComparable<T>
{
    private ComparisonType _minComparisonType;
    private T? _min;

    private ComparisonType _maxComparisonType;
    private T? _max;

    public string? Name { get; set; }

    public TryParseHandler<T>? Parser { get; set; } = null;

    public Func<string, string>? StringHandler { get; set; }

    public ConsoleColor Color { get; set; } = ConsoleColor.DarkGreen;

    private string RestrictionString
    {
        get
        {
            var restrictionString = string.Empty;
            if (_min is not null)
                restrictionString += $"greater than {(_minComparisonType is ComparisonType.Loose ? "or equal to " : string.Empty)}{_min}";
            if (_max is not null)
                restrictionString += $"{(_min is null ? " and" : string.Empty)} less than {(_maxComparisonType is ComparisonType.Loose ? "or equal " : string.Empty)}{_max}";
            return restrictionString;
        }
    }

    public NumberForm<T> WithMinGreaterThan(T min)
    {
        _minComparisonType = ComparisonType.Strict;
        _min = min;
        return this;
    }

    public NumberForm<T> WithMinGreaterThanOrEqualTo(T min)
    {
        _minComparisonType = ComparisonType.Loose;
        _min = min;
        return this;
    }

    public NumberForm<T> WithMaxLessThan(T max)
    {
        _maxComparisonType = ComparisonType.Strict;
        _max = max;
        return this;
    }

    public NumberForm<T> WithMaxLessThanOrEqualTo(T max)
    {
        _maxComparisonType = ComparisonType.Loose;
        _max = max;
        return this;
    }

    public T GetNumber()
    {
        if (Parser is null)
            SetParser();
        Console.ForegroundColor = Color;
        var getString = () =>
        {
            var entered = Console.ReadLine();
            if (entered != null && StringHandler != null)
                entered = StringHandler.Invoke(entered);
            return entered;
        };
        var name = !string.IsNullOrEmpty(Name) ? Name : "number";
        Console.WriteLine($"Enter the {name} that is {RestrictionString}: ");
        bool parsed;
        if (Parser is null)
            throw new InvalidOperationException("Handler cannot be null");
        parsed = Parser.Invoke(getString(), out T entered);
        while (!parsed || !IsValid(entered))
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Error: wrong {name}");
            Console.ForegroundColor = Color;
            Console.WriteLine($"Please, enter the {name} that is {RestrictionString} once more: ");
            parsed = Parser.Invoke(getString(), out entered);
        }
        return entered;
    }

    [MemberNotNull(nameof(Parser))]
    private void SetParser()
    {
        var type = typeof(T);
        var method = type.GetMethod(
            "TryParse",
            new[]
            {
                typeof (string),
                Type.GetType(string.Format("{0}&", type.FullName))
            }!) ?? throw new ArgumentNullException(nameof(Parser), "Provide a Parser for your type");
        Parser = (TryParseHandler<T>)method.CreateDelegate(typeof(TryParseHandler<T>));
    }

    private bool IsValid(T entered)
    {
        if (_min is not null)
        {
            var comparisonResult = entered.CompareTo((T)_min);
            if (comparisonResult < 0 && _minComparisonType is ComparisonType.Loose || comparisonResult <= 0 && _minComparisonType is ComparisonType.Strict)
                return false;
        }
        if (_max is not null)
        {
            var comparisonResult = entered.CompareTo((T)_max);
            if (comparisonResult > 0 && _maxComparisonType is ComparisonType.Loose || comparisonResult >= 0 && _maxComparisonType is ComparisonType.Strict)
                return false;
        }
        return true;
    }
}
