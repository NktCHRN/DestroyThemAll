using Common;
using Demo;
using Demo.Common;
using Demo.UI;
using Plotly.NET.CSharp;
using Solvers.Common;
using Solvers.Solvers.Bruteforce;
using Solvers.Solvers.Dynamic;
using Solvers.Solvers.Genetic;
using Solvers.Solvers.Greedy;

Console.ForegroundColor = ConsoleColor.DarkGreen;
IPrinter mainMenuItemPrinter;
var mainMenu = new Menu
{
    Header = Constants.Header,
    Name = "action",
    Items = new List<MenuItem>
    {
        new MenuItem
        {
            Text = "Solver"
        },
        new MenuItem
        {
            Text = "Experiments"
        },
        new MenuItem
        {
            Text = "View"
        },
    }
};
mainMenu.Print();

Console.ResetColor();
//var militaryObjects = new MilitaryObject[]
//    { new("O1", 3, 5), new("O2", 4, 7), new("O3", 6, 6), new("O4", 7, 12), new("O5", 2, 10) };
//const int maxSoldiersCount = 16;
//ISolver solver = new GeneticSolver();        // change to your solver
//var solution = solver.Solve(new Problem(militaryObjects, maxSoldiersCount));
//var sl = (SolutionModel)solution;
//Console.WriteLine($"Total objects count: {solution.MilitaryObjects.Count}; Total soldiers count: {solution.TotalSoldiersCount}; Total time: {solution.TotalTime} hours");
//Console.WriteLine("Objects:");
//foreach (var militaryObject in solution.MilitaryObjects)
//{
//    Console.WriteLine(militaryObject);
//}

//Chart.Combine(new Plotly.NET.GenericChart.GenericChart[] {Chart.Line<double, double, string>(
//    x: new double[] { 1, 2, 5 },
//    y: new double[] { 5, 10, 50 }
//).WithTraceInfo("info 1"),
//Chart.Line<double, double, string>(
//    x: new double[] { 1, 2, 5 },
//    y: new double[] { 5, 10, 50 }
//) }).WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("xAxis 111")).Show();

//Console.ReadLine();
