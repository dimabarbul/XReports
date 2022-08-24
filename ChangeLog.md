# Change Log

## 0.2.0

Released on: *YYYY-MM-DD*

**Added:**
- Benchmarks for new version (for current project code) and old version (the version specified in `csproj` file as dependency)

**Changed:**
- Improve performance and memory consumption
- Align versions of XReports.Core and XReports libraries
- Report cells are now cached and reused, so reference to cell should not be persisted anywhere; if copy of cell is needed, one can use `BaseReportCell.Clone` method to create shallow copy
