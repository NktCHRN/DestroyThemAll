using Demo;
using Demo.Printing;
using Demo.UI;
using System.Globalization;

Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
Console.ForegroundColor = ConsoleColor.DarkGreen;
var mainMenu = new Menu
{
    Header = Constants.Header,
    Name = "action",
    Items = new MenuItem[]
    {
        new MenuItem
        {
            Text = "Solver",
            Printer = new SolverPrinter()
        },
        new MenuItem
        {
            Text = "Experiments",
            Printer = new ExperimentsPrinter()
        },
        new MenuItem
        {
            Text = "View",
            Printer = new ViewModePrinter()
        },
    }
};
mainMenu.Print();

Console.ResetColor();
