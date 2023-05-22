using Plotly.NET.CSharp;
using Solvers.Common;
using Solvers.Solvers.Bruteforce;
using Solvers.Solvers.Dynamic;
using Solvers.Solvers.Genetic;
using Solvers.Solvers.Greedy;

var militaryObjects = new MilitaryObject[]
    { new("O1", 3, 5), new("O2", 4, 7), new("O3", 6, 6), new("O4", 7, 12), new("O5", 2, 10) };
const int maxSoldiersCount = 16;
ISolver solver = new BruteforceSolver();        // change to your solver
var solution = solver.Solve(militaryObjects, maxSoldiersCount);
Console.WriteLine($"Total objects count: {solution.MilitaryObjects.Count()}; Total soldiers count: {solution.TotalSoldiersCount}; Total time: {solution.TotalTime} hours");
Console.WriteLine("Objects:");
foreach (var militaryObject in solution.MilitaryObjects)
{
    Console.WriteLine(militaryObject);
}

//Chart.Line<double, double, string>(
//    x: new double[] { 1, 2, 5 },
//    y: new double[] { 5, 10, 50 }
//)
//.WithTraceInfo("Hello from C# 1", ShowLegend: true)
//.WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("xAxis"))
//.WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("yAxis"))
//.Show();
//Chart.Line<double, double, string>(
//    x: new double[] { 1, 2, 5 },
//    y: new double[] { 5, 10, 50 }
//)
//.WithTraceInfo("Hello from C# 2", ShowLegend: true)
//.WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("xAxis"))
//.WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("yAxis"))
//.Show();

Console.ReadLine();
