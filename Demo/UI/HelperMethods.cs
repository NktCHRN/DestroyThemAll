namespace Demo.UI;

public static class HelperMethods
{
    public static void Quit()
    {
        Console.WriteLine("Press enter co quit");
        Console.ReadLine();
    }

    public static void Continue()
    {
        Console.WriteLine("Press enter co continue");
        Console.ReadLine();
    }
    public static void Wait()
    {
        Console.WriteLine("Please, keep calm and wait for it...");
    }

    public static void PrintHeader(string header)
    {
        Console.WriteLine($"{header}{Environment.NewLine}");
    }
}
