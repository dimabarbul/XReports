# Change Log

## 0.2.0

Released on: *YYYY-MM-DD*

**Added:**
- Benchmarks for new version (for current project code) and old version (the version specified in `csproj` file as dependency)
- `HtmlStreamWriterDI.AddHtmlStreamWriter` and `HtmlStreamWriterDI.AddHtmlStreamCellWriter` to register Html stream writer in DI
- `IHtmlStreamWriter`/`IHtmlStreamCellWriter` and `HtmlStreamWriter`/`HtmlStreamCellWriter`
- New method `BaseReportCell.Clear` resets report cell to initial state, it should be overriden in derived classes

**Changed:**
- Improved performance and memory consumption
- Aligned versions of XReports.Core and XReports libraries
- Report cells are now cached and reused, so reference to cell should not be persisted anywhere; if copy of cell is needed, one can use `BaseReportCell.Clone` method to create shallow copy
- Properties now are processed by the first property handler that marks them processed, i.e., the property is not processed after it's been marked as processed
- Replaced `BaseReportCell.Value` setter with `BaseReportCell.SetValue<TValue>` method, getter – with `BaseReportCell.GetUnderlyingValue` method
- `IPropertyHandler.Handle` method is now `bool`: it should return `true` if the property has been processed, `false` otherwise
- Renamed classes and interfaces:
  - IStringWriter ⇒ IHtmlStringWriter
    - WriteToStringAsync ⇒ WriteToString
    - removed method `WriteToFileAsync`, `IHtmlStreamWriter` should be used instead
  - IStringCellWriter ⇒ IHtmlStringCellWriter:
    - methods now accept `StringBuilder` instead of returning `string`
  - StringWriter ⇒ HtmlStringWriter
  - StringCellWriter ⇒ HtmlStringCellWriter
  - StringWriterDI ⇒ HtmlStringWriterDI:
    - AddStringWriter ⇒ AddHtmlStringWriter
    - AddStringCellWriter ⇒ AddHtmlStringCellWriter
  - IEpplusWriter:
    - added method `WriteToStream` that accepts existing stream
  - `IReportCellsProvider.CellSelector` property is replaced with `IReportCellsProvider.GetCell` method

**Fixed:**
- Global properties are available to cell processors

**Removed:**
- `ReportCellProperty.Processed` property
