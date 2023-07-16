using XReports.Table;

internal class HtmlReportCell : ReportCell
{
    public List<string> CssClasses { get; set; } = new List<string>();
    public List<string> Styles { get; set; } = new List<string>();

    public override void Clear()
    {
        base.Clear();

        this.CssClasses.Clear();
        this.Styles.Clear();
    }

    public override ReportCell Clone()
    {
        HtmlReportCell reportCell = (HtmlReportCell)base.Clone();

        reportCell.CssClasses = new List<string>(this.CssClasses);
        reportCell.Styles = new List<string>(this.Styles);

        return reportCell;
    }
}
