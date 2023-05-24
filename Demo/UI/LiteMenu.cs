namespace Demo.UI;

public sealed class LiteMenu
{
    public string? Name { get; set; }

    public bool IsQuitable { get; set; }

    public ConsoleColor Color { get; set; } = ConsoleColor.DarkGreen;

    public ICollection<LiteMenuItem> Items { get; set; } = new List<LiteMenuItem>();

    public Action? Callback { get; set; }

    public void Print()
    {
        if (Items is not null && Items.Any())
        {
            var name = !string.IsNullOrEmpty(Name) ? Name : "item";
            var min = Convert.ToInt32(!IsQuitable);
            Console.WriteLine($"Please, select the {name}:");
            var texts = Items.Select(i => i.Text);
            for (int i = 0; i < texts.Count(); i++)
                Console.WriteLine($"{i + 1}. {texts.ElementAt(i)}");
            if (IsQuitable)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"0. Quit{Environment.NewLine}");
                Console.ForegroundColor = Color;
            }
            Console.WriteLine($"Select the number from {min} to {Items.Count}: ");
            bool parsed;
            parsed = int.TryParse(Console.ReadLine(), out int selected);
            while (!parsed || selected < min || selected > Items.Count)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error: wrong number");
                Console.ForegroundColor = Color;
                Console.WriteLine($"Please, select the number from {min} to {Items.Count} once more");
                parsed = int.TryParse(Console.ReadLine(), out selected);
            }
            if (selected != 0)
            {
                selected--;
                Items.ElementAt(selected)?.Action?.Invoke(); 
                Callback?.Invoke();
            }
        }
    }
}
