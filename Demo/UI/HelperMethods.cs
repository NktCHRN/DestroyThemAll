namespace Demo.UI;

public static class HelperMethods
{
    public static void Continue()
    {
        Console.WriteLine("Press enter co continue");
        Console.ReadLine();
    }

    public static void PrintHeader(string header)
    {
        Console.WriteLine($"{header}{Environment.NewLine}");
    }
}
