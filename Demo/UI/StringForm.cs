namespace Demo.UI;

public sealed class StringForm
{
    public string? Name { get; set; }

    public Func<string?, bool>? IsValid { get; set; }

    public string? ErrorMessage { get; set; }

    public ConsoleColor Color { get; set; } = ConsoleColor.DarkGreen;

    public string GetString()
    {
        Console.ForegroundColor = Color;
        var name = !string.IsNullOrEmpty(Name) ? Name : "string";
        Console.WriteLine($"Enter the {name}: ");
        bool isValid;
        string? entered;
        do
        {
            entered = Console.ReadLine();
            isValid = IsValid?.Invoke(entered) ?? true;
            if (!isValid)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                var errorMessage = !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : "wrong value";
                Console.WriteLine($"Error: {errorMessage}");
                Console.ForegroundColor = Color;
                Console.WriteLine($"Please, enter the {name} once more");
            }
        }
        while (!isValid);
        return entered ?? string.Empty;
    }
}
