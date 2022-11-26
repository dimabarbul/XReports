# Cell Types

XReports provides 2 types of cells:

```c#
public class HtmlReportCell : BaseReportCell
{
    public bool IsHtml { get; set; }

    public HashSet<string> CssClasses { get; set; } =
        new HashSet<string>();

    public Dictionary<string, string> Styles { get; set; } =
        new Dictionary<string, string>();

    public Dictionary<string, string> Attributes { get; set; } =
        new Dictionary<string, string>();
    
    public override void Clear()
    {
        base.Clear();

        this.IsHtml = false;
        this.CssClasses.Clear();
        this.Styles.Clear();
        this.Attributes.Clear();
    }
}

public class ExcelReportCell : BaseReportCell
{
    public Alignment? HorizontalAlignment { get; set; }

    public string NumberFormat { get; set; }

    public bool IsBold { get; set; }

    public Color? BackgroundColor { get; set; }

    public Color? FontColor { get; set; }
    
    public override void Clear()
    {
        base.Clear();

        this.HorizontalAlignment = null;
        this.NumberFormat = null;
        this.IsBold = false;
        this.BackgroundColor = null;
        this.FontColor = null;
    }
}
```

While you can work with these types, it's recommended to create your own type (you may inherit it from HtmlReportCell or ExcelReportCell) and use it. The reason is that if you want to add new properties to your report cell later you will need to register converter, writers etc. for your cell type, and if you use your cell type from the beginning, you won't need any changes.
