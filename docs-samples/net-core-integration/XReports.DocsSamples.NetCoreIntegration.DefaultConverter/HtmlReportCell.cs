using XReports.Table;

internal class HtmlReportCell : ReportCell
{
    public List<string> Styles { get; set; } = new List<string>();

    public override void Clear()
    {
        base.Clear();

        this.Styles.Clear();
    }

    public override ReportCell Clone()
    {
        HtmlReportCell reportCell = (HtmlReportCell)base.Clone();

        reportCell.Styles = new List<string>(this.Styles);

        return reportCell;
    }
}
