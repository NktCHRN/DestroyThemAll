﻿namespace Demo.UI;
public sealed class MenuItem
{
    public string Text { get; set; } = string.Empty;

    public IPrinter? Printer { get; set; }
}
