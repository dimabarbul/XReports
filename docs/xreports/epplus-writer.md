# EpplusWriter

EpplusWriter class provides ability to write Excel report to XLSX file (or stream with XLSX content) using [Epplus 4](https://github.com/JanKallman/EPPlus).

EpplusWriter class **is not** thread safe, i.e., you should not use one instance to write several reports in parallel.

## .NET Core Integration

You need to call extension method AddEpplusWriter. There are 3 forms of this method.

```c#
// Registers EpplusWriter as implementation of IEpplusWriter.
services.AddEpplusWriter();

class MyExcelWriter : EpplusWriter {}
// Registers derived class of EpplusWriter as implementation of IEpplusWriter.
// It may be useful if you want to extend/override EpplusWriter methods.
services.AddEpplusWriter<MyExcelWriter>();

interface IExtendedExcelWriter : IEpplusWriter {}
class ExtendedExcelWriter : EpplusWriter, IExtendedExcelWriter {}
// Registers derived class of EpplusWriter as implementation of IEpplusWriter
// and your interface inherited from IEpplusWriter.
// It may be useful if you want to add method(s) to EpplusWriter.
services.AddEpplusWriter<IExtendedExcelWriter, ExtendedExcelWriter>();
```

All of the forms accept 2 optional arguments:
- configuration callback to configure EpplusWriter options
- service lifetime - it applies to EpplusWriter class and all formatters provided in configuration callback, by default it's scoped

Example:

```c#
// Registers EpplusWriter along with all formatters (implementors of
// IEpplusFormatter interface) from executing assembly as singletons.
services.AddEpplusWriter(
    o => o.AddFromAssembly(Assembly.GetExecutingAssembly()),
    ServiceLifetime.Singleton);
```

## Public Methods

There are 2 public methods to export report:

- **WriteToFile**: saves report as XLSX file
- **WriteToStream**: saves report to new in-memory stream or the one provided

## Extending EpplusWriter

[Working example](../../docs-samples/epplus-writer/XReports.DocsSamples.EpplusWriter.ExtendingEpplusWriter/Program.cs)

EpplusWriter class contains number of virtual methods you can override.

General Flow:

- WriteToFile/WriteToStream
    - WriteReportToWorksheet (row=1, column=1)
        - WriteHeader
            - WriteHeaderCell
                - WriteCell
                    - FormatCell
            - FormatHeader
        - WriteBody
            - WriteCell (for all rows for all columns)
                - FormatCell (if cell does not have SameColumnFormatProperty)
            - ApplyColumnFormat
                - FormatCell (for columns that has SameColumnFormatProperty)
        - PostCreate

All methods in above list lower WriteReportToWorksheet can be overridden.

The writer has some configuration properties that can be overridden in inherited class:

- WorksheetName - "Data" by default
- StartColumn - 1 by default
- StartRow - 1 by default

For example, by default report does not have border around the table. To add it, EpplusWriter can be extended.

```c#
class MyEpplusWriter : EpplusWriter
{
    protected override void PostCreate(ExcelWorksheet worksheet, ExcelAddress headerAddress, ExcelAddress bodyAddress)
    {
        base.PostCreate(worksheet, headerAddress, bodyAddress);

        worksheet.Cells[headerAddress.Address].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        worksheet.Cells[bodyAddress.Address].Style.Border.BorderAround(ExcelBorderStyle.Thin);
    }
}
```

## Using Extra Excel Features

EpplusWriter supports limited list of features of Excel cells:
- horizontal alignment
- number format
- bold font
- font and background color

If you want to use more Excel features to apply your cell properties, you have 2 options:
- extend EpplusWriter to handle custom properties
- create formatter class

### Extending EpplusWriter for New Feature

[Working example](../../docs-samples/epplus-writer/XReports.DocsSamples.EpplusWriter.ExtendingEpplusWriterForNewFeature/Program.cs)

```c#
// Property marking cells that should be indented.
class IndentationProperty : ReportCellProperty { }

class MyEpplusWriter : EpplusWriter
{
    // Override method to handle custom property.
    protected override void FormatCell(ExcelRange worksheetCell, ExcelReportCell cell)
    {
        base.FormatCell(worksheetCell, cell);

        // As we don't have handler of this property during conversion to Excel report
        // the property will remain in cell.
        IndentationProperty indentationProperty = cell.GetProperty<IndentationProperty>();
        if (indentationProperty != null)
        {
            // Indent cell content.
            worksheetCell.Style.Indent = 1;
            worksheetCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        }
    }
}
```

### Creating Formatter for New Feature

[Working example](../../docs-samples/epplus-writer/XReports.DocsSamples.EpplusWriter.CreatingFormatterForNewFeature/Program.cs)

```c#
// Property marking cells that should be indented.
class IndentationProperty : ReportCellProperty { }

private class ExcelIndentationPropertyFormatter : EpplusFormatter<IndentationProperty>
{
    protected override void Format(ExcelRange worksheetCell, ExcelReportCell cell, IndentationProperty property)
    {
        worksheetCell.Style.Indent = 1;
        worksheetCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
    }
}

// If you use DI, add ExcelIndentationPropertyFormatter during EpplusWriter registration.
services.AddEpplusWriter(o => o.Add(typeof(ExcelIndentationPropertyFormatter)));
// If you don't use DI, just create instance.
IEpplusWriter writer = new EpplusWriter(new[]
{
    new ExcelIndentationPropertyFormatter(),
});
```
