namespace Demo.UI;

public sealed class LiteMenuItem
{
    public string Text { get; set; } = string.Empty;

    public Action? Action {get; set;}
}
