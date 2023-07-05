# Cell Types

XReports provides 2 types of cells: HtmlReportCell and ExcelReportCell.

While you can work with these types, it's recommended to create your own type (you may inherit it from HtmlReportCell or ExcelReportCell). The reason is that if you want to add new properties to your report cell later you will need to update registration for converter, writers etc. for your cell type, while if you use your cell type from the beginning, you won't need any changes.
