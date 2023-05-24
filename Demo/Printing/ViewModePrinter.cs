using Common;
using Demo.UI;
using Newtonsoft.Json;
using static Demo.Printing.Common.PrintingHelperMethods;

namespace Demo.Printing;
public sealed class ViewModePrinter : IPrinter
{
    public void Print()
    {
        HelperMethods.PrintHeader(Constants.Header);
        Console.WriteLine("What do you want to view?");
        new LiteMenu
        {
            IsQuitable = true,
            Name = "option",
            Items = new LiteMenuItem[]
            {
                new LiteMenuItem
                {
                    Text = "Problem from json file",
                    Action = ViewProblem
                },
                new LiteMenuItem
                {
                    Text = "Solution from json file",
                    Action = ViewSolution
                }
            },
            Callback = () =>
            {
                new Dialog
                {
                    Question = "Do you want to view something else?",
                    YAction = Print,
                    NAction = HelperMethods.Quit
                }.Print();
            }
        }.Print();
    }

    private void ViewProblem()
    {
        Console.WriteLine();
        var fileName = new StringForm
        {
            Name = "json file name",
            IsValid = f => File.Exists(f),
            ErrorMessage = "File does not exist"
        }.GetString();

        var fileData = File.ReadAllText(fileName);
        var problem = JsonConvert.DeserializeObject<Problem>(fileData) ?? throw new InvalidOperationException("JSON object is null");

        Console.WriteLine();
        PrintProblem(problem);
        Console.WriteLine();
    }

    private static void ViewSolution()
    {
        Console.WriteLine();
        var fileName = new StringForm
        {
            Name = "json file name",
            IsValid = f => File.Exists(f),
            ErrorMessage = "File does not exist"
        }.GetString();

        var fileData = File.ReadAllText(fileName);
        var solution = JsonConvert.DeserializeObject<SolutionModel>(fileData) ?? throw new InvalidOperationException("JSON object is null");

        Console.WriteLine();
        Console.WriteLine($"Total objects count: {solution.TotalMilitaryObjectsCount}; Total time: {solution.TotalTime} hours");
        Console.WriteLine($"Total soldiers count: {solution.TotalSoldiersCount}");
        Console.WriteLine("Objects:");
        var j = 1;
        foreach (var militaryObject in solution.MilitaryObjects)
        {
            Console.WriteLine($"{j}. Name: {militaryObject.Name}; Time: {militaryObject.Time}; Soldiers count: {militaryObject.SoldiersCount}");
            j++;
        }
        Console.WriteLine();

        new Dialog
        {
            Question = "Do you want to view one more solution?",
            YAction = ViewSolution
        }.Print();
    }
}
